﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace Dominator.Tools.AutomaticUpdateService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://alienware.com/webservices", ConfigurationName="AutomaticUpdateService.CommandCenterServiceSoap")]
    public interface CommandCenterServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://alienware.com/webservices/IsThereAnyUpdate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        Dominator.Tools.AutomaticUpdateService.LatestVersionData IsThereAnyUpdate(Dominator.Tools.AutomaticUpdateService.VersionData currentVersionInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://alienware.com/webservices/IsThereAnyUpdatebyPlatform", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        Dominator.Tools.AutomaticUpdateService.LatestVersionData IsThereAnyUpdatebyPlatform(Dominator.Tools.AutomaticUpdateService.VersionPlatformData currentVersionInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://alienware.com/webservices/IsAWCCUsageTrackingEnabled", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        bool IsAWCCUsageTrackingEnabled();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://alienware.com/webservices/GetThirdPartyPartners", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        string GetThirdPartyPartners();
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionPlatformData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://alienware.com/webservices")]
    public partial class VersionData : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string applicationNameField;
        
        private string marketingNameField;
        
        private string versionNumberField;
        
        private string systemModelField;
        
        private int priorityLevelField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string ApplicationName {
            get {
                return this.applicationNameField;
            }
            set {
                this.applicationNameField = value;
                this.RaisePropertyChanged("ApplicationName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string MarketingName {
            get {
                return this.marketingNameField;
            }
            set {
                this.marketingNameField = value;
                this.RaisePropertyChanged("MarketingName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string VersionNumber {
            get {
                return this.versionNumberField;
            }
            set {
                this.versionNumberField = value;
                this.RaisePropertyChanged("VersionNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string SystemModel {
            get {
                return this.systemModelField;
            }
            set {
                this.systemModelField = value;
                this.RaisePropertyChanged("SystemModel");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int PriorityLevel {
            get {
                return this.priorityLevelField;
            }
            set {
                this.priorityLevelField = value;
                this.RaisePropertyChanged("PriorityLevel");
            }
        }
        [field: NonSerialized]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://alienware.com/webservices")]
    public partial class LatestVersionData : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string httpVersionLocationField;
        
        private object[] releaseNotesField;
        
        private string applicationNameField;
        
        private string marketingNameField;
        
        private string versionNumberField;
        
        private string systemModelField;
        
        private string oSNameField;
        
        private int priorityLevelField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string HttpVersionLocation {
            get {
                return this.httpVersionLocationField;
            }
            set {
                this.httpVersionLocationField = value;
                this.RaisePropertyChanged("HttpVersionLocation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        public object[] ReleaseNotes {
            get {
                return this.releaseNotesField;
            }
            set {
                this.releaseNotesField = value;
                this.RaisePropertyChanged("ReleaseNotes");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ApplicationName {
            get {
                return this.applicationNameField;
            }
            set {
                this.applicationNameField = value;
                this.RaisePropertyChanged("ApplicationName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string MarketingName {
            get {
                return this.marketingNameField;
            }
            set {
                this.marketingNameField = value;
                this.RaisePropertyChanged("MarketingName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string VersionNumber {
            get {
                return this.versionNumberField;
            }
            set {
                this.versionNumberField = value;
                this.RaisePropertyChanged("VersionNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string SystemModel {
            get {
                return this.systemModelField;
            }
            set {
                this.systemModelField = value;
                this.RaisePropertyChanged("SystemModel");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string OSName {
            get {
                return this.oSNameField;
            }
            set {
                this.oSNameField = value;
                this.RaisePropertyChanged("OSName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public int PriorityLevel {
            get {
                return this.priorityLevelField;
            }
            set {
                this.priorityLevelField = value;
                this.RaisePropertyChanged("PriorityLevel");
            }
        }
        [field: NonSerialized]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://alienware.com/webservices")]
    public partial class VersionPlatformData : VersionData {
        
        private string platformField;
        
        private string cultureField;
        
        private string oSNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Platform {
            get {
                return this.platformField;
            }
            set {
                this.platformField = value;
                this.RaisePropertyChanged("Platform");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Culture {
            get {
                return this.cultureField;
            }
            set {
                this.cultureField = value;
                this.RaisePropertyChanged("Culture");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string OSName {
            get {
                return this.oSNameField;
            }
            set {
                this.oSNameField = value;
                this.RaisePropertyChanged("OSName");
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CommandCenterServiceSoapChannel : Dominator.Tools.AutomaticUpdateService.CommandCenterServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommandCenterServiceSoapClient : System.ServiceModel.ClientBase<Dominator.Tools.AutomaticUpdateService.CommandCenterServiceSoap>, Dominator.Tools.AutomaticUpdateService.CommandCenterServiceSoap {
        
        public CommandCenterServiceSoapClient() {
        }
        
        public CommandCenterServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CommandCenterServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommandCenterServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommandCenterServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Dominator.Tools.AutomaticUpdateService.LatestVersionData IsThereAnyUpdate(Dominator.Tools.AutomaticUpdateService.VersionData currentVersionInfo) {
            return base.Channel.IsThereAnyUpdate(currentVersionInfo);
        }
        
        public Dominator.Tools.AutomaticUpdateService.LatestVersionData IsThereAnyUpdatebyPlatform(Dominator.Tools.AutomaticUpdateService.VersionPlatformData currentVersionInfo) {
            return base.Channel.IsThereAnyUpdatebyPlatform(currentVersionInfo);
        }
        
        public bool IsAWCCUsageTrackingEnabled() {
            return base.Channel.IsAWCCUsageTrackingEnabled();
        }
        
        public string GetThirdPartyPartners() {
            return base.Channel.GetThirdPartyPartners();
        }
    }
}