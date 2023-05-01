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
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.MoveBall);
			// 
			// colorSelect
			// 
			this.colorSelect.FormattingEnabled = true;
			this.colorSelect.Items.AddRange(new object[] {
			"modrá",
			"červená",
			"zelená",
			"žlutá",
			"fialová"});
			this.colorSelect.Location = new System.Drawing.Point(12, 12);
			this.colorSelect.Name = "colorSelect";
			this.colorSelect.Size = new System.Drawing.Size(121, 24);
			this.colorSelect.TabIndex = 0;
			this.colorSelect.Text = "barva";
			this.colorSelect.SelectedIndexChanged += new System.EventHandler(this.ColorSelectSelectedIndexChanged);
			// 
			// speedSelect
			// 
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
			this.speedSelect.Location = new System.Drawing.Point(139, 12);
			this.speedSelect.Name = "speedSelect";
			this.speedSelect.Size = new System.Drawing.Size(121, 24);
			this.speedSelect.TabIndex = 1;
			this.speedSelect.Text = "rychlost";
			this.speedSelect.SelectedIndexChanged += new System.EventHandler(this.SpeedSelectSelectedIndexChanged);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(277, 12);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(529, 23);
			this.progressBar1.TabIndex = 3;
			this.progressBar1.Click += new System.EventHandler(this.ProgressBar1Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1378, 661);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.speedSelect);
			this.Controls.Add(this.colorSelect);
			this.Name = "MainForm";
			this.Text = "KulecnikAmon";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintCircle);
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.ComboBox speedSelect;
		private System.Windows.Forms.ComboBox colorSelect;
		
	}
}
