using FFXIV_Translator.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FFXIV_Translator
{
    public partial class ChatWindow : Form
    {
        private bool isActivated = false;

        // Hide from Alt+Tab
        // https://social.msdn.microsoft.com/Forums/windows/en-US/0eefb6f4-3619-4f7a-b144-48df80e2c603/how-to-hide-form-from-alttab-dialog?forum=winforms
        protected override CreateParams CreateParams
        {
            get
            {
                // Turn on WS_EX_TOOLWINDOW style bit
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        // Resize
        // https://stackoverflow.com/a/2575452
        private const int cGrip = 12;      // Grip size

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Settings.Default.Resizable)
            {
                Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
                ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            }

            if (isActivated)
            {
                Rectangle rect = new Rectangle(penWidth, penWidth, Width - penWidth * 2, Height - penWidth * 2);
                e.Graphics.DrawRectangle(moveRectPen, rect);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = PointToClient(pos);
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip && Settings.Default.Resizable)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        public ChatWindow()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void ChatWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Settings.Default.Draggable)
            {
                this.MoveWithMouse();
            }
        }

        private void ChatWindow_LocationChanged(object sender, EventArgs e)
        {
            Settings.Default.Location = Location;
            Settings.Default.Save();
        }

        private void ChatWindow_SizeChanged(object sender, EventArgs e)
        {
            Size size = new Size(Size.Width - 90, textBox.Size.Height);
            textPanel.Size = size;
            size.Width += 17;
            textBox.Size = size;

            Settings.Default.Size = Size;
            Settings.Default.Save();
        }

        private void ChatWindow_Activated(object sender, EventArgs e)
        {
            isActivated = true;
            Refresh();
        }

        private void ChatWindow_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
            Refresh();
        }
    }
}
