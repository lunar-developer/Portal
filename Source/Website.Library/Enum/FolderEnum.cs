namespace Website.Library.Enum
{
    public static class FolderEnum
    {
        public const string BaseAssetFolder = "~/Portals/_default/Skins/Assets/";
        public static readonly string BaseImageFolder = BaseAssetFolder + "images/";
        public static readonly string BaseScriptFolder = BaseAssetFolder + "js/";
        public static readonly string BaseScriptSkinFolder = BaseAssetFolder + "js_skin/";
        public static readonly string BaseStyleFolder = BaseAssetFolder + "css/";
        public static readonly string BaseStyleSkinFolder = BaseAssetFolder + "css_skin/";

        public static readonly string BaseScriptBundle = BaseAssetFolder + "js/script.bundle.js";
        public static readonly string BaseStyleBundle = BaseAssetFolder + "css/style.bunble.css";
    }
}