using System.Windows.Forms;

namespace XrmToolBox.AutoDeployer
{
    public class WebResourcesManagerDialog : Form
    {
        public WebResourcesManagerDialog()
        {
            this.Text = "Web Resources Manager";
            this.Width = 400;
            this.Height = 300;
            this.StartPosition = FormStartPosition.CenterParent;
            var label = new Label { Text = "Web Resources Manager Placeholder", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
            this.Controls.Add(label);
        }
    }
}
