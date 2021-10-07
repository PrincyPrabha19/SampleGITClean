using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dominator.Tools.Classes.Security;
using System;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
	[PluginInfo(Name = PluginName, Version = PluginVersion, Id = PluginId, Description = PluginDescription)]
	public class IntrepidPlugin : IAgentPlugin
	{
		public const string PluginId = "{AA58CECB-4B82-480C-A1A9-364E29A4EDCE}";
		public const string PluginName = "Dominator Plugin : Intrepid";
		public const string PluginVersion = "1.0";
		public const string PluginDescription = "This plugin is for Dominator in Intrepid";

		private readonly Log log;

		
		public IntrepidPlugin(IAgent agent)
		{
			log = agent.CreateLog("Loading Dominator Plugin in Intrepid");	
		}

		public void OnStartPlugin()
		{
			log.Info("Dominator Service Started");

			try
			{
				EncryptionHelper.StartWCFService();
				BIOSSupportHelper.StartWCFService();
				XTUServiceHelper.StartWCFService();
				XTUMonitorHelper.StartWCFService();
				AuthenticationManager.Singleton.StartAsync();
				NotificationService.Start();
			}
			catch (Exception e)
			{
				log.Info("Service.Start", null, e.ToString());
			}
		}


		public void OnStopPlugin()
		{
			try
			{
				XTUServiceHelper.StopWCFService();
				XTUMonitorHelper.StopWCFService();
				BIOSSupportHelper.StopWCFService();
				EncryptionHelper.StopWCFService();
			}
			catch (Exception e)
			{
				log.Info("Service.Stop", null, e.ToString());
			}

			log.Info("Dominator Service Stopped");
		}

		/// <summary>
		/// Policy identifer that this plugin handles - return empty string since this plugin has no 
		/// policies.
		/// </summary>
		public string PolicyId { get { return string.Empty; } }

		/// <summary>
		/// This method returns a class that reports back information about this plugin.
		/// </summary>
		public AgentPluginInfo GetAgentPluginInfo()
		{
			var info = new AgentPluginInfo
			{
				PluginName = PluginName,
				PluginGuid = new Guid(PluginId),
				PluginVersion = PluginVersion,
				PluginDescription = PluginDescription,
				PluginEnabled = true
			};
			return info;
		}

		
		public void OnSetPolicies(string strPolicies)
		{
		}
		
	}
}
