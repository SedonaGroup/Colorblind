namespace ColorFormTracker
{
    partial class HiddenForm
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
            this.displayColorName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // displayColorName
            // 
            this.displayColorName.AutoSize = true;
            this.displayColorName.Location = new System.Drawing.Point(3, 9);
            this.displayColorName.Name = "displayColorName";
            this.displayColorName.Size = new System.Drawing.Size(35, 13);
            this.displayColorName.TabIndex = 0;
            this.displayColorName.Text = "label1";
            // 
            // HiddenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 24);
            this.Controls.Add(this.displayColorName);
            this.Name = "HiddenForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label displayColorName;
    }
}

