using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.Diagnostics;

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

            Test();
        }

        public void DeInitPlugin()
        {
            lblStatus.Text = "No Status";
        }

        private async void Test()
        {
            string result = await PapagoAPI.PapagoAPI.Translate("안녕하세요", "ja");
            Debug.WriteLine(result);

            string result2 = await PapagoAPI.PapagoAPI.Detect("hi");
            Debug.WriteLine(result2);
        }
    }
}
