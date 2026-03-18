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
        private const int BallSize = 50;
        private const int DefaultMoveStep = 2;
        private const int MinSpeed = 1;
        private const int MaxSpeed = 20;
        private const int DefaultSpeed = 1;
        private const int DefaultColorIndex = 2;
        private const int DefaultBallX = 200;
        private const int DefaultBallY = 150;
		
		private int _ballWidth = BallSize;
		private int _ballHeight = BallSize;
        private long _ballPosX = DefaultBallX;
        private long _ballPosY = DefaultBallY;
		private int _moveStepX = DefaultMoveStep;
		private int _moveStepY = DefaultMoveStep;
		private int _speed = DefaultSpeed;
		private Brush _ballBrush = Brushes.Green;
	
		
		public MainForm()
		{
			InitializeComponent();
            progressBar1.Maximum = MaxSpeed;
			
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			
			
		UpdateStyles();
		
        colorSelect.SelectedIndex = DefaultColorIndex;
	speedSelect.SelectedIndex = 0;
	progressBar1.Value = DefaultSpeed;
		}
    private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				
			e.Graphics.Clear(BackColor);
			
            e.Graphics.FillEllipse(_ballBrush, _ballPosX, _ballPosY, _ballWidth, _ballHeight);
			
            using (Pen outlinePen = new Pen(Color.Black, 2))
{
e.Graphics.DrawEllipse(outlinePen, _ballPosX, _ballPosY, _ballWidth, _ballHeight);
}

using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
{
e.Graphics.FillEllipse(highlightBrush, _ballPosX + 10, _ballPosY + 5, _ballWidth / 3, _ballHeight / 3);
}
		}

    private void Timer_Tick(object sender, EventArgs e)
		{
	_ballPosX += _moveStepX * _speed;
			
	if (_ballPosX < 0 ||
	    _ballPosX + _ballWidth > ClientSize.Width) {
		_moveStepX = -_moveStepX;
			MoveColor();
		}
	_ballPosY += _moveStepY * _speed;
	if (_ballPosY < 0 ||
	    _ballPosY + _ballHeight > ClientSize.Height) 
	{
		_moveStepY = -_moveStepY;
			MoveColor();
		}
		Invalidate();
	}
		
    private void SpeedSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
	if (int.TryParse(speedSelect.Text, out int parsedSpeed))
	{
	if (parsedSpeed >= MinSpeed && parsedSpeed <= MaxSpeed)
	{
	_speed = parsedSpeed;
	progressBar1.Value = parsedSpeed;
	}
	}
    }
		
        private void MoveColor() 
        {
		int actualColor = colorSelect.SelectedIndex;
			
		if (actualColor < colorSelect.Items.Count - 1) {
				actualColor++;				
			} else {
				actualColor = 0;
			}	
			
			colorSelect.SelectedIndex = actualColor;			
		this.ChangeColor(colorSelect.Text);			
		}
		
    private void ChangeColor(string colorName)
    {
	switch (colorName)
	{
	case "modrá":
	_ballBrush = Brushes.Blue;
	break;
	case "červená":
	_ballBrush = Brushes.Red;
	break;
	case "zelená":
	_ballBrush = Brushes.Green;
	break;
	case "žlutá":
	_ballBrush = Brushes.Yellow;
	break;
	case "fialová":
	_ballBrush = Brushes.Purple;
	break;
	case "černá":
	_ballBrush = Brushes.Black;
	break;
	case "oranžová":
	_ballBrush = Brushes.Orange;
	break;
	case "hnědá":
	_ballBrush = Brushes.Brown;
	break;
	case "růžová":
	_ballBrush = Brushes.Pink;
	break;
	case "azurová":
	_ballBrush = Brushes.Cyan;
	break;
	default:
	_ballBrush = Brushes.Green;
	break;
	}
    }
		
    private void ColorSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
	ChangeColor(colorSelect.Text);
	Invalidate();
    }
		
    private void ProgressBar1_Click(object sender, EventArgs e)
	{			
		float absoluteMouse = (PointToClient(MousePosition).X - progressBar1.Bounds.X);
		float calcFactor = progressBar1.Width / (float)progressBar1.Maximum;
		float relativeMouse = absoluteMouse / calcFactor;

		int newSpeed = Convert.ToInt32(relativeMouse);
		
	if (newSpeed < MinSpeed)
		newSpeed = MinSpeed;
	if (newSpeed > MaxSpeed)
		newSpeed = MaxSpeed;
		
        _speed = progressBar1.Value = newSpeed;
		if (newSpeed > 0 && newSpeed <= speedSelect.Items.Count)
speedSelect.SelectedIndex = newSpeed - 1; 
	}
	}
}
