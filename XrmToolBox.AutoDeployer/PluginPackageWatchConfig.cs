using System;
using System.Collections.Generic;

public class PluginPackageWatchItem
{
    public bool IsActive { get; set; } = true;
    public string NupkgPath { get; set; }
    public Guid PackageId { get; set; }
    public string PackageName { get; set; }

    // Persisted UI state
    public DateTime? LastFileWriteUtc { get; set; }
    public DateTime? LastUploadUtc { get; set; }
    public string LastStatus { get; set; }
    public string LastError { get; set; }
}

public class PluginPackageWatchConfig
{
    public List<PluginPackageWatchItem> Items { get; set; } = new List<PluginPackageWatchItem>();
}
