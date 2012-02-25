namespace ArcaneDefense
{
    partial class frmArcaneDefense
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
            this.SuspendLayout();
            // 
            // frmArcaneDefense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(451, 510);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "frmArcaneDefense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arcane Defense";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmArcaneDefense_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmArcaneDefense_MouseClick);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmArcaneDefense_KeyPress);
            this.Resize += new System.EventHandler(this.frmArcaneDefense_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmArcaneDefense_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmArcaneDefense_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion



    }
}

