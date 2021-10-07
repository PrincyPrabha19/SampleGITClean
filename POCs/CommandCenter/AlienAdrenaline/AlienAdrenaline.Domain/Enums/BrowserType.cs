
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Enums
{   
    public enum BrowserType
    {
        [BrowserMultiTabParameterAttributeClass("")]
        [BrowserMultiWindowParameterAttributeClass("")]
        [BrowserProgIdAttributeClass("")]
        Unknown,

        [BrowserMultiTabParameterAttributeClass("")]
        [BrowserMultiWindowParameterAttributeClass("")]
        [BrowserProgIdAttributeClass("IE.HTTP")]
        IExplore,

        [BrowserMultiTabParameterAttributeClass("-new-tab")]
        [BrowserMultiWindowParameterAttributeClass("-new-window")]
        [BrowserProgIdAttributeClass("FirefoxURL")]
        Firefox,

        [BrowserMultiTabParameterAttributeClass("")]
        [BrowserMultiWindowParameterAttributeClass("--new-window")]
        [BrowserProgIdAttributeClass("ChromeHTML")]
        Chrome,

        [BrowserMultiTabParameterAttributeClass("")]
        [BrowserMultiWindowParameterAttributeClass("")]
        [BrowserProgIdAttributeClass("SafariURL")]
        Safari,

        [BrowserMultiTabParameterAttributeClass("-newpage")]
        [BrowserMultiWindowParameterAttributeClass("-newwindow")]
        [BrowserProgIdAttributeClass("Opera.Protocol")]
        Opera,
    }
}
