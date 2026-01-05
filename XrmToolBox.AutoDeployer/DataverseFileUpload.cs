using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xrm.Sdk;

namespace XrmToolBox.AutoDeployer
{
    internal static class DataverseFileUpload
    {
        internal sealed class UploadProgress
        {
            public string Phase { get; set; }           // "Initializing", "Uploading", "Committing"
            public int BlockIndex { get; set; }         // 1-based
            public int BlockCount { get; set; }
            public long BytesSent { get; set; }
            public long TotalBytes { get; set; }
            public string FileName { get; set; }
        }

        public static void UploadPluginPackageNupkg(
            IOrganizationService svc,
            Guid pluginPackageId,
            string filePath,
            Action<UploadProgress> progress = null)
        {
            if (svc == null) throw new ArgumentNullException(nameof(svc));
            if (pluginPackageId == Guid.Empty) throw new ArgumentException("pluginPackageId is empty", nameof(pluginPackageId));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("Package file not found", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);
            var totalBytes = fileInfo.Length;

            progress?.Invoke(new UploadProgress
            {
                Phase = "Initializing",
                BlockIndex = 0,
                BlockCount = 0,
                BytesSent = 0,
                TotalBytes = totalBytes,
                FileName = fileName
            });

            // 1) Initialize
            var initReq = new OrganizationRequest("InitializeFileBlocksUpload");
            initReq["Target"] = new EntityReference("pluginpackage", pluginPackageId);
            initReq["FileAttributeName"] = "package";
            initReq["FileName"] = fileName;

            var initResp = svc.Execute(initReq);
            if (!initResp.Results.Contains("FileContinuationToken"))
                throw new InvalidOperationException("InitializeFileBlocksUpload did not return FileContinuationToken");

            var token = (string)initResp.Results["FileContinuationToken"];

            // 2) Upload blocks
            const int blockSize = 4 * 1024 * 1024; // 4MB (safe default)
            var blockCount = (int)((totalBytes + blockSize - 1) / blockSize);

            var blockIds = new List<string>(blockCount);

            long bytesSent = 0;
            int blockIndex = 0;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[blockSize];

                while (true)
                {
                    int read = fs.Read(buffer, 0, buffer.Length);
                    if (read <= 0) break;

                    blockIndex++;

                    // trim block
                    var blockData = new byte[read];
                    Buffer.BlockCopy(buffer, 0, blockData, 0, read);

                    var blockId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    blockIds.Add(blockId);

                    var uploadReq = new OrganizationRequest("UploadBlock");
                    uploadReq["FileContinuationToken"] = token;
                    uploadReq["BlockId"] = blockId;
                    uploadReq["BlockData"] = blockData;

                    svc.Execute(uploadReq);

                    bytesSent += read;

                    progress?.Invoke(new UploadProgress
                    {
                        Phase = "Uploading",
                        BlockIndex = blockIndex,
                        BlockCount = blockCount,
                        BytesSent = bytesSent,
                        TotalBytes = totalBytes,
                        FileName = fileName
                    });
                }
            }

            // 3) Commit
            progress?.Invoke(new UploadProgress
            {
                Phase = "Committing",
                BlockIndex = blockIndex,
                BlockCount = blockCount,
                BytesSent = bytesSent,
                TotalBytes = totalBytes,
                FileName = fileName
            });

            var commitReq = new OrganizationRequest("CommitFileBlocksUpload");
            commitReq["FileContinuationToken"] = token;
            commitReq["BlockList"] = blockIds.ToArray();

            svc.Execute(commitReq);
        }
    }
}
