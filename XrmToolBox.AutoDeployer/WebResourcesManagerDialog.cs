using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XrmToolBox.AutoDeployer
{
    public partial class WebResourcesManagerDialog : Form
    {
        public WebResourcesManagerDialog()
        {
            InitializeComponent();
            btnClose.Click += (s, e) => Close();
        }
    }
}
