/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Dominator.Tools.Classes
{
    /// <summary>
    /// This class defines static helper functions that are useful for retrieving signing information about 
    /// .NET assemblies.  This helper class is required because in the current version of .NET (4.5), 
    /// the method X509Certificate2.GetCertHash(assemblyName) returns only a single certificate and does not support SHA256
    /// signed certificates.  However, this class does support SHA256 signatures by making use of the Win32 WinVerifyTrust() 
    /// API to get signature information about an assembly.
    /// </summary>
    /// <remarks>
    /// Note that WinVerifyTrust will not recognize SHA256 certificates on Windows 7.
    /// </remarks>
    public static class SigningHelper
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable UnusedMember.Global
        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Global
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable FieldCanBeMadeReadOnly.Global

        #region Constants

        private const string WINTRUST_ACTION_GENERIC_VERIFY_V2 = "{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}";

        private const int SGNR_TYPE_TIMESTAMP = 0x00000010;
        private const int WTD_UI_NONE = 2;
        private const int WTD_CHOICE_FILE = 1;
        private const int WTD_REVOKE_NONE = 0;
        private const int WTD_REVOKE_WHOLECHAIN = 1;
        private const int WTD_STATEACTION_IGNORE = 0;
        private const int WTD_STATEACTION_VERIFY = 1;
        private const int WTD_STATEACTION_CLOSE = 2;
        private const int WTD_REVOCATION_CHECK_NONE = 16;
        private const int WTD_REVOCATION_CHECK_CHAIN = 64;
        private const int WTD_UICONTEXT_EXECUTE = 0;
        private const int WSS_VERIFY_SPECIFIC = 0x00000001;
        private const int WSS_GET_SECONDARY_SIG_COUNT = 0x00000002;

        #endregion

        #region DllImports

        /// <summary>
        /// The WinVerifyTrust function performs a trust verification action on a specified object. The function passes 
        /// the inquiry to a trust provider that supports the action identifier, if one exists.
        /// </summary>
        /// <param name="hWnd">
        /// Optional handle to a caller window. A trust provider can use this value to determine whether it can interact with the user. 
        /// However, trust providers typically perform verification actions without input from the user.
        /// </param>
        /// <param name="pgActionID">
        /// A pointer to a GUID structure that identifies an action and the trust provider that supports that action. This value indicates 
        /// the type of verification action to be performed on the structure pointed to by pWinTrustData.
        /// </param>
        /// <param name="pWVTData">
        /// A pointer that, when cast as a WINTRUST_DATA structure, contains information that the trust provider needs to process the 
        /// specified action identifier. Typically, the structure includes information that identifies the object that the trust provider must evaluate.
        /// </param>
        /// <returns>
        /// If the trust provider verifies that the subject is trusted for the specified action, the return value is zero. 
        /// No other value besides zero should be considered a successful return.
        /// </returns>
        [DllImport("wintrust.dll")]
        private static extern int WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWVTData);

        /// <summary>
        /// The WTHelperProvDataFromStateData function retrieves trust provider information from a specified handle. 
        /// This function has no associated import library. You must use the LoadLibrary and GetProcAddress functions 
        /// to dynamically link to Wintrust.dll.
        /// </summary>
        /// <param name="hStateData">
        /// A handle previously set by the WinVerifyTrustEx function as the hWVTStateData	member of the WINTRUST_DATA structure.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to a CRYPT_PROVIDER_DATA structure. 
        /// The returned pointer can be used by the WTHelperGetProvSignerFromChain function. If the function fails, it returns NULL.
        /// </returns>
        [DllImport("wintrust.dll")]
        private static extern IntPtr WTHelperProvDataFromStateData(IntPtr hStateData);

        /// <summary>
        /// The WTHelperGetProvSignerFromChain function retrieves a signer or countersigner by index from the chain. 
        /// This function has no associated import library. You must use the LoadLibrary and GetProcAddress functions 
        /// to dynamically link to Wintrust.dll.
        /// </summary>
        /// <param name="pProvData">
        /// A pointer to the CRYPT_PROVIDER_DATA structure that contains the signer and countersigner information.
        /// </param>
        /// <param name="idxSigner">
        /// The index of the signer. The index is zero based.
        /// </param>
        /// <param name="fCounterSigner">
        /// If TRUE, the countersigner, as specified by idxCounterSigner, is retrieved by this function; the signer 
        /// that contains the countersigner is identified by idxSigner. If FALSE, the signer, as specified by idxSigner, 
        /// is retrieved by this function.
        /// </param>
        /// <param name="idxCounterSigner">
        /// The index of the countersigner. The index is zero based. The countersigner applies to the signer identified by idxSigner.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to a CRYPT_PROVIDER_SGNR structure for the requested signer or countersigner. 
        /// If the function fails, it returns NULL.
        /// </returns>
        [DllImport("wintrust.dll")]
        private static extern IntPtr WTHelperGetProvSignerFromChain(IntPtr pProvData, int idxSigner, bool fCounterSigner, int idxCounterSigner);

        /// <summary>
        /// The WTHelperGetProvCertFromChain function retrieves a trust provider certificate from the 
        /// certificate chain. This function has no associated import library. You must use the LoadLibrary and 
        /// GetProcAddress functions to dynamically link to Wintrust.dll.
        /// </summary>
        /// <param name="pSgnr">
        /// A pointer to a CRYPT_PROVIDER_SGNR structure that represents the signers. This pointer is retrieved 
        /// by the WTHelperGetProvSignerFromChain function.
        /// </param>
        /// <param name="idxCert">
        /// The index of the certificate. The index is zero based.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns a pointer to a CRYPT_PROVIDER_CERT structure that represents the trust provider certificate. 
        /// If the function fails, it returns NULL.
        /// </returns>
        [DllImport("wintrust.dll")]
        private static extern IntPtr WTHelperGetProvCertFromChain(IntPtr pSgnr, int idxCert);

        #endregion

        #region Public Definitions

        /// <summary>
        /// This class defines the information that is returned by GetSigningInfo
        /// </summary>
        public struct SigningInfo
        {
            /// <summary>
            /// The certificate used to sign the object
            /// </summary>
            public X509Certificate2 Certificate;

            /// <summary>
            /// The time when the object was stamped
            /// </summary>
            public DateTime TimeStamp;

            /// <summary>
            /// The hashing algorithm used - e.g "sha1" or "sha256"
            /// </summary>
            public string HashAlgorithm;
        }

        #endregion

        #region Windows Structures

        [StructLayout(LayoutKind.Sequential)]
        private struct WINTRUST_DATA
        {
            internal int cbStruct;
            internal IntPtr pPolicyCallbackData;
            internal IntPtr pSIPClientData;
            internal int dwUIChoice;
            internal int fdwRevocationChecks;
            internal int dwUnionChoice;
            internal IntPtr pFile;
            internal int dwStateAction;
            internal IntPtr hWVTStateData;
            internal IntPtr pwszURLReference;
            internal int dwProvFlags;
            internal int dwUIContext;
            internal IntPtr pSignatureSettings;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINTRUST_SIGNATURE_SETTINGS
        {
            internal int cbStruct;
            internal int dwIndex;
            internal int dwFlags;
            internal int cSecondarySigs;
            internal int dwVerifiedSigIndex;
            internal IntPtr pCryptoPolicy;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINTRUST_FILE_INFO
        {
            internal int cbStruct;
            internal IntPtr pcwszFilePath;
            internal IntPtr hFile;
            internal IntPtr pgKnownSubject;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_PROVIDER_DATA
        {
            internal int cbStruct;
            internal IntPtr pWintrustData;
            internal bool fOpenedFile;
            internal IntPtr hWndParent;
            internal IntPtr pgActionID;
            internal IntPtr hProv;
            internal int dwError;
            internal int dwRegSecuritySettings;
            internal int dwRegPolicySettings;
            internal IntPtr psPfns;
            internal int cdwTrustStepErrors;
            internal IntPtr padwTrustStepErrors;
            internal int chStores;
            internal IntPtr pahStores;
            internal int dwEncoding;
            internal IntPtr hMsg;
            internal int csSigners;
            internal IntPtr pasSigners;
            internal int csProvPrivData;
            internal IntPtr pasProvPrivData;
            internal int dwSubjectChoice;
            internal IntPtr pPDSip;
            internal IntPtr pszUsageOID;
            internal bool fRecallWithState;
            internal System.Runtime.InteropServices.ComTypes.FILETIME sftSystemTime;
            internal IntPtr pszCTLSignerUsageOID;
            internal int dwProvFlags;
            internal int dwFinalError;
            internal IntPtr pRequestUsage;
            internal int dwTrustPubSettings;
            internal int dwUIStateFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_PROVIDER_SGNR
        {
            internal int cbStruct;
            internal System.Runtime.InteropServices.ComTypes.FILETIME sftVerifyAsOf;
            internal int csCertChain;
            internal IntPtr pasCertChain;
            internal int dwSignerType;
            internal IntPtr psSigner;
            internal int dwError;
            internal int csCounterSigners;
            internal IntPtr pasCounterSigners;
            internal IntPtr pChainContext;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_PROVIDER_CERT
        {
            internal int cbStruct;
            internal IntPtr pCert;
            internal bool fCommercial;
            internal bool fTrustedRoot;
            internal bool fSelfSigned;
            internal bool fTestCert;
            internal int dwRevokedReason;
            internal int dwConfidence;
            internal int dwError;
            internal IntPtr pTrustListContext;
            internal bool fTrustListSignerCert;
            internal IntPtr pCtlContext;
            internal int dwCtlError;
            internal bool fIsCyclic;
            internal IntPtr pChainElement;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_ALGORITHM_IDENTIFIER
        {
            internal IntPtr pszObjId;
            internal CRYPT_INTEGER_BLOB Parameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CMSG_SIGNER_INFO
        {
            internal int dwVersion;
            internal CRYPT_INTEGER_BLOB Issuer;
            internal CRYPT_INTEGER_BLOB SerialNumber;
            internal CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
            internal CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;
            internal CRYPT_INTEGER_BLOB EncryptedHash;
            internal CRYPT_ATTRIBUTES AuthAttrs;
            internal CRYPT_ATTRIBUTES UnauthAttrs;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_INTEGER_BLOB
        {
            internal int cbData;
            internal IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CRYPT_ATTRIBUTES
        {
            internal int cAttr;
            internal IntPtr rgAttr;
        }

        #endregion

        // ReSharper restore UnusedMember.Local
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore MemberCanBePrivate.Local
        // ReSharper restore ClassNeverInstantiated.Global
        // ReSharper enable InconsistentNaming
        // ReSharper enable UnusedMember.Global
        // ReSharper restore FieldCanBeMadeReadOnly.Global

        /// <summary>
        /// Given a fully qualified filename, returns a list of SigningInfo objects that contain information about
        /// how an assembly is signed.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Returns a list of signature information about the assembly.</returns>
        public static List<SigningInfo> GetSigningInfo(string fileName)
        {
            var list = new List<SigningInfo>();
            var pcwszFilePath = IntPtr.Zero;
            var ptrFile = IntPtr.Zero;
            var pSignatureSettings = IntPtr.Zero;
            var pgActionID = IntPtr.Zero;
            var pWVTData = IntPtr.Zero;

            try
            {
                pcwszFilePath = Marshal.StringToHGlobalAuto(fileName);

                var File = new WINTRUST_FILE_INFO
                {
                    cbStruct = Marshal.SizeOf(typeof(WINTRUST_FILE_INFO)),
                    pcwszFilePath = pcwszFilePath,
                    hFile = IntPtr.Zero,
                    pgKnownSubject = IntPtr.Zero,
                };
                ptrFile = Marshal.AllocHGlobal(File.cbStruct);
                Marshal.StructureToPtr(File, ptrFile, false);

                var WVTData = new WINTRUST_DATA
                {
                    cbStruct = Marshal.SizeOf(typeof(WINTRUST_DATA)),
                    pPolicyCallbackData = IntPtr.Zero,
                    pSIPClientData = IntPtr.Zero,
                    dwUIChoice = WTD_UI_NONE,
                    fdwRevocationChecks = WTD_REVOKE_NONE,
                    dwUnionChoice = WTD_CHOICE_FILE,
                    pFile = ptrFile,
                    dwStateAction = WTD_STATEACTION_IGNORE,
                    hWVTStateData = IntPtr.Zero,
                    pwszURLReference = IntPtr.Zero,
                    dwProvFlags = WTD_REVOCATION_CHECK_NONE,
                    dwUIContext = WTD_UICONTEXT_EXECUTE,
                    pSignatureSettings = IntPtr.Zero
                };

                var signatureSettings = default(WINTRUST_SIGNATURE_SETTINGS);
                if (Environment.OSVersion.Version > new Version(6, 2, 0, 0))        // can only call on Windows 8.0 or greater
                {
                    signatureSettings = new WINTRUST_SIGNATURE_SETTINGS
                    {
                        cbStruct = Marshal.SizeOf(typeof(WINTRUST_SIGNATURE_SETTINGS)),
                        dwIndex = 0,
                        dwFlags = WSS_GET_SECONDARY_SIG_COUNT,
                        cSecondarySigs = 0,
                        dwVerifiedSigIndex = 0,
                        pCryptoPolicy = IntPtr.Zero,
                    };
                    pSignatureSettings = Marshal.AllocHGlobal(signatureSettings.cbStruct);
                }
                if (pSignatureSettings != IntPtr.Zero)
                {
                    Marshal.StructureToPtr(signatureSettings, pSignatureSettings, false);
                    WVTData.pSignatureSettings = pSignatureSettings;
                }

                var actionIdBytes = new Guid(WINTRUST_ACTION_GENERIC_VERIFY_V2).ToByteArray();
                pgActionID = Marshal.AllocHGlobal(actionIdBytes.Length);
                Marshal.Copy(actionIdBytes, 0, pgActionID, actionIdBytes.Length);

                pWVTData = Marshal.AllocHGlobal(WVTData.cbStruct);
                Marshal.StructureToPtr(WVTData, pWVTData, false);

                var hWnd = new IntPtr(-1);
                var hResult = WinVerifyTrust(hWnd, pgActionID, pWVTData);
                if (hResult != 0)
                    throw new Exception(string.Format("WinVerifyTrust returned 0x{0:x8}", hResult));

                if (pSignatureSettings != IntPtr.Zero)
                    signatureSettings = (WINTRUST_SIGNATURE_SETTINGS)Marshal.PtrToStructure(pSignatureSettings, typeof(WINTRUST_SIGNATURE_SETTINGS));
                var nSignatures = signatureSettings.cSecondarySigs + 1;

                for (var idx = 0; idx < nSignatures; idx++)
                {
                    if (pSignatureSettings != IntPtr.Zero)
                    {
                        signatureSettings.dwIndex = idx;
                        signatureSettings.dwFlags = WSS_VERIFY_SPECIFIC;
                        Marshal.StructureToPtr(signatureSettings, pSignatureSettings, false);
                    }
                    WVTData.dwStateAction = WTD_STATEACTION_VERIFY;
                    WVTData.hWVTStateData = IntPtr.Zero;
                    Marshal.StructureToPtr(WVTData, pWVTData, false);
                    hResult = WinVerifyTrust(hWnd, pgActionID, pWVTData);
                    try
                    {
                        if (hResult != 0)
                            throw new Exception(string.Format("WinVerifyTrust returned 0x{0:x8} for signature index {1}", hResult, idx));
                        WVTData = (WINTRUST_DATA)Marshal.PtrToStructure(pWVTData, typeof(WINTRUST_DATA));
                        var ptrProvData = WTHelperProvDataFromStateData(WVTData.hWVTStateData);
                        var provData = (CRYPT_PROVIDER_DATA)Marshal.PtrToStructure(ptrProvData, typeof(CRYPT_PROVIDER_DATA));

                        for (var idxSigner = 0; idxSigner < provData.csSigners; idxSigner++)
                        {
                            var info = new SigningInfo();
                            var ptrProvSigner = WTHelperGetProvSignerFromChain(ptrProvData, idxSigner, false, 0);
                            var ProvSigner = (CRYPT_PROVIDER_SGNR)Marshal.PtrToStructure(ptrProvSigner, typeof(CRYPT_PROVIDER_SGNR));
                            var Signer = (CMSG_SIGNER_INFO)Marshal.PtrToStructure(ProvSigner.psSigner, typeof(CMSG_SIGNER_INFO));

                            //
                            // Get the signer's hash algorithm
                            //
#if __MAC__
                            info.HashAlgorithm = "sha1";
#else
                            if (Signer.HashAlgorithm.pszObjId != IntPtr.Zero)
                            {
                                var objId = Marshal.PtrToStringAnsi(Signer.HashAlgorithm.pszObjId);
                                if (objId != null)
                                    info.HashAlgorithm = Oid.FromOidValue(objId, OidGroup.All).FriendlyName;
                            }
#endif

                            //
                            // Get the timestamp when signed
                            //
                            if (ProvSigner.sftVerifyAsOf.dwHighDateTime != provData.sftSystemTime.dwHighDateTime && ProvSigner.sftVerifyAsOf.dwLowDateTime != provData.sftSystemTime.dwLowDateTime)
                                info.TimeStamp = DateTime.FromFileTime(((long)ProvSigner.sftVerifyAsOf.dwHighDateTime << 32) | (uint)ProvSigner.sftVerifyAsOf.dwLowDateTime);

                            //
                            // Get the certificate used to sign
                            //
                            var ptrCert = WTHelperGetProvCertFromChain(ptrProvSigner, 0);
                            var cert = (CRYPT_PROVIDER_CERT)Marshal.PtrToStructure(ptrCert, typeof(CRYPT_PROVIDER_CERT));
                            if (cert.cbStruct > 0)
                            {
                                info.Certificate = new X509Certificate2(cert.pCert);
                                list.Add(info);
                            }
                        }
                    }
                    finally
                    {
                        WVTData.dwStateAction = WTD_STATEACTION_CLOSE;
                        Marshal.StructureToPtr(WVTData, pWVTData, false);
                        WinVerifyTrust(hWnd, pgActionID, pWVTData);
                    }
                }
            }
            finally
            {
                if (pWVTData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pWVTData);
                if (pgActionID != IntPtr.Zero)
                    Marshal.FreeHGlobal(pgActionID);
                if (pSignatureSettings != IntPtr.Zero)
                    Marshal.FreeHGlobal(pSignatureSettings);
                if (ptrFile != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptrFile);
                if (pcwszFilePath != IntPtr.Zero)
                    Marshal.FreeHGlobal(pcwszFilePath);
            }
            return list;
        }

    }
}
