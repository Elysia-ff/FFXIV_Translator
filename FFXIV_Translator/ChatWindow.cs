using FFXIV_Translator.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FFXIV_Translator.PapagoAPIs;

namespace FFXIV_Translator
{
    public partial class ChatWindow : Form
    {
        private bool isActivated = false;
        private List<Label> chats = new List<Label>();
        private int scrollPosition = 0;
        private const int scrollMargin = 2;
        private const int scrollDelta = 10;
        private const int maxScrollPosition = 0;

        private PapagoAPI.LangCode langCode = PapagoAPI.LangCode.KR;

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
            UpdateLangBtn();
        }

        private void AddChat(string msg)
        {
            using (new SuspendLayoutScope(chatPanel, true))
            {
                Label newLabel = new Label();
                using (new SuspendLayoutScope(newLabel, false))
                {
                    Size size = GetTextSize(msg);
                    int y = chatPanel.Size.Height - scrollMargin - size.Height;

                    newLabel.BorderStyle = BorderStyle.FixedSingle;
                    newLabel.Location = new Point(0, y);
                    newLabel.Font = Font;
                    newLabel.Size = size;
                    newLabel.TextAlign = ContentAlignment.MiddleLeft;
                    newLabel.Text = msg;
                    newLabel.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
                    {
                        Label label = sender as Label;
                        string text = label.Text.Replace("\n", "\r\n");
                        Clipboard.SetText(text);
                    };

                    chatPanel.Controls.Add(newLabel);
                    chats.Insert(0, newLabel);
                    scrollPosition = maxScrollPosition;

                    Reposition();
                }
            }
        }

        private Size GetTextSize(string msg)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF sizef = g.MeasureString(msg, Font, chatPanel.Size.Width - scrollMargin);
                Size size = new Size(chatPanel.Size.Width - scrollMargin, (int)Math.Ceiling(sizef.Height));

                return size;
            }
        }

        private int GetTotalHeight()
        {
            int h = 0;
            for (int i = 0; i < chats.Count; ++i)
            {
                h += chats[i].Size.Height;
            }

            return h;
        }

        private void Reposition()
        {
            using (new SuspendLayoutScope(chatPanel, true))
            {
                int prevY = 0;
                for (int i = 0; i < chats.Count; ++i)
                {
                    using (new SuspendLayoutScope(chats[i], false))
                    {
                        int y = i > 0 ?
                            prevY - chats[i].Size.Height :
                            chatPanel.Size.Height - scrollPosition - scrollMargin - chats[i].Size.Height;
                        prevY = y;

                        if (y + chats[i].Size.Height >= 0 && y <= chatPanel.Size.Height)
                        {
                            chats[i].Location = new Point(chats[i].Location.X, y);
                            chats[i].Visible = true;
                        }
                        else
                        {
                            chats[i].Visible = false;
                        }
                    }
                }
            }
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
            using (new SuspendLayoutScope(this, true))
            {
                using (new SuspendLayoutScope(chatParent, false))
                {
                    Size size = new Size(Width - 10, Height - 40);
                    chatParent.Size = size;

                    using (new SuspendLayoutScope(chatPanel, false))
                    {
                        chatPanel.Size = size;

                        for (int i = 0; i < chats.Count; ++i)
                        {
                            using (new SuspendLayoutScope(chats[i], false))
                            {
                                chats[i].Size = new Size(chatPanel.Size.Width - scrollMargin, chats[i].Size.Height);
                            }
                        }
                    }
                }

                using (new SuspendLayoutScope(textPanel, false))
                {
                    Size size = new Size(Size.Width - 90, textBox.Size.Height);
                    textPanel.Size = size;

                    using (new SuspendLayoutScope(textBox, false))
                    {
                        size.Width += 17;
                        textBox.Size = size;
                    }
                }
            }

            Reposition();
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

        private void ChatPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0 || chats.Count <= 0)
                return;

            if (e.Delta > 0)
            {
                scrollPosition -= scrollDelta;
            }
            else if (e.Delta < 0)
            {
                scrollPosition += scrollDelta;
            }

            int height = GetTotalHeight();
            int minScrollPosition = -height + chatPanel.Size.Height - scrollMargin;
            if (scrollPosition < minScrollPosition)
                scrollPosition = minScrollPosition;
            if (scrollPosition > maxScrollPosition)
                scrollPosition = maxScrollPosition;

            Reposition();
        }

        private void chatPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // test code 
            //return;
            Random random = new Random();
            for (int i = 0; i < 100; ++i)
            {
                int r = random.Next(3);
                if (r == 0)
                {
                    AddChat("test string" + i);
                }
                else if (r == 1)
                {
                    AddChat("test stringdddddddddddddddddddddddddddddddddddddddddddddd" + i);
                }
                else if (r == 2)
                {
                    AddChat("test string\nasdfasdf\nassf" + i);
                }
            }
        }

        private void LangBtn_Click(object sender, EventArgs e)
        {
            langCode += 1;
            if (langCode >= PapagoAPI.LangCode.Count)
                langCode = 0;

            UpdateLangBtn();
        }

        private void UpdateLangBtn()
        {
            langBtn.Text = langCode.ToString();
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e)
        {
            string msg = await PapagoAPI.Translate(textBox.Text, langCode);

            AddChat(msg);
            textBox.Text = string.Empty;
        }
    }
}
