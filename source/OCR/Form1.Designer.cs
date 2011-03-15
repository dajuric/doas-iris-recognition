namespace OCR
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.charDrawer2 = new OCR.CharDrawer(this.components);
            this.charDrawer1 = new OCR.CharDrawer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(13, 89);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(265, 189);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // charDrawer2
            // 
            this.charDrawer2.BackColor = System.Drawing.Color.Gainsboro;
            this.charDrawer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.charDrawer2.Cursor = System.Windows.Forms.Cursors.Cross;
            this.charDrawer2.Location = new System.Drawing.Point(122, 8);
            this.charDrawer2.Name = "charDrawer2";
            this.charDrawer2.Size = new System.Drawing.Size(75, 75);
            this.charDrawer2.TabIndex = 2;
            this.charDrawer2.TabStop = false;
            // 
            // charDrawer1
            // 
            this.charDrawer1.BackColor = System.Drawing.Color.Black;
            this.charDrawer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.charDrawer1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.charDrawer1.Location = new System.Drawing.Point(12, 8);
            this.charDrawer1.Name = "charDrawer1";
            this.charDrawer1.Size = new System.Drawing.Size(75, 75);
            this.charDrawer1.TabIndex = 0;
            this.charDrawer1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 227);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.charDrawer2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.charDrawer1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CharDrawer charDrawer1;
        private System.Windows.Forms.Button button1;
        private CharDrawer charDrawer2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button button2;
    }
}