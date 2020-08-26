namespace TrimmingHelper
{
    partial class About
    {
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.label1 = new System.Windows.Forms.Label();
            this.m_lbVersion = new System.Windows.Forms.Label();
            this.m_txtVersion = new System.Windows.Forms.TextBox();
            this.m_btnClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "TrimmingHelper";
            // 
            // m_lbVersion
            // 
            this.m_lbVersion.AutoSize = true;
            this.m_lbVersion.Location = new System.Drawing.Point(50, 29);
            this.m_lbVersion.Name = "m_lbVersion";
            this.m_lbVersion.Size = new System.Drawing.Size(63, 12);
            this.m_lbVersion.TabIndex = 1;
            this.m_lbVersion.Text = "xxx.xxx.xxx";
            // 
            // m_txtVersion
            // 
            this.m_txtVersion.Location = new System.Drawing.Point(12, 52);
            this.m_txtVersion.Multiline = true;
            this.m_txtVersion.Name = "m_txtVersion";
            this.m_txtVersion.ReadOnly = true;
            this.m_txtVersion.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_txtVersion.Size = new System.Drawing.Size(478, 165);
            this.m_txtVersion.TabIndex = 2;
            this.m_txtVersion.WordWrap = false;
            // 
            // m_btnClose
            // 
            this.m_btnClose.Location = new System.Drawing.Point(415, 227);
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(75, 23);
            this.m_btnClose.TabIndex = 3;
            this.m_btnClose.Text = "&Close";
            this.m_btnClose.UseVisualStyleBackColor = true;
            this.m_btnClose.Click += new System.EventHandler(this.m_btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 262);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.m_btnClose);
            this.Controls.Add(this.m_txtVersion);
            this.Controls.Add(this.m_lbVersion);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label m_lbVersion;
        private System.Windows.Forms.TextBox m_txtVersion;
        private System.Windows.Forms.Button m_btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}