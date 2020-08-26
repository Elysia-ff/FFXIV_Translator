using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.Diagnostics;
using FFXIV_Translator.Properties;
using FFXIV_Translator.PapagoAPIs;

namespace FFXIV_Translator
{
    public partial class FFXIV_Translator : UserControl, IActPluginV1
    {
        private Label lblStatus;
        private ChatWindow chatWindow;

        public FFXIV_Translator()
        {
            InitializeComponent();

            chatWindow = new ChatWindow();
            chatWindow.Location = Settings.Default.Location;
            chatWindow.Size = Settings.Default.Size;
            visibleCheckBox.Checked = Settings.Default.Visible;
            dragCheckbox.Checked = Settings.Default.Draggable;
            resizeCheckbox.Checked = Settings.Default.Resizable;
            opacitySlider.Value = Settings.Default.Opacity;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Controls.Add(this);
            Dock = DockStyle.Fill;
            lblStatus = pluginStatusText;
            lblStatus.Text = "Plugin Started";

            chatWindow.Show();
            chatWindow.Visible = Settings.Default.Visible;
            int opacity = Settings.Default.Opacity;
            chatWindow.Opacity = opacity * 0.01d;
            opacityValueLabel.Text = opacity + "%";
        }

        public void DeInitPlugin()
        {
            lblStatus.Text = "No Status";
            chatWindow.Hide();
        }

        private void VisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            bool visible = visibleCheckBox.Checked;
            chatWindow.Visible = visible;
            Settings.Default.Visible = visible;
            Settings.Default.Save();
        }

        private void DragCheckbox_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.Draggable = dragCheckbox.Checked;
            Settings.Default.Save();
        }

        private void ResizeCheckbox_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.Resizable = resizeCheckbox.Checked;
            Settings.Default.Save();

            chatWindow.Refresh();
        }

        private void OpacitySlider_Scroll(object sender, System.EventArgs e)
        {
            int opacity = opacitySlider.Value;
            chatWindow.Opacity = opacity * 0.01d;
            opacityValueLabel.Text = opacity + "%";
            Settings.Default.Opacity = opacity;
            Settings.Default.Save();
        }
    }
}
