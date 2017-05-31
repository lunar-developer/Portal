using System.Collections.Generic;

namespace Website.Library.DataTransfer
{
    public class BundleData
    {
        public readonly string BundleFolder;
        public readonly List<FolderData> SourceFolders = new List<FolderData>();
        public readonly bool IsScriptBundle;

        public BundleData(string bundleFolder, bool isScriptBundle = true)
        {
            BundleFolder = bundleFolder;
            IsScriptBundle = isScriptBundle;
        }

        public void Include(string folder, List<string> files)
        {
            SourceFolders.Add(new FolderData
            {
                Folder = folder,
                Files = files
            });
        }
    }
}