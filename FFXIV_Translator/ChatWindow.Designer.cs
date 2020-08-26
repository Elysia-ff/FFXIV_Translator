using System.Drawing;
using System.Drawing.Drawing2D;

namespace FFXIV_Translator
{
    partial class ChatWindow
    {
        private const int penWidth = 1;
        private readonly Pen moveRectPen = new Pen(Color.FromName("Highlight"), penWidth * 2)
        {
            LineJoin = LineJoin.Bevel,
        };

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                moveRectPen.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.TextBox();
            this.langBtn = new System.Windows.Forms.Button();
            this.executeBtn = new System.Windows.Forms.Button();
            this.textPanel = new System.Windows.Forms.Panel();
            this.chatPanel = new System.Windows.Forms.Panel();
            this.chatParent = new System.Windows.Forms.Panel();
            this.textPanel.SuspendLayout();
            this.chatParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.AcceptsReturn = true;
            this.textBox.AcceptsTab = true;
            this.textBox.BackColor = System.Drawing.Color.Black;
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.textBox.ForeColor = System.Drawing.SystemColors.Highlight;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(387, 23);
            this.textBox.TabIndex = 0;
            this.textBox.Text = "Insert here";
            // 
            // langBtn
            // 
            this.langBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.langBtn.BackColor = System.Drawing.Color.Black;
            this.langBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.langBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.langBtn.Location = new System.Drawing.Point(380, 222);
            this.langBtn.Name = "langBtn";
            this.langBtn.Size = new System.Drawing.Size(34, 23);
            this.langBtn.TabIndex = 1;
            this.langBtn.Text = "EN";
            this.langBtn.UseVisualStyleBackColor = false;
            this.langBtn.Click += new System.EventHandler(this.LangBtn_Click);
            // 
            // executeBtn
            // 
            this.executeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.executeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.executeBtn.Image = global::FFXIV_Translator.Properties.Resources.enter_key_cutout;
            this.executeBtn.Location = new System.Drawing.Point(420, 222);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(35, 23);
            this.executeBtn.TabIndex = 2;
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // textPanel
            // 
            this.textPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textPanel.Controls.Add(this.textBox);
            this.textPanel.Location = new System.Drawing.Point(5, 222);
            this.textPanel.Name = "textPanel";
            this.textPanel.Size = new System.Drawing.Size(370, 23);
            this.textPanel.TabIndex = 3;
            // 
            // chatPanel
            // 
            this.chatPanel.AutoScrollMargin = new System.Drawing.Size(0, 2);
            this.chatPanel.Location = new System.Drawing.Point(0, 0);
            this.chatPanel.Name = "chatPanel";
            this.chatPanel.Size = new System.Drawing.Size(450, 210);
            this.chatPanel.TabIndex = 4;
            this.chatPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chatPanel_MouseDown);
            this.chatPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ChatPanel_MouseWheel);
            // 
            // chatParent
            // 
            this.chatParent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatParent.Controls.Add(this.chatPanel);
            this.chatParent.Location = new System.Drawing.Point(5, 5);
            this.chatParent.Name = "chatParent";
            this.chatParent.Size = new System.Drawing.Size(450, 210);
            this.chatParent.TabIndex = 5;
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(460, 250);
            this.Controls.Add(this.chatParent);
            this.Controls.Add(this.textPanel);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.langBtn);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(165, 65);
            this.Name = "ChatWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ChatWindow";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.ChatWindow_Activated);
            this.Deactivate += new System.EventHandler(this.ChatWindow_Deactivate);
            this.LocationChanged += new System.EventHandler(this.ChatWindow_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.ChatWindow_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChatWindow_MouseDown);
            this.textPanel.ResumeLayout(false);
            this.textPanel.PerformLayout();
            this.chatParent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button langBtn;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.Panel textPanel;
        private System.Windows.Forms.Panel chatPanel;
        private System.Windows.Forms.Panel chatParent;
    }
}