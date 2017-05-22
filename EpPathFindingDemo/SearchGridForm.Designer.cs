namespace EpPathFindingDemo
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
            this.cbUseRecursive = new System.Windows.Forms.CheckBox();
            this.cbbJumpType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(11, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 25);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClearPath
            // 
            this.btnClearPath.Location = new System.Drawing.Point(81, 13);
            this.btnClearPath.Name = "btnClearPath";
            this.btnClearPath.Size = new System.Drawing.Size(64, 25);
            this.btnClearPath.TabIndex = 1;
            this.btnClearPath.Text = "Clear Path";
            this.btnClearPath.UseVisualStyleBackColor = true;
            this.btnClearPath.Click += new System.EventHandler(this.btnClearPath_Click);
            // 
            // btnClearWall
            // 
            this.btnClearWall.Location = new System.Drawing.Point(152, 12);
            this.btnClearWall.Name = "btnClearWall";
            this.btnClearWall.Size = new System.Drawing.Size(89, 25);
            this.btnClearWall.TabIndex = 2;
            this.btnClearWall.Text = "Clear Walls";
            this.btnClearWall.UseVisualStyleBackColor = true;
            this.btnClearWall.Click += new System.EventHandler(this.btnClearWall_Click);
            // 
            // cbUseRecursive
            // 
            this.cbUseRecursive.AutoSize = true;
            this.cbUseRecursive.Location = new System.Drawing.Point(533, 17);
            this.cbUseRecursive.Name = "cbUseRecursive";
            this.cbUseRecursive.Size = new System.Drawing.Size(96, 17);
            this.cbUseRecursive.TabIndex = 6;
            this.cbUseRecursive.Text = "Use Recursive";
            this.cbUseRecursive.UseVisualStyleBackColor = true;
            // 
            // cbbJumpType
            // 
            this.cbbJumpType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbJumpType.FormattingEnabled = true;
            this.cbbJumpType.Location = new System.Drawing.Point(338, 16);
            this.cbbJumpType.Name = "cbbJumpType";
            this.cbbJumpType.Size = new System.Drawing.Size(189, 21);
            this.cbbJumpType.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(247, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Diagonal Mode: ";
            // 
            // SearchGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(693, 284);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbJumpType);
            this.Controls.Add(this.cbUseRecursive);
            this.Controls.Add(this.btnClearWall);
            this.Controls.Add(this.btnClearPath);
            this.Controls.Add(this.btnSearch);
            this.MaximizeBox = false;
            this.Name = "SearchGridForm";
            this.Text = "EpPathFinding.cs Demo";
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
        private System.Windows.Forms.CheckBox cbUseRecursive;
        private System.Windows.Forms.ComboBox cbbJumpType;
        private System.Windows.Forms.Label label1;
    }
}

