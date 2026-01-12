using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace XrmToolBox.AutoDeployer
{
    internal sealed class SelectItemDialog : Form
    {
        #region Private Fields

        private readonly Button _cancel = new Button();
        private readonly Label _hint = new Label();
        private readonly ListBox _list = new ListBox();
        private readonly Button _ok = new Button();

        #endregion Private Fields

        #region Public Constructors

        /// <param name="title">Window title</param>
        /// <param name="items">Items to select from</param>
        /// <param name="hintText">Short instruction shown above the list</param>
        /// <param name="okText">OK button text (optional)</param>
        public SelectItemDialog(string title, IList<string> items, string hintText = null, string okText = "OK")
        {
            Text = title ?? "Select item";
            StartPosition = FormStartPosition.CenterParent;
            Width = 560;
            Height = 460;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;

            // Make Enter/Esc work
            AcceptButton = _ok;
            CancelButton = _cancel;
            KeyPreview = true;

            // Hint/instructions
            _hint.Dock = DockStyle.Top;
            _hint.AutoSize = false;
            _hint.Height = 44;
            _hint.Padding = new Padding(12, 10, 12, 8);
            _hint.Text = hintText ?? "Select an item from the list and click OK (or double-click an item).";
            _hint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // List
            _list.Dock = DockStyle.Fill;
            _list.IntegralHeight = false;
            _list.SelectionMode = SelectionMode.One;

            if (items != null && items.Count > 0)
            {
                _list.Items.AddRange(items.Cast<object>().ToArray());
                _list.SelectedIndex = 0; // default selection to reduce “what do I do?”
            }

            _list.SelectedIndexChanged += (s, e) => _ok.Enabled = _list.SelectedIndex >= 0;
            _list.DoubleClick += (s, e) => TryAccept();

            // Also accept on Enter while list has focus
            _list.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    TryAccept();
                }
            };

            // Buttons panel
            var panel = new Panel { Dock = DockStyle.Bottom, Height = 52, Padding = new Padding(12, 10, 12, 10) };

            _ok.Text = okText ?? "OK";
            _ok.Width = 90;
            _ok.Height = 28;
            _ok.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _ok.Left = panel.Width - 200; // will be re-laid out in Resize below
            _ok.Top = 10;
            _ok.Enabled = _list.SelectedIndex >= 0;
            _ok.Click += (s, e) => TryAccept();

            _cancel.Text = "Cancel";
            _cancel.Width = 90;
            _cancel.Height = 28;
            _cancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _cancel.Left = panel.Width - 100; // will be re-laid out in Resize below
            _cancel.Top = 10;
            _cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            panel.Controls.Add(_ok);
            panel.Controls.Add(_cancel);

            // Keep buttons aligned on resize
            panel.Resize += (s, e) =>
            {
                _cancel.Left = panel.ClientSize.Width - _cancel.Width;
                _ok.Left = _cancel.Left - 10 - _ok.Width;
            };

            Controls.Add(_list);
            Controls.Add(panel);
            Controls.Add(_hint);

            _list.DrawMode = DrawMode.OwnerDrawFixed;
            _list.IntegralHeight = false;
            _list.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            _list.ItemHeight = 22;
            _list.DrawItem += List_DrawItem;
        }

        #endregion Public Constructors

        #region Public Properties

        public int SelectedIndex => _list.SelectedIndex;

        #endregion Public Properties

        #region Private Methods

        private void List_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            var list = (ListBox)sender;
            var text = list.Items[e.Index]?.ToString() ?? string.Empty;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Alternating row background (only when not selected)
            Color backColor = selected
                ? SystemColors.Highlight
                : (e.Index % 2 == 0 ? Color.White : Color.FromArgb(245, 245, 245));

            Color foreColor = selected ? SystemColors.HighlightText : SystemColors.ControlText;

            using (var backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, e.Bounds);

            // Text padding
            var textRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 2, e.Bounds.Width - 12, e.Bounds.Height - 4);
            TextRenderer.DrawText(e.Graphics, text, e.Font, textRect, foreColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            e.DrawFocusRectangle();
        }

        private void TryAccept()
        {
            if (_list.SelectedIndex < 0) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion Private Methods
    }
}