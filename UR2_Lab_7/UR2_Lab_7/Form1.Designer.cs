namespace UR2_Lab_7
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
            this.emguPictureBox = new System.Windows.Forms.PictureBox();
            this.roiPictureBox = new System.Windows.Forms.PictureBox();
            this.countc = new System.Windows.Forms.Label();
            this.countm = new System.Windows.Forms.Label();
            this.out_put = new System.Windows.Forms.Label();
            this.return1 = new System.Windows.Forms.Label();
            this.loc = new System.Windows.Forms.Label();
            this.Y1 = new System.Windows.Forms.TextBox();
            this.X1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SQ = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.emguPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // emguPictureBox
            // 
            this.emguPictureBox.Location = new System.Drawing.Point(537, 12);
            this.emguPictureBox.Name = "emguPictureBox";
            this.emguPictureBox.Size = new System.Drawing.Size(519, 404);
            this.emguPictureBox.TabIndex = 0;
            this.emguPictureBox.TabStop = false;
            // 
            // roiPictureBox
            // 
            this.roiPictureBox.Location = new System.Drawing.Point(12, 12);
            this.roiPictureBox.Name = "roiPictureBox";
            this.roiPictureBox.Size = new System.Drawing.Size(519, 404);
            this.roiPictureBox.TabIndex = 1;
            this.roiPictureBox.TabStop = false;
            // 
            // countc
            // 
            this.countc.AutoSize = true;
            this.countc.Location = new System.Drawing.Point(12, 432);
            this.countc.Name = "countc";
            this.countc.Size = new System.Drawing.Size(46, 17);
            this.countc.TabIndex = 2;
            this.countc.Text = "label1";
            // 
            // countm
            // 
            this.countm.AutoSize = true;
            this.countm.Location = new System.Drawing.Point(12, 486);
            this.countm.Name = "countm";
            this.countm.Size = new System.Drawing.Size(46, 17);
            this.countm.TabIndex = 3;
            this.countm.Text = "label2";
            // 
            // out_put
            // 
            this.out_put.AutoSize = true;
            this.out_put.Location = new System.Drawing.Point(522, 470);
            this.out_put.Name = "out_put";
            this.out_put.Size = new System.Drawing.Size(46, 17);
            this.out_put.TabIndex = 5;
            this.out_put.Text = "label2";
            // 
            // return1
            // 
            this.return1.AutoSize = true;
            this.return1.Location = new System.Drawing.Point(418, 548);
            this.return1.Name = "return1";
            this.return1.Size = new System.Drawing.Size(46, 17);
            this.return1.TabIndex = 9;
            this.return1.Text = "label1";
            // 
            // loc
            // 
            this.loc.AutoSize = true;
            this.loc.Location = new System.Drawing.Point(418, 565);
            this.loc.Name = "loc";
            this.loc.Size = new System.Drawing.Size(46, 17);
            this.loc.TabIndex = 10;
            this.loc.Text = "label1";
            // 
            // Y1
            // 
            this.Y1.Location = new System.Drawing.Point(12, 543);
            this.Y1.Name = "Y1";
            this.Y1.Size = new System.Drawing.Size(100, 22);
            this.Y1.TabIndex = 11;
            // 
            // X1
            // 
            this.X1.Location = new System.Drawing.Point(12, 520);
            this.X1.Name = "X1";
            this.X1.Size = new System.Drawing.Size(100, 22);
            this.X1.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(646, 519);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SQ
            // 
            this.SQ.Location = new System.Drawing.Point(12, 565);
            this.SQ.Name = "SQ";
            this.SQ.Size = new System.Drawing.Size(100, 22);
            this.SQ.TabIndex = 14;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(727, 520);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 615);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SQ);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.X1);
            this.Controls.Add(this.Y1);
            this.Controls.Add(this.loc);
            this.Controls.Add(this.return1);
            this.Controls.Add(this.out_put);
            this.Controls.Add(this.countm);
            this.Controls.Add(this.countc);
            this.Controls.Add(this.roiPictureBox);
            this.Controls.Add(this.emguPictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.emguPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox emguPictureBox;
        private System.Windows.Forms.PictureBox roiPictureBox;
        private System.Windows.Forms.Label countc;
        private System.Windows.Forms.Label countm;
        private System.Windows.Forms.Label out_put;
        private System.Windows.Forms.Label return1;
        private System.Windows.Forms.Label loc;
        private System.Windows.Forms.TextBox Y1;
        private System.Windows.Forms.TextBox X1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SQ;
        private System.Windows.Forms.Button button2;
    }
}

