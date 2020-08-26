using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIV_Translator
{
    public class SuspendLayoutScope : IDisposable
    {
        private Control control;
        private bool performLayout;

        public SuspendLayoutScope(Control _control, bool _performLayout)
        {
            control = _control;
            performLayout = _performLayout;
            control.SuspendLayout();
        }

        public void Dispose()
        {
            control.ResumeLayout(performLayout);
        }
    }
}
