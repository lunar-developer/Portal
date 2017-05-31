using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Website.Library.Business
{
    public static class BundleBusiness
    {
        /*
         * Properties
         */
        public static readonly List<BundleData> Bundles = new List<BundleData>();


        /*
         * Constructor
         */
        static BundleBusiness()
        {
            // Base Style
            BundleData styleBundle = new BundleData(FolderEnum.BaseStyleBundle, false);
            styleBundle.Include(FolderEnum.BaseStyleFolder, new List<string>
            {
                "font-awesome.min.css",
                "font-roboto.min.css",
                "bootstrap.min.css",
                "jquery-control.css"
            });
            Bundles.Add(styleBundle);

            // Base Script
            BundleData scriptBundle = new BundleData(FolderEnum.BaseScriptBundle);
            scriptBundle.Include(FolderEnum.BaseScriptFolder, new List<string>
            {
                "jquery.hoverIntent.min.js",
                "dnn.jquery.js",
                "bootstrap.min.js",
                "jquery-ui.js",
                "jquery-control.js",
                "jquery.cookie.js"
            });
            Bundles.Add(scriptBundle);
        }


        /*
         * Functions
         */
        public static void Initialize()
        {
            RegisterBundles(BundleTable.Bundles);
        }

        public static void Include(List<BundleData> bundles)
        {
            MergeBundle(bundles, Bundles);
        }

        private static void MergeBundle(IEnumerable<BundleData> source, ICollection<BundleData> target)
        {
            foreach (BundleData item in source)
            {
                BundleData bundle = target.FirstOrDefault(iterator => iterator.BundleFolder == item.BundleFolder
                    && iterator.IsScriptBundle == item.IsScriptBundle);
                if (bundle == null)
                {
                    target.Add(item);
                }
                else
                {
                    bundle.SourceFolders.AddRange(item.SourceFolders);
                }
            }
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            foreach (BundleData bundleData in Bundles)
            {
                bundles.Add(CreateBundle(bundleData));
            }
        }

        private static Bundle CreateBundle(BundleData bundleData)
        {
            Bundle bundle;
            if (bundleData.IsScriptBundle)
            {
                bundle = new ScriptBundle(bundleData.BundleFolder)
                {
                    Orderer = new IncludeOrderer()
                };
            }
            else
            {
                bundle = new StyleBundle(bundleData.BundleFolder)
                {
                    Orderer = new IncludeOrderer()
                };
            }


            foreach (FolderData folderData in bundleData.SourceFolders)
            {
                foreach (string file in folderData.Files)
                {
                    bundle.Include($"{folderData.Folder}{file}");
                }
            }
            return bundle;
        }
    }

    public class IncludeOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}