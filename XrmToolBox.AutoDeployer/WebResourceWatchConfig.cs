using System;
using System.Collections.Generic;

namespace XrmToolBox.AutoDeployer
{
    public class WebResourceMapping
    {
        #region Public Properties

        public string CrmName { get; set; }
        public bool IsActive { get; set; }
        public string RelativePath { get; set; }

        #endregion Public Properties
    }

    public class WebResourceWatchConfig
    {
        #region Public Properties

        public int DebounceMs { get; set; }
        public List<WebResourceMapping> Mappings { get; set; } = new List<WebResourceMapping>();
        public string Patterns { get; set; }
        public string Prefix { get; set; }

        // Multiline string
        public bool PublishEnabled { get; set; }

        public string RootPath { get; set; }

        #endregion Public Properties
    }
}