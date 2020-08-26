namespace TrimmingHelper
{
    partial class ProcessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessForm));
            this.m_treeProcess = new System.Windows.Forms.TreeView();
            this.m_pnlPreview = new System.Windows.Forms.Panel();
            this.m_btnRefresh = new System.Windows.Forms.Button();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_sstatusbar = new System.Windows.Forms.StatusStrip();
            this.m_stLabelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_btnStop = new System.Windows.Forms.Button();
            this.m_sstatusbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_treeProcess
            // 
            resources.ApplyResources(this.m_treeProcess, "m_treeProcess");
            this.m_treeProcess.Name = "m_treeProcess";
            this.m_treeProcess.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_treeProcess_AfterSelect);
            // 
            // m_pnlPreview
            // 
            resources.ApplyResources(this.m_pnlPreview, "m_pnlPreview");
            this.m_pnlPreview.Name = "m_pnlPreview";
            this.m_pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.m_pnlPreview_Paint);
            // 
            // m_btnRefresh
            // 
            resources.ApplyResources(this.m_btnRefresh, "m_btnRefresh");
            this.m_btnRefresh.Name = "m_btnRefresh";
            this.m_btnRefresh.UseVisualStyleBackColor = true;
            this.m_btnRefresh.Click += new System.EventHandler(this.m_btnRefresh_Click);
            // 
            // m_btnOK
            // 
            resources.ApplyResources(this.m_btnOK, "m_btnOK");
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // m_btnCancel
            // 
            resources.ApplyResources(this.m_btnCancel, "m_btnCancel");
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
            // 
            // m_sstatusbar
            // 
            this.m_sstatusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_stLabelState});
            resources.ApplyResources(this.m_sstatusbar, "m_sstatusbar");
            this.m_sstatusbar.Name = "m_sstatusbar";
            // 
            // m_stLabelState
            // 
            this.m_stLabelState.Name = "m_stLabelState";
            resources.ApplyResources(this.m_stLabelState, "m_stLabelState");
            // 
            // m_btnStop
            // 
            resources.ApplyResources(this.m_btnStop, "m_btnStop");
            this.m_btnStop.Name = "m_btnStop";
            this.m_btnStop.UseVisualStyleBackColor = true;
            this.m_btnStop.Click += new System.EventHandler(this.m_btnStop_Click);
            // 
            // ProcessForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnStop);
            this.Controls.Add(this.m_sstatusbar);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.m_btnRefresh);
            this.Controls.Add(this.m_pnlPreview);
            this.Controls.Add(this.m_treeProcess);
            this.Name = "ProcessForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessForm_FormClosing);
            this.Load += new System.EventHandler(this.ProcessForm_Load);
            this.Resize += new System.EventHandler(this.ProcessForm_Resize);
            this.m_sstatusbar.ResumeLayout(false);
            this.m_sstatusbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView m_treeProcess;
        private System.Windows.Forms.Panel m_pnlPreview;
        private System.Windows.Forms.Button m_btnRefresh;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.StatusStrip m_sstatusbar;
        private System.Windows.Forms.ToolStripStatusLabel m_stLabelState;
        private System.Windows.Forms.Button m_btnStop;
    }
}