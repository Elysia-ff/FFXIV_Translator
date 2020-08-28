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
        private LabelPool labelPool;
        private List<Chat> chats = new List<Chat>();
        private int scrollPosition = 0;
        private int prevScrollPosition = 0;
        public const int scrollMargin = 2;
        public const int scrollDelta = 10;
        public const int minScrollPosition = 0;
        public const int scrollBarWidth = 10;

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
            labelPool = new LabelPool(chatPanel);
            int savedCode = Settings.Default.LangCode;
            if (savedCode < 0 || savedCode >= (int)PapagoAPI.LangCode.Count)
                savedCode = 0;
            langCode = (PapagoAPI.LangCode)savedCode;
            UpdateLangBtn();
        }

        private void AddChat(string msg, bool reposition = true)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            Chat newChat = new Chat(msg, GetTextSize(msg));
            chats.Insert(0, newChat);

            scrollPosition = minScrollPosition;
            prevScrollPosition = scrollPosition;
            if (reposition)
                Reposition();
            UpdateScrollBarSize();
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
                labelPool.StartReposition();
                int prevY = 0;

                for (int i = 0; i < chats.Count; ++i)
                {
                    int y = i > 0 ?
                        prevY - chats[i].Height :
                        chatPanel.Height + scrollPosition - scrollMargin - chats[i].Height;
                    prevY = y;

                    int upperThreshold = y + chats[i].Height;
                    if (upperThreshold >= 0 && y <= chatPanel.Height)
                    {
                        Label label = labelPool.GetLabel();
                        using (new SuspendLayoutScope(label, false))
                        {
                            label.Location = new Point(0, y);
                            label.Size = chats[i].Size;
                            label.Text = chats[i].Str;
                        }
                    }
                    else if (upperThreshold < 0)
                        break;
                }
                labelPool.EndReposition();
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
                            Chat c = chats[i];
                            c.Size = GetTextSize(chats[i].Str);
                            chats[i] = c;
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
            prevScrollPosition = scrollPosition;

            Reposition();
            UpdateScrollBarPosition();
        }

#if DEBUG
        private void ChatPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // test code 
            return;
            Random random = new Random();
            int count = 5000;
            for (int i = 1; i <= count; ++i)
            {
                int r = random.Next(3);
                if (r == 0)
                {
                    AddChat("test string" + i, i == count);
                }
                else if (r == 1)
                {
                    AddChat("test stringdddddddddddddddddddddddddddddddddddddddddddddd" + i, i == count);
                }
                else if (r == 2)
                {
                    AddChat("test string\nasdfasdf\nassf" + i, i == count);
                }
            }
        }
#endif

        private void LangBtn_Click(object sender, EventArgs e)
        {
            langCode += 1;
            if (langCode >= PapagoAPI.LangCode.Count)
                langCode = 0;

            UpdateLangBtn();
            Settings.Default.LangCode = (int)langCode;
            Settings.Default.Save();
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
                Point point = scrollBar.Location;
                point.Y = scrollBarPanel.PointToClient(Cursor.Position).Y - (int)(scrollBar.Height * 0.5f);
                if (point.Y < 0)
                    point.Y = 0;
                else if (point.Y + scrollBar.Height > scrollBarPanel.Height)
                    point.Y = scrollBarPanel.Height - scrollBar.Height;

                float t = 1 - (float)point.Y / (scrollBarPanel.Height - scrollBar.Height);
                if (float.IsNaN(t)) t = 1;
                int newScrollPosition = (int)Math.Ceiling(MathExtension.Lerp(minScrollPosition, MaxScrollPosition, t));

                if (MathExtension.Distance(newScrollPosition, prevScrollPosition) >= scrollDelta)
                {
                    using (new SuspendLayoutScope(scrollBar, false))
                    {
                        scrollBar.Location = point;
                    }
                    scrollPosition = newScrollPosition;
                    prevScrollPosition = scrollPosition;

                    Reposition();
                }
            }
        }

        private void UpdateScrollBarPosition()
        {
            using (new SuspendLayoutScope(scrollBar, false))
            {
                float t = 1 - (float)scrollPosition / MaxScrollPosition;
                if (float.IsNaN(t)) t = 1;
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
                    if (float.IsNaN(t)) t = 1;
                    v = (int)Math.Ceiling(MathExtension.Lerp(0, panelHeight, t));
                }
                else
                {
                    v = scrollBarPanel.Height;
                }

                Size size = scrollBar.Size;
                size.Height = Math.Max(v, 5);
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
