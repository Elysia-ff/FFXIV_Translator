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
        private const int minScrollPosition = 0;
        private const int scrollBarWidth = 10;

        private int MaxScrollPosition { get { return Math.Max(GetTotalHeight() - chatPanel.Height + scrollMargin, minScrollPosition); } }

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
                    int y = chatPanel.Height - scrollMargin - size.Height;

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
                    scrollPosition = minScrollPosition;

                    Reposition();
                    UpdateScrollBarSize();
                }
            }
        }

        private Size GetTextSize(string msg)
        {
            using (Graphics g = CreateGraphics())
            {
                int width = chatPanel.Width - scrollMargin - scrollBarWidth;
                SizeF sizef = g.MeasureString(msg, Font, width);
                Size size = new Size(width, (int)Math.Ceiling(sizef.Height));

                return size;
            }
        }

        private int GetTotalHeight()
        {
            int h = 0;
            for (int i = 0; i < chats.Count; ++i)
            {
                h += chats[i].Height;
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
                            prevY - chats[i].Height :
                            chatPanel.Height + scrollPosition - scrollMargin - chats[i].Height;
                        prevY = y;

                        if (y + chats[i].Height >= 0 && y <= chatPanel.Height)
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
                                chats[i].Size = new Size(chatPanel.Width - scrollMargin - scrollBarWidth, chats[i].Height);
                            }
                        }

                        using (new SuspendLayoutScope(scrollBarPanel, false))
                        {
                            scrollBarPanel.Size = new Size(scrollBarPanel.Width, chatPanel.Height);

                            UpdateScrollBarPosition();
                        }
                    }
                }

                using (new SuspendLayoutScope(textPanel, false))
                {
                    Size size = new Size(Size.Width - 90, textBox.Height);
                    textPanel.Size = size;

                    using (new SuspendLayoutScope(textBox, false))
                    {
                        size.Width += 17;
                        textBox.Size = size;
                    }
                }
            }

            Reposition();
            UpdateScrollBarSize();
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
                scrollPosition += scrollDelta;
            }
            else if (e.Delta < 0)
            {
                scrollPosition -= scrollDelta;
            }

            int maxScrollPosition = MaxScrollPosition;
            if (scrollPosition < minScrollPosition)
                scrollPosition = minScrollPosition;
            if (scrollPosition > maxScrollPosition)
                scrollPosition = maxScrollPosition;

            Reposition();
            UpdateScrollBarPosition();
        }

        private void chatPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // test code 
            //return;
            Random random = new Random();
            for (int i = 0; i < 10; ++i)
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

        private void ScrollBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                using (new SuspendLayoutScope(scrollBar, false))
                {
                    Point point = scrollBar.Location;
                    point.Y = scrollBarPanel.PointToClient(Cursor.Position).Y - (int)(scrollBar.Height * 0.5f);
                    if (point.Y < 0)
                        point.Y = 0;
                    else if (point.Y + scrollBar.Height > scrollBarPanel.Height)
                        point.Y = scrollBarPanel.Height - scrollBar.Height;

                    scrollBar.Location = point;
                }

                float t = 1 - (float)scrollBar.Location.Y / (scrollBarPanel.Height - scrollBar.Height);
                if (float.IsNaN(t))
                    t = 1;
                scrollPosition = (int)Math.Ceiling(MathExtension.Lerp(minScrollPosition, MaxScrollPosition, t));
                Reposition();
            }
        }

        private void UpdateScrollBarPosition()
        {
            using (new SuspendLayoutScope(scrollBar, false))
            {
                float t = 1 - (float)scrollPosition / MaxScrollPosition;
                if (float.IsNaN(t))
                    t = 1;
                int v = (int)Math.Ceiling(MathExtension.Lerp(0, scrollBarPanel.Height - scrollBar.Height, t));

                Point point = scrollBar.Location;
                point.Y = v;
                scrollBar.Location = point;
            }
        }

        private void UpdateScrollBarSize()
        {
            using (new SuspendLayoutScope(scrollBar, false))
            {
                int panelHeight = chatPanel.Height;
                int chatHeight = GetTotalHeight() + scrollMargin * 2;
                int v;
                if (panelHeight <= chatHeight)
                {
                    float t = (float)panelHeight / chatHeight;
                    if (float.IsNaN(t))
                        t = 1;
                    v = (int)Math.Ceiling(MathExtension.Lerp(0, panelHeight, t));
                }
                else
                {
                    v = scrollBarPanel.Height;
                }

                Size size = scrollBar.Size;
                size.Height = v;
                scrollBar.Size = size;
            }

            UpdateScrollBarPosition();
        }

        private void ScrollBarPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ScrollBar_MouseMove(sender, e);
        }

        private void ScrollBarPanel_MouseMove(object sender, MouseEventArgs e)
        {
            ScrollBar_MouseMove(sender, e);
        }
    }
}
