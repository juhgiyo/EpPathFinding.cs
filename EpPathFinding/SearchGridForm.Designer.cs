namespace EpPathFinding
{
    partial class SearchGridForm
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClearPath = new System.Windows.Forms.Button();
            this.btnClearWall = new System.Windows.Forms.Button();
            this.cbDontCrossCorners = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(13, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClearPath
            // 
            this.btnClearPath.Location = new System.Drawing.Point(95, 12);
            this.btnClearPath.Name = "btnClearPath";
            this.btnClearPath.Size = new System.Drawing.Size(75, 23);
            this.btnClearPath.TabIndex = 1;
            this.btnClearPath.Text = "Clear Path";
            this.btnClearPath.UseVisualStyleBackColor = true;
            this.btnClearPath.Click += new System.EventHandler(this.btnClearPath_Click);
            // 
            // btnClearWall
            // 
            this.btnClearWall.Location = new System.Drawing.Point(177, 11);
            this.btnClearWall.Name = "btnClearWall";
            this.btnClearWall.Size = new System.Drawing.Size(104, 23);
            this.btnClearWall.TabIndex = 2;
            this.btnClearWall.Text = "Clear Walls";
            this.btnClearWall.UseVisualStyleBackColor = true;
            this.btnClearWall.Click += new System.EventHandler(this.btnClearWall_Click);
            // 
            // cbDontCrossCorners
            // 
            this.cbDontCrossCorners.AutoSize = true;
            this.cbDontCrossCorners.Location = new System.Drawing.Point(287, 16);
            this.cbDontCrossCorners.Name = "cbDontCrossCorners";
            this.cbDontCrossCorners.Size = new System.Drawing.Size(140, 16);
            this.cbDontCrossCorners.TabIndex = 4;
            this.cbDontCrossCorners.Text = "Don\'t Cross Corners";
            this.cbDontCrossCorners.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(613, 262);
            this.Controls.Add(this.cbDontCrossCorners);
            this.Controls.Add(this.btnClearWall);
            this.Controls.Add(this.btnClearPath);
            this.Controls.Add(this.btnSearch);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearPath;
        private System.Windows.Forms.Button btnClearWall;
        private System.Windows.Forms.CheckBox cbDontCrossCorners;
    }
}

