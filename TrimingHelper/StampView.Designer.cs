namespace TrimmingHelper
{
    partial class StampView
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_lstStamp = new System.Windows.Forms.ListView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.m_stampInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_snapList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lstStamp
            // 
            this.m_lstStamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lstStamp.LargeImageList = this.m_snapList;
            this.m_lstStamp.Location = new System.Drawing.Point(0, 0);
            this.m_lstStamp.MultiSelect = false;
            this.m_lstStamp.Name = "m_lstStamp";
            this.m_lstStamp.Size = new System.Drawing.Size(150, 127);
            this.m_lstStamp.TabIndex = 0;
            this.m_lstStamp.UseCompatibleStateImageBehavior = false;
            this.m_lstStamp.DoubleClick += new System.EventHandler(this.m_lstStamp_DoubleClick);
            this.m_lstStamp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_lstStamp_KeyDown);
            this.m_lstStamp.Leave += new System.EventHandler(this.m_lstStamp_Leave);
            this.m_lstStamp.MouseLeave += new System.EventHandler(this.m_lstStamp_MouseLeave);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_stampInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 127);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(150, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // m_stampInfo
            // 
            this.m_stampInfo.Name = "m_stampInfo";
            this.m_stampInfo.Size = new System.Drawing.Size(44, 18);
            this.m_stampInfo.Text = "準備中";
            // 
            // m_snapList
            // 
            this.m_snapList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.m_snapList.ImageSize = new System.Drawing.Size(16, 16);
            this.m_snapList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // StampView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lstStamp);
            this.Controls.Add(this.statusStrip1);
            this.Name = "StampView";
            this.Load += new System.EventHandler(this.StampView_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView m_lstStamp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel m_stampInfo;
        private System.Windows.Forms.ImageList m_snapList;
    }
}
