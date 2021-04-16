using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FFXIV_Translator
{
    public class LabelPool
    {
        private readonly List<Label> pool = new List<Label>();
        private int idx = 0;

        private readonly Control parent;
        private readonly Size defaultSize;
        private const int poolInitialCount = 10;

        public LabelPool(Control _parent)
        {
            parent = _parent;
            defaultSize = new Size(parent.Width, 20);

            using (new SuspendLayoutScope(parent, true))
            {
                for (int i = 0; i < poolInitialCount; ++i)
                {
                    CreateNewLabel(false);
                }
            }
        }

        private Label CreateNewLabel(bool visible)
        {
            Label newLabel = new Label();
            using (new SuspendLayoutScope(newLabel, false))
            {
                newLabel.BorderStyle = BorderStyle.FixedSingle;
                newLabel.Font = parent.Font;
                newLabel.Size = defaultSize;
                newLabel.TextAlign = ContentAlignment.MiddleLeft;
                newLabel.Text = string.Empty;
                newLabel.MouseDoubleClick += delegate (object sender, MouseEventArgs e)
                {
                    Label label = sender as Label;
                    string text = label.Text.Replace("\n", "\r\n");
                    Clipboard.SetText(text);
                };
                newLabel.Visible = visible;

                parent.Controls.Add(newLabel);
                pool.Add(newLabel);
            }

            return newLabel;
        }

        public Label GetLabel()
        {
            int i = idx++;
            if (i < pool.Count)
            {
                using (new SuspendLayoutScope(pool[i], false))
                {
                    pool[i].Visible = true;
                }

                return pool[i];
            }

            return CreateNewLabel(true);
        }

        public void StartReposition()
        {
            idx = 0;
        }

        public void EndReposition()
        {
            for (int i = idx; i < pool.Count; ++i)
            {
                if (string.IsNullOrEmpty(pool[i].Text) && pool[i].Visible)
                {
                    using (new SuspendLayoutScope(pool[i], false))
                    {
                        pool[i].Visible = false;
                    }
                }
            }
        }
    }
}
