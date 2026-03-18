/*
 * Vytvořeno aplikací SharpDevelop.
 * Uživatel: amonv
 * Datum: 11.11.2022
 * Čas: 1:13
 * 
 * Tento template můžete změnit pomocí Nástroje | Možnosti | Psaní kódu | Upravit standardní hlavičky souborů.
 */
namespace KulecnikAmon
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Timer timer1;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorSelect = new System.Windows.Forms.ComboBox();
            this.speedSelect = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.colorLabel = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // colorSelect
            // 
            this.colorSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorSelect.FormattingEnabled = true;
            this.colorSelect.Items.AddRange(new object[] {
            "modrá",
            "červená",
            "zelená",
            "žlutá",
            "fialová",
            "černá",
            "oranžová",
            "hnědá",
            "růžová",
            "azurová"});
            this.colorSelect.Location = new System.Drawing.Point(52, 10);
            this.colorSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.colorSelect.Name = "colorSelect";
            this.colorSelect.Size = new System.Drawing.Size(87, 21);
            this.colorSelect.TabIndex = 0;
            this.colorSelect.SelectedIndexChanged += new System.EventHandler(this.ColorSelect_SelectedIndexChanged);
            // 
            // speedSelect
            // 
            this.speedSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.speedSelect.FormattingEnabled = true;
            this.speedSelect.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.speedSelect.Location = new System.Drawing.Point(199, 10);
            this.speedSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.speedSelect.Name = "speedSelect";
            this.speedSelect.Size = new System.Drawing.Size(46, 21);
            this.speedSelect.TabIndex = 1;
            this.speedSelect.SelectedIndexChanged += new System.EventHandler(this.SpeedSelect_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(255, 10);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(435, 19);
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Click += new System.EventHandler(this.ProgressBar1_Click);
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(9, 12);
            this.colorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(38, 13);
            this.colorLabel.TabIndex = 4;
            this.colorLabel.Text = "Barva:";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(146, 12);
            this.speedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(51, 13);
            this.speedLabel.TabIndex = 5;
            this.speedLabel.Text = "Rychlost:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(1034, 537);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.speedSelect);
            this.Controls.Add(this.colorSelect);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Animace skákající koule";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

}
private System.Windows.Forms.ProgressBar progressBar1;
private System.Windows.Forms.ComboBox speedSelect;
private System.Windows.Forms.ComboBox colorSelect;
private System.Windows.Forms.Label colorLabel;
private System.Windows.Forms.Label speedLabel;
		
	}
}
