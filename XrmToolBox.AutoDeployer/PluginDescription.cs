namespace XrmToolBox.AutoDeployer
{
    using System.ComponentModel.Composition;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;

    [Export(typeof(IXrmToolBoxPlugin)),
    ExportMetadata("Name", "Auto Deployer for Plugins and Web Resources"),
    ExportMetadata("Description", "Tool to automatically upload plugin assemblies and web resources as they are changed during build process"),
    ExportMetadata("SmallImageBase64", Constants.B64_IMAGE_SMALL), // null for "no logo" image or base64 image content
    ExportMetadata("BigImageBase64", Constants.B64_IMAGE_LARGE), // null for "no logo" image or base64 image content
    ExportMetadata("BackgroundColor", "#ffffff"), // Use a HTML color name
    ExportMetadata("PrimaryFontColor", "#000000"), // Or an hexadecimal code
    ExportMetadata("SecondaryFontColor", "DarkGray")]
    public class AutoDeployerTool : PluginBase
    {
        #region Public Methods

        public override IXrmToolBoxPluginControl GetControl() => 
            new MainControl();

        #endregion Public Methods
    }
}