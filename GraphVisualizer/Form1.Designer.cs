namespace GraphVisualizer
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
            this.graphInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // graphInput
            // 
            this.graphInput.Location = new System.Drawing.Point(0, 0);
            this.graphInput.Multiline = true;
            this.graphInput.Name = "graphInput";
            this.graphInput.Size = new System.Drawing.Size(100, 602);
            this.graphInput.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 676);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(106, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(880, 711);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(0, 645);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(88, 25);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // zoomInButton
            // 
            this.zoomInButton.Location = new System.Drawing.Point(13, 609);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(33, 30);
            this.zoomInButton.TabIndex = 4;
            this.zoomInButton.Text = "+";
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Location = new System.Drawing.Point(52, 609);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(33, 30);
            this.zoomOutButton.TabIndex = 5;
            this.zoomOutButton.Text = "-";
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(998, 713);
            this.Controls.Add(this.zoomOutButton);
            this.Controls.Add(this.zoomInButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.graphInput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox graphInput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button zoomInButton;
        private System.Windows.Forms.Button zoomOutButton;
    }
}

