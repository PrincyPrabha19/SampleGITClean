using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Tools;
using Microsoft.Win32;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class WebBrowserServiceClass : WebBrowserService
    {
        #region Private Consts
        private const string DEFAULT_BROWSER_PROGID = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
        private const string DEFAULT_BROWSER_PATH = @"{0}\shell\open\command";
        #endregion

        #region Private Properties
        private string defaultBrowserProgId { get; set; }
        private BrowserType defaultBrowserType { get; set; }
        #endregion

        #region WebBrowserService Members
        public string DefaultBrowserPath { get; private set; }
        public bool EnableTabbedBrowsing { get; set; }

        public void Execute(IEnumerable<string> urls, bool enableTabbedBrowsing)
        {
            if (enableTabbedBrowsing)
            {
                foreach (var url in urls)
                {
                    Process.Start(url);
                    Thread.Sleep(1000);
                }

                return;
            }

            foreach (var url in urls)
            {
                Process process;
                ApplicationLaunchHelper.Execute(DefaultBrowserPath, getMultiWindowUrlParameter(url), out process);
                Thread.Sleep(500);
            }
        }
        #endregion

        #region Constructors
        public WebBrowserServiceClass()
        {
            defaultBrowserProgId = getDefaultBrowserProgId();
            if (!String.IsNullOrEmpty(defaultBrowserProgId))
            {
                defaultBrowserType = getBrowserTypeByProgId(defaultBrowserProgId);
                DefaultBrowserPath = getDefaultBrowserPath(String.Format(DEFAULT_BROWSER_PATH, defaultBrowserProgId));
                return;
            }

            DefaultBrowserPath = getDefaultBrowserPath(String.Format(DEFAULT_BROWSER_PATH, "http"));        
        }
        #endregion

        #region Private Methods
        private string getDefaultBrowserPath(string subkey)
        {
            string browserPath = String.Empty;

            RegistryKey key = null;

            try
            {
                key = Registry.ClassesRoot.OpenSubKey(subkey, false);
                if (key != null)
                {
                    browserPath = key.GetValue(null).ToString();
                    browserPath = browserPath.Replace("\"", "");

                    string filename, _arguments;
                    if (FilePathHelper.IsValidPath(browserPath, out filename, out _arguments))                           
                        browserPath = filename;
                }
            }
            finally
            {
                if (key != null)
                    key.Close();
            }

            return browserPath;
        }

        private string getDefaultBrowserProgId()
        {
            string progid = String.Empty;

            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.OpenSubKey(DEFAULT_BROWSER_PROGID, false);
                if (key != null)
                {
                    var _progid = key.GetValue("Progid");
                    if (_progid != null)
                        progid = _progid.ToString();
                }
            }
            finally
            {
                if (key != null)
                    key.Close();
            }

            return progid;
        }

        private BrowserType getBrowserTypeByProgId(string progId)
        {
            foreach (var item in Enum.GetNames(typeof(BrowserType)))
            {
                var browserType = (BrowserType)Enum.Parse(typeof(BrowserType), item);
                string browserProgId =
                    EnumHelper.GetAttributeValue<BrowserProgIdAttributeClass, string>(browserType);
                if (String.Compare(progId, browserProgId, true) == 0)
                    return browserType;
            }

            return BrowserType.Unknown;
        }

        private string getMultiWindowUrlParameter(string url)
        {
            if (defaultBrowserType != BrowserType.Unknown)
            {
                string parameter =
                    EnumHelper.GetAttributeValue<BrowserMultiWindowParameterAttributeClass, string>(defaultBrowserType);
                if (!String.IsNullOrEmpty(parameter))
                    return parameter + " " + url;
            }

            return url;
        }
        #endregion
    }
}
