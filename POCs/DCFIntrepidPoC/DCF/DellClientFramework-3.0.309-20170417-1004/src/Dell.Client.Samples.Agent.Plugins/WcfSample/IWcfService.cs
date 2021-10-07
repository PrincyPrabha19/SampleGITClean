/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.ServiceModel;

namespace Dell.Client.Samples.Agent.Plugins.WcfSample
{
    /// <summary>
    /// This defines the messages that can be sent to a client.
    /// </summary>
    public enum ClientNotifyMessage
    {
        UpdateProviderInfo = 0
    }

    /// <summary>
    /// This defines the service contract that the WCF host supplies.
    /// </summary>
    [ServiceContract(Namespace = "Dell.Client.Samples.Agent.Plugins.WcfSample",
       CallbackContract = typeof(ISampleWcfServiceCallback),
       SessionMode = SessionMode.Required)]
    public interface ISampleWcfService
    {
        [OperationContract(IsOneWay = false)]
        void RegisterClient();

        [OperationContract(IsOneWay = false)]
        void UnregisterClient();        

        [OperationContract(IsOneWay = false)]
        object SendMessage(int msg, object o1);

    }

    /// <summary>
    /// This defines the interface that a WCF client must supply to get session notifications from the WCF host.
    /// </summary>
    public interface ISampleWcfServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnServiceNotify(ClientNotifyMessage msg, string s);
    }
}
