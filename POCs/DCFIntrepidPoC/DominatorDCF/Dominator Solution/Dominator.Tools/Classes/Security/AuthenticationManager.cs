// <copyright file="AuthenticationManager.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Tools.Classes.Security
{
    /// <summary>
    /// The service side of the wcf secure communication.
    /// </summary>
    public class AuthenticationManager
    {
        /// <summary>
        /// Single instance.
        /// </summary>
        public static readonly AuthenticationManager Singleton = new AuthenticationManager();

        /// <summary>
        /// Pipe name.
        /// </summary>
        internal static readonly string AuthPipeName = "DominatorSPipe";

        /// Certication candidate subject of dell.
        /// </summary>
        private static readonly string DellCertSubject1 = "CN=Dell Inc.";

        /// <summary>
        /// Certication candidate subject of dell.
        /// </summary>
        private static readonly string DellCertSubject2 = "CN=Dell Inc";

        /// <summary>
        /// Certication candidate subject of dell.
        /// </summary>
        private static readonly string DellCertSubject3 = "CN=Dell";

        /// <summary>
        /// All registered WCF endpoints.
        /// key = friendly endpoint, value = actual endpoint.
        /// e.g.: key = netpipe://localhost/abc, value: netpipe://localhost/6FA3705C-B424-466F-8D52-E2F1A160039F
        /// </summary>
        private readonly ConcurrentDictionary<string, string> endpoints = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Random token and associate with an appId and a friendly endpoint url.
        /// </summary>
        private readonly ConcurrentDictionary<string, byte[]> apiTokens = new ConcurrentDictionary<string, byte[]>();

        /// <summary>
        /// Clients who have been authenticated.
        /// </summary>
        private readonly ConcurrentDictionary<InstanceContext, int> clients = new ConcurrentDictionary<InstanceContext, int>();
        
        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        /// <summary>
        /// Indicating authentication service is running.
        /// </summary>
        private volatile bool isRunning;

        /// <summary>
        /// Indicating authentication service is running.
        /// </summary>
        public bool IsRuning => isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationManager" /> class.
        /// </summary>
        private AuthenticationManager() { }

        /// <summary>
        /// Register WCF endpoints.
        /// </summary>
        /// <param name="apiUrl">friendly endpoint url</param>
        /// <param name="endpoint">actual endpoint url</param>
        public void RegisterEndpoint(string apiUrl, string endpoint)
        {
            endpoints.TryAdd(apiUrl, endpoint);
        }

        /// <summary>
        /// Start the authentication service.
        /// </summary>
        public void StartAsync()
        {
            if (isRunning)
                return;

            isRunning = true;

            ThreadPool.QueueUserWorkItem((o) => setupAuthPipeListener());
        }

        /// <summary>
        /// checks to see if the downloaded file has been digitally signed with Dell's certificate
        /// </summary>
        /// <param name="filePath">The path of the file to be checked</param>
        /// <returns>true if the binary is signed by dell, false if it's not signed or signed by someone other than dell</returns>
        public bool IsSigned(string filePath)
        {
#if DEBUG
            return true;
#endif

            bool result = false;
            Tryblock.Run(() =>
            {
                var signInfo = SigningHelper.GetSigningInfo(filePath);
                bool isValidCert = (signInfo.Count > 0);

                // try to impersonate current user if verify fails.
                if (!isValidCert)
                {
                    Impersonator.RunImpersonated(() =>
                    {
                        signInfo = SigningHelper.GetSigningInfo(filePath);
                        isValidCert = (signInfo.Count > 0);
                    });
                }

                if (isValidCert)
                {
                    // verify the subject with possible dell subjects.
                    for (int i = 0; i < signInfo.Count; i++)
                    {
                        string[] subjects = signInfo[i].Certificate.Subject.Split(',').Select(s => s.Trim()).ToArray();

                        result =
                            (subjects.Contains(DellCertSubject1)
                            || subjects.Contains(DellCertSubject2)
                            || subjects.Contains(DellCertSubject3));

                        if (result)
                        {
                            break;
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Verifying wether the token is valid.
        /// </summary>
        /// <param name="tokenKey">key of the token</param>
        /// <param name="token">given token</param>
        /// <returns>True or False</returns>
        internal bool VerifyToken(string tokenKey, byte[] token)
        {
            byte[] expected = null;
            if (apiTokens.TryGetValue(tokenKey, out expected))
            {
                return expected.SequenceEqual(token);
            }

            return false;
        }

        /// <summary>
        /// Save the WCF client instance context.
        /// </summary>
        /// <param name="instanceContext">given instance</param>
        internal void CacheInstanceContext(InstanceContext instanceContext)
        {
            int ingore = 0;
            clients.TryAdd(instanceContext, 0);
            clients.Where(c => c.Key.State == CommunicationState.Faulted || c.Key.State == CommunicationState.Closing || c.Key.State == CommunicationState.Closed)
                .ToList().ForEach(c =>
                {
                    clients.TryRemove(c.Key, out ingore);
                });

            logger?.WriteLine($"clients count {clients.Count}");
        }

        /// <summary>
        /// Check whether the WCF client instance context has been cached.
        /// </summary>
        /// <param name="instanceContext">given instance</param>
        /// <returns>True or False</returns>
        internal bool InstanceContextExist(InstanceContext instanceContext)
        {
            return clients.ContainsKey(instanceContext);
        }

        /// <summary>
        /// Remove token from cache by given key.
        /// </summary>
        /// <param name="tokenKey">key of token</param>
        internal void RemoveToken(string tokenKey)
        {
            byte[] expected = null;
            apiTokens.TryRemove(tokenKey, out expected);
        }

        /// <summary>
        /// Create a namepipe for authentication purpose.
        /// </summary>
        /// <returns>The pipe stream.</returns>
        private NamedPipeServerStream createAuthPipe()
        {
            var ps = new PipeSecurity();
            // Granting Read/Write access to all Authenticated SIDs. 
            // This also denys any Authenticated UserSid from creating named pipes with this same name. 
            var sid1 = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            var par1 = new PipeAccessRule(sid1, PipeAccessRights.ReadWrite, AccessControlType.Allow);
            ps.AddAccessRule(par1);

            //
            // Denying access to connections coming over the network.
            // Connections made from within a Remote Desktop (RDP) session still work. This is the behavior we want.
            //
            var sid2 = new SecurityIdentifier(WellKnownSidType.NetworkSid, null);
            var par2 = new PipeAccessRule(sid2, PipeAccessRights.FullControl, AccessControlType.Deny);
            ps.AddAccessRule(par2);


            var par3 = new PipeAccessRule(WindowsIdentity.GetCurrent().Name, PipeAccessRights.FullControl, AccessControlType.Allow);
            ps.AddAccessRule(par3);

            var authPipe = new NamedPipeServerStream(AuthPipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances,
                                        PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 0, 0, ps);

            return authPipe;
        }

        /// <summary>
        /// Create a pipe and listen on it for coming authentication request.
        /// </summary>
        private void setupAuthPipeListener()
        {
            // Could suppress Fortify warning here, Category: Unreleased Resource: Streams
            // Because we dispose the stream when exception happens.


            // Create the authentication pipe
            while (true)
            {
                var authPipe = createAuthPipe();

                try
                {
                    // Wait for clients to authenticate
                    authPipe.WaitForConnection();
                }
                catch (IOException)
                {
                    authPipe.Disconnect();
                    authPipe.Close();
                    continue;
                }

                // Client connected. Process client.
                ThreadPool.QueueUserWorkItem(state =>
                {
                    // we need a separate variable here, so as not to make the lambda capture the pipeClientConnection variable 
                    using (var pipeClientConn = (NamedPipeServerStream)state)
                    {
                        Tryblock.Run(
                            () => authPipeWorker(pipeClientConn),
                            finallyAction: () => Tryblock.Run(() => pipeClientConn.Close())
                        );
                    }
                }, authPipe);
            }
        }

        /// <summary>
        /// The authentication work flow on the server side.
        /// </summary>
        /// <param name="pipe">The authenctication namepipe</param>
        private void authPipeWorker(NamedPipeServerStream pipe)
        {
            UInt32 processId = 0;
            NamedPipeServerStream authPipe = pipe;
            if (!NativeMethods.GetNamedPipeClientProcessId(authPipe.SafePipeHandle.DangerousGetHandle(), out processId))
                return;

            string clientExcutablePath = getExecuableByProcessId(processId);

            // Check if client is signed.
            if (!IsSigned(clientExcutablePath))
            {
                logger?.WriteLine($"The caller process:{clientExcutablePath} is not a trust caller.");
                // Signature check failed. Disconnect.
                return;
            }

            logger?.WriteLine($"Trust caller checking: {clientExcutablePath} is a trust application");

            BinaryFormatter bf = new BinaryFormatter();

            Tryblock.Run(() =>
            {
                // Get the client request.
                var request = (AuthorizeRequest)bf.Deserialize(authPipe);

                logger?.WriteLine($"Requesting authentication, url: {request.APIUrl}");

                // Get the URI for the requested plugin.
                string uri = GetEndpoint(request.APIUrl);

                // Create a response for the client.
                var response = new AuthorizeResponse();

                if (uri != null)
                {
                    // Create a random token and associate it with the appId and uri.
                    byte[] token = this.getToken(request.AppID + uri);
                    response.Token = token;
                    response.RealUrl = uri;
                }
                else
                {
                    response.Token = null;
                    response.RealUrl = null;
                }

                // Send response back to the client.
                bf.Serialize(authPipe, response);
            }, ex => bf.Serialize(authPipe, new AuthorizeResponse()));
        }

        /// <summary>
        /// Query the executuable by process id.
        /// </summary>
        /// <param name="processId">given process id.</param>
        /// <returns>The executuable path.</returns>
        private static string getExecuableByProcessId(uint processId)
        {
            string execuablePath = null;
            using (ManagementObjectSearcher procs = new ManagementObjectSearcher($"SELECT * FROM Win32_Process where ProcessId={processId}"))
            {
                foreach (ManagementObject proc in procs.Get())
                {
                    execuablePath = (string)proc["ExecutablePath"];
                    break;
                }
            }

            return execuablePath;
        }

        /// <summary>
        /// Get the real WCF endpoint for given friendly endpoint url.
        /// </summary>
        /// <param name="apiUrl">given friendly endpoint url</param>
        /// <returns>the real WCF endpoint</returns>
        internal string GetEndpoint(string apiUrl)
        {
            return endpoints[apiUrl];
        }

        /// <summary>
        /// Get the friendly endpoint url for real WCF endpoint.
        /// </summary>
        /// <param name="endpoint">given endpoint url</param>
        /// <returns>friendly endpoint url</returns>
        internal string GetFriendlyUrl(string endpoint)
        {
            var kv = endpoints.SingleOrDefault(e => e.Value == endpoint);
            return kv.Key;
        }

        /// <summary>
        /// Get the token for given key (app id + friendly endpoint url).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private byte[] getToken(string key)
        {
            return apiTokens.GetOrAdd(key, createToken());
        }

        /// <summary>
        /// Generate a new token.
        /// </summary>
        /// <returns></returns>
        private byte[] createToken()
        {
            byte[] token = new byte[8];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(token);
            }
            return token;
        }
    }
}
