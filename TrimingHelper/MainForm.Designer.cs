namespace TrimmingHelper
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.m_btnPaste = new System.Windows.Forms.Button();
            this.m_btnTrim = new System.Windows.Forms.Button();
            this.m_txtOffset = new System.Windows.Forms.TextBox();
            this.m_btnRetry = new System.Windows.Forms.Button();
            this.m_cmbMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_chkClearSelect = new System.Windows.Forms.CheckBox();
            this.m_grpFrom = new System.Windows.Forms.GroupBox();
            this.m_cbAutoSend = new System.Windows.Forms.CheckBox();
            this.m_btnPasteProcess = new System.Windows.Forms.Button();
            this.m_grpMode = new System.Windows.Forms.GroupBox();
            this.m_btnStamp = new System.Windows.Forms.Button();
            this.m_btnSave = new System.Windows.Forms.Button();
            this.m_dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.m_pnlFunc = new System.Windows.Forms.Panel();
            this.m_cbUseClasssic = new System.Windows.Forms.CheckBox();
            this.m_btnAbout = new System.Windows.Forms.Button();
            this.m_cbLeftTopTransparent = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_btnStopTimer = new System.Windows.Forms.Button();
            this.m_btnStartTimer = new System.Windows.Forms.Button();
            this.m_cmbTimer = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.m_stRGB = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_pnlTriming = new TrimmingHelper.PanelEx();
            this.m_stColor = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_grpFrom.SuspendLayout();
            this.m_grpMode.SuspendLayout();
            this.m_pnlFunc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_btnPaste
            // 
            resources.ApplyResources(this.m_btnPaste, "m_btnPaste");
            this.m_btnPaste.Name = "m_btnPaste";
            this.m_btnPaste.UseVisualStyleBackColor = true;
            this.m_btnPaste.Click += new System.EventHandler(this.m_btnPaste_Click);
            // 
            // m_btnTrim
            // 
            resources.ApplyResources(this.m_btnTrim, "m_btnTrim");
            this.m_btnTrim.Name = "m_btnTrim";
            this.m_btnTrim.UseVisualStyleBackColor = true;
            this.m_btnTrim.Click += new System.EventHandler(this.m_btnTrim_Click);
            // 
            // m_txtOffset
            // 
            resources.ApplyResources(this.m_txtOffset, "m_txtOffset");
            this.m_txtOffset.Name = "m_txtOffset";
            // 
            // m_btnRetry
            // 
            resources.ApplyResources(this.m_btnRetry, "m_btnRetry");
            this.m_btnRetry.Name = "m_btnRetry";
            this.m_btnRetry.UseVisualStyleBackColor = true;
            this.m_btnRetry.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_cmbMode
            // 
            this.m_cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbMode.FormattingEnabled = true;
            this.m_cmbMode.Items.AddRange(new object[] {
            resources.GetString("m_cmbMode.Items"),
            resources.GetString("m_cmbMode.Items1")});
            resources.ApplyResources(this.m_cmbMode, "m_cmbMode");
            this.m_cmbMode.Name = "m_cmbMode";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_chkClearSelect
            // 
            resources.ApplyResources(this.m_chkClearSelect, "m_chkClearSelect");
            this.m_chkClearSelect.Name = "m_chkClearSelect";
            this.m_chkClearSelect.UseVisualStyleBackColor = true;
            // 
            // m_grpFrom
            // 
            this.m_grpFrom.Controls.Add(this.m_cbAutoSend);
            this.m_grpFrom.Controls.Add(this.m_btnPasteProcess);
            this.m_grpFrom.Controls.Add(this.m_btnPaste);
            this.m_grpFrom.Controls.Add(this.m_chkClearSelect);
            resources.ApplyResources(this.m_grpFrom, "m_grpFrom");
            this.m_grpFrom.Name = "m_grpFrom";
            this.m_grpFrom.TabStop = false;
            // 
            // m_cbAutoSend
            // 
            resources.ApplyResources(this.m_cbAutoSend, "m_cbAutoSend");
            this.m_cbAutoSend.Name = "m_cbAutoSend";
            this.m_cbAutoSend.UseVisualStyleBackColor = true;
            // 
            // m_btnPasteProcess
            // 
            resources.ApplyResources(this.m_btnPasteProcess, "m_btnPasteProcess");
            this.m_btnPasteProcess.Name = "m_btnPasteProcess";
            this.m_btnPasteProcess.UseVisualStyleBackColor = true;
            this.m_btnPasteProcess.Click += new System.EventHandler(this.m_btnPasteProcess_Click);
            // 
            // m_grpMode
            // 
            this.m_grpMode.Controls.Add(this.m_cmbMode);
            this.m_grpMode.Controls.Add(this.label1);
            this.m_grpMode.Controls.Add(this.m_btnRetry);
            this.m_grpMode.Controls.Add(this.label2);
            this.m_grpMode.Controls.Add(this.m_txtOffset);
            resources.ApplyResources(this.m_grpMode, "m_grpMode");
            this.m_grpMode.Name = "m_grpMode";
            this.m_grpMode.TabStop = false;
            // 
            // m_btnStamp
            // 
            resources.ApplyResources(this.m_btnStamp, "m_btnStamp");
            this.m_btnStamp.Name = "m_btnStamp";
            this.m_btnStamp.UseVisualStyleBackColor = true;
            this.m_btnStamp.Click += new System.EventHandler(this.m_btnStamp_Click);
            // 
            // m_btnSave
            // 
            resources.ApplyResources(this.m_btnSave, "m_btnSave");
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
            // 
            // m_dlgSaveFile
            // 
            resources.ApplyResources(this.m_dlgSaveFile, "m_dlgSaveFile");
            // 
            // m_pnlFunc
            // 
            this.m_pnlFunc.Controls.Add(this.m_cbUseClasssic);
            this.m_pnlFunc.Controls.Add(this.m_btnAbout);
            this.m_pnlFunc.Controls.Add(this.m_cbLeftTopTransparent);
            this.m_pnlFunc.Controls.Add(this.groupBox3);
            this.m_pnlFunc.Controls.Add(this.m_grpFrom);
            this.m_pnlFunc.Controls.Add(this.m_btnSave);
            this.m_pnlFunc.Controls.Add(this.m_grpMode);
            this.m_pnlFunc.Controls.Add(this.m_btnStamp);
            this.m_pnlFunc.Controls.Add(this.m_btnTrim);
            resources.ApplyResources(this.m_pnlFunc, "m_pnlFunc");
            this.m_pnlFunc.Name = "m_pnlFunc";
            // 
            // m_cbUseClasssic
            // 
            resources.ApplyResources(this.m_cbUseClasssic, "m_cbUseClasssic");
            this.m_cbUseClasssic.Name = "m_cbUseClasssic";
            this.m_cbUseClasssic.UseVisualStyleBackColor = true;
            // 
            // m_btnAbout
            // 
            resources.ApplyResources(this.m_btnAbout, "m_btnAbout");
            this.m_btnAbout.Name = "m_btnAbout";
            this.m_btnAbout.UseVisualStyleBackColor = true;
            this.m_btnAbout.Click += new System.EventHandler(this.m_btnAbout_Click);
            // 
            // m_cbLeftTopTransparent
            // 
            resources.ApplyResources(this.m_cbLeftTopTransparent, "m_cbLeftTopTransparent");
            this.m_cbLeftTopTransparent.Name = "m_cbLeftTopTransparent";
            this.m_cbLeftTopTransparent.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_btnStopTimer);
            this.groupBox3.Controls.Add(this.m_btnStartTimer);
            this.groupBox3.Controls.Add(this.m_cmbTimer);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_btnStopTimer
            // 
            resources.ApplyResources(this.m_btnStopTimer, "m_btnStopTimer");
            this.m_btnStopTimer.Name = "m_btnStopTimer";
            this.m_btnStopTimer.UseVisualStyleBackColor = true;
            this.m_btnStopTimer.Click += new System.EventHandler(this.m_btnStopTimer_Click);
            // 
            // m_btnStartTimer
            // 
            resources.ApplyResources(this.m_btnStartTimer, "m_btnStartTimer");
            this.m_btnStartTimer.Name = "m_btnStartTimer";
            this.m_btnStartTimer.UseVisualStyleBackColor = true;
            this.m_btnStartTimer.Click += new System.EventHandler(this.m_btnStartTimer_Click);
            // 
            // m_cmbTimer
            // 
            this.m_cmbTimer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbTimer.FormattingEnabled = true;
            this.m_cmbTimer.Items.AddRange(new object[] {
            resources.GetString("m_cmbTimer.Items"),
            resources.GetString("m_cmbTimer.Items1"),
            resources.GetString("m_cmbTimer.Items2"),
            resources.GetString("m_cmbTimer.Items3"),
            resources.GetString("m_cmbTimer.Items4"),
            resources.GetString("m_cmbTimer.Items5"),
            resources.GetString("m_cmbTimer.Items6"),
            resources.GetString("m_cmbTimer.Items7"),
            resources.GetString("m_cmbTimer.Items8")});
            resources.ApplyResources(this.m_cmbTimer, "m_cmbTimer");
            this.m_cmbTimer.Name = "m_cmbTimer";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_stRGB,
            this.m_stColor});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // m_stRGB
            // 
            resources.ApplyResources(this.m_stRGB, "m_stRGB");
            this.m_stRGB.Name = "m_stRGB";
            // 
            // m_pnlTriming
            // 
            resources.ApplyResources(this.m_pnlTriming, "m_pnlTriming");
            this.m_pnlTriming.Name = "m_pnlTriming";
            this.m_pnlTriming.Scroll += new System.Windows.Forms.ScrollEventHandler(this.m_pnlTriming_Scroll);
            this.m_pnlTriming.Click += new System.EventHandler(this.m_pnlTriming_Click);
            this.m_pnlTriming.Paint += new System.Windows.Forms.PaintEventHandler(this.m_pnlTriming_Paint);
            this.m_pnlTriming.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_pnlTriming_MouseDown);
            this.m_pnlTriming.MouseHover += new System.EventHandler(this.m_pnlTriming_MouseHover);
            this.m_pnlTriming.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_pnlTriming_MouseMove);
            this.m_pnlTriming.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_pnlTriming_MouseUp);
            // 
            // m_stColor
            // 
            this.m_stColor.Name = "m_stColor";
            resources.ApplyResources(this.m_stColor, "m_stColor");
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_pnlTriming);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.m_pnlFunc);
            this.Name = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.m_grpFrom.ResumeLayout(false);
            this.m_grpFrom.PerformLayout();
            this.m_grpMode.ResumeLayout(false);
            this.m_grpMode.PerformLayout();
            this.m_pnlFunc.ResumeLayout(false);
            this.m_pnlFunc.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelEx m_pnlTriming;
        private System.Windows.Forms.Button m_btnPaste;
        private System.Windows.Forms.Button m_btnTrim;
        private System.Windows.Forms.TextBox m_txtOffset;
        private System.Windows.Forms.Button m_btnRetry;
        private System.Windows.Forms.ComboBox m_cmbMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox m_chkClearSelect;
        private System.Windows.Forms.GroupBox m_grpFrom;
        private System.Windows.Forms.GroupBox m_grpMode;
        private System.Windows.Forms.Button m_btnStamp;
        private System.Windows.Forms.Button m_btnSave;
        private System.Windows.Forms.SaveFileDialog m_dlgSaveFile;
        private System.Windows.Forms.Panel m_pnlFunc;
        private System.Windows.Forms.Button m_btnPasteProcess;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button m_btnStartTimer;
        private System.Windows.Forms.ComboBox m_cmbTimer;
        private System.Windows.Forms.Button m_btnStopTimer;
        private System.Windows.Forms.CheckBox m_cbLeftTopTransparent;
        private System.Windows.Forms.Button m_btnAbout;
        private System.Windows.Forms.CheckBox m_cbAutoSend;
        private System.Windows.Forms.CheckBox m_cbUseClasssic;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel m_stRGB;
        private System.Windows.Forms.ToolStripStatusLabel m_stColor;
    }
}

