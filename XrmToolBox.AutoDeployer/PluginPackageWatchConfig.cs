using System;
using System.Collections.Generic;

public class PluginPackageWatchConfig
{
    #region Public Properties

    public List<PluginPackageWatchItem> Items { get; set; } = new List<PluginPackageWatchItem>();

    #endregion Public Properties
}

public class PluginPackageWatchItem
{
    #region Public Properties

    public bool IsActive { get; set; } = true;
    public string LastError { get; set; }

    // Persisted UI state
    public DateTime? LastFileWriteUtc { get; set; }

    public string LastStatus { get; set; }
    public DateTime? LastUploadUtc { get; set; }
    public string NupkgPath { get; set; }
    public Guid PackageId { get; set; }
    public string PackageName { get; set; }

    #endregion Public Properties
}