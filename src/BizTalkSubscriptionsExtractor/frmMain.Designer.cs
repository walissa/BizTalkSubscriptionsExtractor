
namespace BizTalkSubscriptionsExtractor
{
    partial class frmMain
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
            this.btnExport = new System.Windows.Forms.Button();
            this.txtMgmtDB = new System.Windows.Forms.TextBox();
            this.txtMsgBoxDB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dsBTSubscription1 = new BizTalkSubscriptionsExtractor.dsBTSubscription();
            ((System.ComponentModel.ISupportInitialize)(this.dsBTSubscription1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(498, 65);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Export Subscriptions";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtMgmtDB
            // 
            this.txtMgmtDB.Location = new System.Drawing.Point(118, 12);
            this.txtMgmtDB.Name = "txtMgmtDB";
            this.txtMgmtDB.Size = new System.Drawing.Size(530, 20);
            this.txtMgmtDB.TabIndex = 1;
            // 
            // txtMsgBoxDB
            // 
            this.txtMsgBoxDB.Location = new System.Drawing.Point(118, 39);
            this.txtMsgBoxDB.Name = "txtMsgBoxDB";
            this.txtMsgBoxDB.Size = new System.Drawing.Size(530, 20);
            this.txtMsgBoxDB.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "BizTalkMgmtDB:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "BizTalkMsgBoxDB:";
            // 
            // dsBTSubscription1
            // 
            this.dsBTSubscription1.DataSetName = "dsBTSubscription";
            this.dsBTSubscription1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 96);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMsgBoxDB);
            this.Controls.Add(this.txtMgmtDB);
            this.Controls.Add(this.btnExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BizTalk Subscriptions Extractor";
            ((System.ComponentModel.ISupportInitialize)(this.dsBTSubscription1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private dsBTSubscription dsBTSubscription1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtMgmtDB;
        private System.Windows.Forms.TextBox txtMsgBoxDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}