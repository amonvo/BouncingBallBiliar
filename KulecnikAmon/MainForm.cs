/*
 * Vytvořeno aplikací SharpDevelop.
 * Uživatel: amonv
 * Datum: 11.11.2022
 * Čas: 1:13
 * 
 * Tento template můžete změnit pomocí Nástroje | Možnosti | Psaní kódu | Upravit standardní hlavičky souborů.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KulecnikAmon
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private int ballWidth = 50;
		private int ballHeight = 50;
		public long ballPosX = 0;
		public long ballPosY = 0;
		private int moveStepX = 2;
		private int moveStepY = 2;
		public  int speed = 1;
		public Brush bColor = Brushes.Green;
	
		
		public MainForm()
		{
			InitializeComponent();
			progressBar1.Maximum = 20;
			
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			
			
			this.UpdateStyles();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void PaintCircle (object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				
			e.Graphics.Clear(this.BackColor);
			
			e.Graphics.FillEllipse(this.bColor, ballPosX, ballPosY, ballWidth, ballHeight);
			
			e.Graphics.DrawEllipse(Pens.Black, ballPosX, ballPosY, ballWidth, ballHeight);
		}

		void MoveBall(object sender, EventArgs e)
		{
			ballPosX += moveStepX*this.speed;
			
			if (ballPosX < 0 ||
			    ballPosX + ballWidth > this.ClientSize.Width) {
				moveStepX = -moveStepX;
				this.moveColor();
			}
			ballPosY += moveStepY*this.speed;
			if (ballPosY < 0 ||
			    ballPosY + ballHeight > this.ClientSize.Height) 
			{
				moveStepY = -moveStepY;
				this.moveColor();
			}
			this.Refresh();
		}
		
		void SpeedSelectSelectedIndexChanged(object sender, EventArgs e)
		{
			this.speed = int.Parse(speedSelect.Text);
		}
		
		void moveColor() 
		{
			int actualColor = colorSelect.SelectedIndex;
			
			if (actualColor < colorSelect.Items.Count-1) {
				actualColor++;				
			} else {
				actualColor = 0;
			}	
			
			colorSelect.SelectedIndex = actualColor;			
			this.changeColor(colorSelect.Text);			
		}
		
		void changeColor(String barva)
		{
			switch(barva) {
				case "modrá":
					this.bColor = Brushes.Blue;
					break;
				case "červená":
					this.bColor = Brushes.Red;
					break;
				case "zelená":
					this.bColor = Brushes.Green;
					break;
				case "žlutá":
					this.bColor = Brushes.Yellow;
					break;
				case "fialová":
					this.bColor = Brushes.Purple;
					break;
				default:
					this.bColor = Brushes.Green;
					break;
			}			
		}
		
		void ColorSelectSelectedIndexChanged(object sender, EventArgs e)
		{
			this.changeColor(colorSelect.Text);

		}
		
		void ProgressBar1Click(object sender, EventArgs e)
		{			
			float absoluteMouse = (PointToClient(MousePosition).X - progressBar1.Bounds.X);
            float calcFactor = progressBar1.Width / (float)progressBar1.Maximum;
            float relativeMouse = absoluteMouse / calcFactor;

            this.speed = progressBar1.Value = Convert.ToInt32(relativeMouse);
            speedSelect.Text = this.speed.ToString(); 
		}
	}
}
