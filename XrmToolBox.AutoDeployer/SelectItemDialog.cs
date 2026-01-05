using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XrmToolBox.AutoDeployer
{
    internal sealed class SelectItemDialog : Form
    {
        private readonly ListBox _list = new ListBox();
        private readonly Button _ok = new Button();
        private readonly Button _cancel = new Button();

        public int SelectedIndex => _list.SelectedIndex;

        public SelectItemDialog(string title, IList<string> items)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            Width = 520;
            Height = 420;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            _list.Dock = DockStyle.Fill;
            _list.IntegralHeight = false;
            _list.Items.AddRange(items.Cast<object>().ToArray());
            _list.DoubleClick += (s, e) => { if (_list.SelectedIndex >= 0) { DialogResult = DialogResult.OK; Close(); } };

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 48 };

            _ok.Text = "OK";
            _ok.Width = 90;
            _ok.Height = 28;
            _ok.Left = Width - 220;
            _ok.Top = 10;
            _ok.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _ok.Enabled = false;
            _ok.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            _cancel.Text = "Cancel";
            _cancel.Width = 90;
            _cancel.Height = 28;
            _cancel.Left = Width - 120;
            _cancel.Top = 10;
            _cancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _list.SelectedIndexChanged += (s, e) => _ok.Enabled = _list.SelectedIndex >= 0;

            panel.Controls.Add(_ok);
            panel.Controls.Add(_cancel);

            Controls.Add(_list);
            Controls.Add(panel);
        }
    }
}
