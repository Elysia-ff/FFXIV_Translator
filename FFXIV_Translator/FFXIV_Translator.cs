using System;
using System.Globalization;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using FFXIV_Translator.Properties;

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
            eventChatCheckbox.Checked = Settings.Default.FFEvent;
            partyChatCheckbox.Checked = Settings.Default.FFParty;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Controls.Add(this);
            Dock = DockStyle.Fill;
            lblStatus = pluginStatusText;
            lblStatus.Text = "Plugin Started";

            ActGlobals.oFormActMain.BeforeLogLineRead += OFormActMain_BeforeLogLineRead;

            chatWindow.Show();
            chatWindow.Visible = Settings.Default.Visible;
            int opacity = Settings.Default.Opacity;
            chatWindow.Opacity = opacity * 0.01d;
            opacityValueLabel.Text = opacity + "%";
        }

        public void DeInitPlugin()
        {
            lblStatus.Text = "No Status";
            ActGlobals.oFormActMain.BeforeLogLineRead -= OFormActMain_BeforeLogLineRead;
            chatWindow.Hide();
        }

        private void OFormActMain_BeforeLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            string[] logLine = logInfo.originalLogLine.Split('|');
            if (logLine.Length < 5 ||!int.TryParse(logLine[0], out int logCode))
                return;

            // 0    1      2      3         4
            // 00 | date | 003d | speaker | string  event
            // 00 | date | 000e | speaker | string  party
            if (logCode == 0)
            {
                if (int.TryParse(logLine[2], NumberStyles.HexNumber, null, out int chatCode))
                {
                    if (chatCode == (int)FFChatCode.Event && Settings.Default.FFEvent)
                    {
                        AddChatLog(logLine, FFChatCode.Event);
                    }
                    else if (chatCode == (int)FFChatCode.Party && Settings.Default.FFParty)
                    {
                        AddChatLog(logLine, FFChatCode.Party);
                    }
                }
            }
        }

        private void AddChatLog(string[] logLIne, FFChatCode code)
        {
            Invoke(() =>
            {
                chatWindow.Execute(logLIne[3], logLIne[4], code);
            });
        }

        private static void Invoke(Action action)
        {
            if (ActGlobals.oFormActMain != null &&
                ActGlobals.oFormActMain.IsHandleCreated &&
                !ActGlobals.oFormActMain.IsDisposed)
            {
                if (ActGlobals.oFormActMain.InvokeRequired)
                {
                    ActGlobals.oFormActMain.Invoke((MethodInvoker)delegate
                    {
                        action();
                    });
                }
                else
                {
                    action();
                }
            }
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
            opacityValueLabel.Text = opacity + "%";
            Settings.Default.Opacity = opacity;
            Settings.Default.Save();

            chatWindow.UpdateOpacity();
        }

        private void EventChatCheckbox_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.FFEvent = eventChatCheckbox.Checked;
            Settings.Default.Save();
        }

        private void PartyChatCheckbox_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.FFParty = partyChatCheckbox.Checked;
            Settings.Default.Save();
        }
    }
}
