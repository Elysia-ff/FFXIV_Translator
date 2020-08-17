using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace FFXIV_Translator
{
    public partial class FFXIV_Translator : UserControl, IActPluginV1
    {
        private Label lblStatus;

        public FFXIV_Translator()
        {
            InitializeComponent();
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Controls.Add(this);
            Dock = DockStyle.Fill;
            lblStatus = pluginStatusText;
            lblStatus.Text = "Plugin Started";

            //ActGlobals.oFormActMain.BeforeLogLineRead
        }

        public void DeInitPlugin()
        {
            lblStatus.Text = "No Status";
        }
    }
}
