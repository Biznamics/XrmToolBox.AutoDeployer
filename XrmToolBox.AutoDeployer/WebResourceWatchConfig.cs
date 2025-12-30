using System.Collections.Generic;

namespace XrmToolBox.AutoDeployer
{
    public class WebResourceMapping
    {
        public bool IsActive { get; set; }
        public string RelativePath { get; set; }
        public string CrmName { get; set; }
    }

    public class WebResourceWatchConfig
    {
        public string RootPath { get; set; }
        public string Prefix { get; set; }
        public string Patterns { get; set; } // Multiline string
        public bool PublishEnabled { get; set; }
        public int DebounceMs { get; set; }
        public List<WebResourceMapping> Mappings { get; set; } = new List<WebResourceMapping>();
    }
}
