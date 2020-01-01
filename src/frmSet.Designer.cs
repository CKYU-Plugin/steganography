namespace Steganography
{
    partial class frmSet
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lbAdminQq = new System.Windows.Forms.Label();
            this.txtAdminQq = new System.Windows.Forms.TextBox();
            this.tlpSet = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.Location = new System.Drawing.Point(12, 50);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(253, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "储存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbAdminQq
            // 
            this.lbAdminQq.AutoSize = true;
            this.lbAdminQq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAdminQq.Location = new System.Drawing.Point(3, 0);
            this.lbAdminQq.Name = "lbAdminQq";
            this.lbAdminQq.Size = new System.Drawing.Size(74, 25);
            this.lbAdminQq.TabIndex = 1;
            this.lbAdminQq.Text = "主人QQ";
            this.lbAdminQq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAdminQq
            // 
            this.txtAdminQq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdminQq.Location = new System.Drawing.Point(83, 3);
            this.txtAdminQq.Name = "txtAdminQq";
            this.txtAdminQq.Size = new System.Drawing.Size(167, 22);
            this.txtAdminQq.TabIndex = 2;
            // 
            // tlpSet
            // 
            this.tlpSet.ColumnCount = 2;
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSet.Controls.Add(this.lbAdminQq, 0, 0);
            this.tlpSet.Controls.Add(this.txtAdminQq, 1, 0);
            this.tlpSet.Location = new System.Drawing.Point(13, 13);
            this.tlpSet.Name = "tlpSet";
            this.tlpSet.RowCount = 2;
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSet.Size = new System.Drawing.Size(253, 26);
            this.tlpSet.TabIndex = 3;
            // 
            // frmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(278, 84);
            this.Controls.Add(this.tlpSet);
            this.Controls.Add(this.btnSave);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSet_FormClosing);
            this.Shown += new System.EventHandler(this.frmSet_Shown);
            this.tlpSet.ResumeLayout(false);
            this.tlpSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbAdminQq;
        private System.Windows.Forms.TextBox txtAdminQq;
        private System.Windows.Forms.TableLayoutPanel tlpSet;
    }
}