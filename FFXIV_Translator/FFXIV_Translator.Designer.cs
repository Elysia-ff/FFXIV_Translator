namespace FFXIV_Translator
{
    partial class FFXIV_Translator
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.visibleCheckBox = new System.Windows.Forms.CheckBox();
            this.dragCheckbox = new System.Windows.Forms.CheckBox();
            this.resizeCheckbox = new System.Windows.Forms.CheckBox();
            this.opacitySlider = new System.Windows.Forms.TrackBar();
            this.opacityTitleLabel = new System.Windows.Forms.Label();
            this.opacityValueLabel = new System.Windows.Forms.Label();
            this.ffChatGroup = new System.Windows.Forms.GroupBox();
            this.partyChatCheckbox = new System.Windows.Forms.CheckBox();
            this.eventChatCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).BeginInit();
            this.ffChatGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // visibleCheckBox
            // 
            this.visibleCheckBox.AutoSize = true;
            this.visibleCheckBox.Checked = true;
            this.visibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.visibleCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.visibleCheckBox.Location = new System.Drawing.Point(15, 18);
            this.visibleCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visibleCheckBox.Name = "visibleCheckBox";
            this.visibleCheckBox.Size = new System.Drawing.Size(61, 19);
            this.visibleCheckBox.TabIndex = 0;
            this.visibleCheckBox.Text = "Visible";
            this.visibleCheckBox.UseVisualStyleBackColor = true;
            this.visibleCheckBox.CheckedChanged += new System.EventHandler(this.VisibleCheckBox_CheckedChanged);
            // 
            // dragCheckbox
            // 
            this.dragCheckbox.AutoSize = true;
            this.dragCheckbox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dragCheckbox.Location = new System.Drawing.Point(39, 64);
            this.dragCheckbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dragCheckbox.Name = "dragCheckbox";
            this.dragCheckbox.Size = new System.Drawing.Size(81, 19);
            this.dragCheckbox.TabIndex = 1;
            this.dragCheckbox.Text = "Draggable";
            this.dragCheckbox.UseVisualStyleBackColor = true;
            this.dragCheckbox.CheckedChanged += new System.EventHandler(this.DragCheckbox_CheckedChanged);
            // 
            // resizeCheckbox
            // 
            this.resizeCheckbox.AutoSize = true;
            this.resizeCheckbox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.resizeCheckbox.Location = new System.Drawing.Point(39, 106);
            this.resizeCheckbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resizeCheckbox.Name = "resizeCheckbox";
            this.resizeCheckbox.Size = new System.Drawing.Size(75, 19);
            this.resizeCheckbox.TabIndex = 3;
            this.resizeCheckbox.Text = "Resizable";
            this.resizeCheckbox.UseVisualStyleBackColor = true;
            this.resizeCheckbox.CheckedChanged += new System.EventHandler(this.ResizeCheckbox_CheckedChanged);
            // 
            // opacitySlider
            // 
            this.opacitySlider.LargeChange = 10;
            this.opacitySlider.Location = new System.Drawing.Point(91, 154);
            this.opacitySlider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.opacitySlider.Maximum = 100;
            this.opacitySlider.Name = "opacitySlider";
            this.opacitySlider.Size = new System.Drawing.Size(239, 45);
            this.opacitySlider.TabIndex = 5;
            this.opacitySlider.TickFrequency = 10;
            this.opacitySlider.Scroll += new System.EventHandler(this.OpacitySlider_Scroll);
            // 
            // opacityTitleLabel
            // 
            this.opacityTitleLabel.AutoSize = true;
            this.opacityTitleLabel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.opacityTitleLabel.Location = new System.Drawing.Point(37, 154);
            this.opacityTitleLabel.Name = "opacityTitleLabel";
            this.opacityTitleLabel.Size = new System.Drawing.Size(48, 15);
            this.opacityTitleLabel.TabIndex = 4;
            this.opacityTitleLabel.Text = "Opacity";
            // 
            // opacityValueLabel
            // 
            this.opacityValueLabel.AutoSize = true;
            this.opacityValueLabel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.opacityValueLabel.Location = new System.Drawing.Point(336, 154);
            this.opacityValueLabel.Name = "opacityValueLabel";
            this.opacityValueLabel.Size = new System.Drawing.Size(38, 15);
            this.opacityValueLabel.TabIndex = 6;
            this.opacityValueLabel.Text = "100%";
            // 
            // ffChatGroup
            // 
            this.ffChatGroup.Controls.Add(this.partyChatCheckbox);
            this.ffChatGroup.Controls.Add(this.eventChatCheckbox);
            this.ffChatGroup.Location = new System.Drawing.Point(40, 218);
            this.ffChatGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ffChatGroup.Name = "ffChatGroup";
            this.ffChatGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ffChatGroup.Size = new System.Drawing.Size(88, 89);
            this.ffChatGroup.TabIndex = 7;
            this.ffChatGroup.TabStop = false;
            this.ffChatGroup.Text = "FFXIV Chat";
            // 
            // partyChatCheckbox
            // 
            this.partyChatCheckbox.AutoSize = true;
            this.partyChatCheckbox.Location = new System.Drawing.Point(17, 52);
            this.partyChatCheckbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.partyChatCheckbox.Name = "partyChatCheckbox";
            this.partyChatCheckbox.Size = new System.Drawing.Size(53, 19);
            this.partyChatCheckbox.TabIndex = 9;
            this.partyChatCheckbox.Text = "Party";
            this.partyChatCheckbox.UseVisualStyleBackColor = true;
            this.partyChatCheckbox.CheckedChanged += new System.EventHandler(this.PartyChatCheckbox_CheckedChanged);
            // 
            // eventChatCheckbox
            // 
            this.eventChatCheckbox.AutoSize = true;
            this.eventChatCheckbox.Location = new System.Drawing.Point(17, 25);
            this.eventChatCheckbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eventChatCheckbox.Name = "eventChatCheckbox";
            this.eventChatCheckbox.Size = new System.Drawing.Size(55, 19);
            this.eventChatCheckbox.TabIndex = 8;
            this.eventChatCheckbox.Text = "Event";
            this.eventChatCheckbox.UseVisualStyleBackColor = true;
            this.eventChatCheckbox.CheckedChanged += new System.EventHandler(this.EventChatCheckbox_CheckedChanged);
            // 
            // FFXIV_Translator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ffChatGroup);
            this.Controls.Add(this.opacityValueLabel);
            this.Controls.Add(this.opacityTitleLabel);
            this.Controls.Add(this.opacitySlider);
            this.Controls.Add(this.resizeCheckbox);
            this.Controls.Add(this.dragCheckbox);
            this.Controls.Add(this.visibleCheckBox);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FFXIV_Translator";
            this.Size = new System.Drawing.Size(800, 562);
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).EndInit();
            this.ffChatGroup.ResumeLayout(false);
            this.ffChatGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox visibleCheckBox;
        private System.Windows.Forms.CheckBox dragCheckbox;
        private System.Windows.Forms.CheckBox resizeCheckbox;
        private System.Windows.Forms.TrackBar opacitySlider;
        private System.Windows.Forms.Label opacityTitleLabel;
        private System.Windows.Forms.Label opacityValueLabel;
        private System.Windows.Forms.GroupBox ffChatGroup;
        private System.Windows.Forms.CheckBox eventChatCheckbox;
        private System.Windows.Forms.CheckBox partyChatCheckbox;
    }
}
