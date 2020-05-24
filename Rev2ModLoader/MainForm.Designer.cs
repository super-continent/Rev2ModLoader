namespace Rev2ModLoader
{
    partial class MainForm
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
            this.buttonExtractScript = new System.Windows.Forms.Button();
            this.comboScriptSelected = new System.Windows.Forms.ComboBox();
            this.buttonInject = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonExtractScript
            // 
            this.buttonExtractScript.Location = new System.Drawing.Point(12, 39);
            this.buttonExtractScript.Name = "buttonExtractScript";
            this.buttonExtractScript.Size = new System.Drawing.Size(139, 130);
            this.buttonExtractScript.TabIndex = 0;
            this.buttonExtractScript.Text = "Extract Script";
            this.buttonExtractScript.UseVisualStyleBackColor = true;
            this.buttonExtractScript.Click += new System.EventHandler(this.buttonExtractScript_Click);
            // 
            // comboScriptSelected
            // 
            this.comboScriptSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScriptSelected.FormattingEnabled = true;
            this.comboScriptSelected.Items.AddRange(new object[] {
            "P1",
            "P1 ETC",
            "P2",
            "P2 ETC",
            "CMN",
            "CMNEF (Mechanics)"});
            this.comboScriptSelected.Location = new System.Drawing.Point(12, 12);
            this.comboScriptSelected.Name = "comboScriptSelected";
            this.comboScriptSelected.Size = new System.Drawing.Size(139, 21);
            this.comboScriptSelected.TabIndex = 1;
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(157, 12);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(135, 157);
            this.buttonInject.TabIndex = 2;
            this.buttonInject.Text = "Enable Mods";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 181);
            this.Controls.Add(this.buttonInject);
            this.Controls.Add(this.comboScriptSelected);
            this.Controls.Add(this.buttonExtractScript);
            this.MaximumSize = new System.Drawing.Size(320, 220);
            this.MinimumSize = new System.Drawing.Size(320, 220);
            this.Name = "MainForm";
            this.Text = "Rev2 Mod Loader";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExtractScript;
        private System.Windows.Forms.ComboBox comboScriptSelected;
        private System.Windows.Forms.Button buttonInject;
    }
}

