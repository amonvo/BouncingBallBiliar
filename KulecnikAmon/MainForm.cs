/*
 * Vytvoøeno aplikací SharpDevelop.
 * Uživatel: amonv
 * Datum: 11.11.2022
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace KulecnikAmon
{
    public class MainForm : Form
    {
        // ?? constants ?????????????????????????????????????????????????????????????
        private const int MinSpeed = 1;
        private const int MaxSpeed = 20;
        private const int BallDia  = 28;

        private static readonly Color[] BallColors =
        {
            Color.Yellow,       Color.Blue,         Color.Red,       Color.Purple,
            Color.Orange,       Color.Green,        Color.Maroon,    Color.Black,
            Color.Gold,         Color.DodgerBlue,   Color.Crimson,
            Color.MediumPurple, Color.DarkOrange,   Color.DarkGreen, Color.Brown
        };

        // ?? infrastructure ????????????????????????????????????????????????????????
        private IContainer components = null;
        private Timer      timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        // ?? simulation fields ??????????????????????????????????????????????????????
        private List<Ball> _balls    = new List<Ball>();
        private int        _speed    = 5;
        private bool       _paused   = false;
        private Random     _rng      = new Random();
        private Ball       _cueBall  = null;
        private bool       _aiming   = false;
        private Point      _mousePos = Point.Empty;

        // ?? toolbar controls ???????????????????????????????????????????????????????
        private Label    lblSpeed;
        private TrackBar speedTrackBar;
        private Label    lblSpeedVal;
        private Button   pauseButton;
        private Button   resetButton;
        private Label    totalBouncesLabel;

        // ?? felt geometry helpers ?????????????????????????????????????????????????
        private int FeltLeft    { get { return 40; } }
        private int FeltRight   { get { return ClientSize.Width  - 40; } }
        private int FeltTop     { get { return 85; } }
        private int FeltBottom  { get { return ClientSize.Height - 40; } }
        private int FeltCenterY { get { return (FeltTop + FeltBottom) / 2; } }

        // ?? constructor ????????????????????????????????????????????????????????????
        public MainForm()
        {
            components = new Container();

            // Timer
            timer1          = new Timer(components);
            timer1.Interval = 25;   // matches default speedTrackBar.Value=5: max(5, 30-5)
            timer1.Tick    += Timer_Tick;
            timer1.Enabled  = true;

            // Form properties
            this.Text                = "Bouncing Ball Simulator";
            this.BackColor           = Color.FromArgb(30, 30, 30);
            this.ClientSize          = new System.Drawing.Size(1034, 537);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = AutoScaleMode.Font;
            this.Paint               += MainForm_Paint;
            this.MouseMove           += MainForm_MouseMove;
            this.MouseDown           += MainForm_MouseDown;
            this.MouseUp             += MainForm_MouseUp;

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint  |
                ControlStyles.UserPaint, true);
            UpdateStyles();

            // ?? Speed label ???????????????????????????????????????????????????????
            lblSpeed           = new Label();
            lblSpeed.Text      = "Speed:";
            lblSpeed.ForeColor = Color.White;
            lblSpeed.Location  = new System.Drawing.Point(10, 12);
            lblSpeed.AutoSize  = true;

            // ?? Speed trackbar ????????????????????????????????????????????????????
            speedTrackBar               = new TrackBar();
            speedTrackBar.Minimum       = MinSpeed;
            speedTrackBar.Maximum       = MaxSpeed;
            speedTrackBar.Value         = 5;
            speedTrackBar.Width         = 180;
            speedTrackBar.Height        = 30;
            speedTrackBar.Location      = new System.Drawing.Point(65, 8);
            speedTrackBar.TickFrequency = 5;
            speedTrackBar.ValueChanged += SpeedTrackBar_ValueChanged;

            // ?? Speed value label ?????????????????????????????????????????????????
            lblSpeedVal           = new Label();
            lblSpeedVal.Text      = "5";
            lblSpeedVal.ForeColor = Color.White;
            lblSpeedVal.Width     = 25;
            lblSpeedVal.Location  = new System.Drawing.Point(250, 12);

            // ?? Pause button ??????????????????????????????????????????????????????
            pauseButton           = new Button();
            pauseButton.Text      = "\u23F8 Pause";
            pauseButton.Width     = 90;
            pauseButton.Height    = 28;
            pauseButton.Location  = new System.Drawing.Point(290, 9);
            pauseButton.FlatStyle = FlatStyle.Flat;
            pauseButton.BackColor = Color.FromArgb(60, 60, 60);
            pauseButton.ForeColor = Color.White;
            pauseButton.Click    += PauseButton_Click;

            // ?? Reset button ??????????????????????????????????????????????????????
            resetButton           = new Button();
            resetButton.Text      = "\u21BA Reset";
            resetButton.Width     = 80;
            resetButton.Height    = 28;
            resetButton.Location  = new System.Drawing.Point(388, 9);
            resetButton.FlatStyle = FlatStyle.Flat;
            resetButton.BackColor = Color.FromArgb(60, 60, 60);
            resetButton.ForeColor = Color.White;
            resetButton.Click    += ResetButton_Click;

            // ?? Total bounces label ???????????????????????????????????????????????
            totalBouncesLabel           = new Label();
            totalBouncesLabel.Text      = "Bounces: 0";
            totalBouncesLabel.ForeColor = Color.White;
            totalBouncesLabel.Width     = 130;
            totalBouncesLabel.Location  = new System.Drawing.Point(480, 12);
            totalBouncesLabel.AutoSize  = false;

            this.Controls.Add(lblSpeed);
            this.Controls.Add(speedTrackBar);
            this.Controls.Add(lblSpeedVal);
            this.Controls.Add(pauseButton);
            this.Controls.Add(resetButton);
            this.Controls.Add(totalBouncesLabel);

            InitializeBalls();
        }

        // ?? ball initialisation ????????????????????????????????????????????????????
        private void InitializeBalls()
        {
            _balls.Clear();
            _aiming = false;

            int ballR  = BallDia / 2;
            int feltW  = FeltRight - FeltLeft;   // 954 px at default size

            // ?? Cue ball: 1/4 felt width from felt left edge ??????????????????????
            int cueCX = FeltLeft + feltW / 4;
            _cueBall = new Ball
            {
                X           = cueCX - ballR,
                Y           = FeltCenterY - ballR,
                VelocityX   = 0,
                VelocityY   = 0,
                Size        = BallDia,
                BallColor   = Color.White,
                Number      = 0,
                BounceCount = 0
            };
            _balls.Add(_cueBall);

            // ?? Triangle rack: tip at 3/4 felt width ??????????????????????????????
            // Equilateral triangle packing:
            //   horizontal step per row : sqrt(3)/2 * (dia+1) ? 25 px
            //   vertical spacing in row : dia + 1 = 29 px
            int    dy   = BallDia + 1;                            // 29
            double dxD  = dy * Math.Sqrt(3.0) / 2.0;             // ? 25.1
            int    dx   = (int)Math.Round(dxD);                   // 25

            int rackTipCX = FeltLeft + feltW * 3 / 4;
            int rackTipCY = FeltCenterY;

            int ballNum = 1;
            int[] rowCounts = { 1, 2, 3, 4, 5 };

            for (int row = 0; row < rowCounts.Length; row++)
            {
                int count = rowCounts[row];
                int rowCX = rackTipCX + row * dx;

                for (int col = 0; col < count; col++)
                {
                    // Vertically centre the row, then distribute cols evenly
                    double rowCY = rackTipCY + (col - (count - 1) / 2.0) * dy;

                    _balls.Add(new Ball
                    {
                        X           = rowCX - ballR,
                        Y           = (int)Math.Round(rowCY) - ballR,
                        VelocityX   = 0,
                        VelocityY   = 0,
                        Size        = BallDia,
                        BallColor   = BallColors[(ballNum - 1) % BallColors.Length],
                        Number      = ballNum,
                        BounceCount = 0
                    });
                    ballNum++;
                }
            }
        }

        // ?? pocket positions ??????????????????????????????????????????????????????
        private System.Drawing.Point[] GetPocketPositions()
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            return new[]
            {
                new System.Drawing.Point(40,     85),
                new System.Drawing.Point(w / 2,  85),
                new System.Drawing.Point(w - 40, 85),
                new System.Drawing.Point(40,     h - 40),
                new System.Drawing.Point(w / 2,  h - 40),
                new System.Drawing.Point(w - 40, h - 40)
            };
        }

        // ?? painting ??????????????????????????????????????????????????????????????
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(30, 30, 30));

            int w = ClientSize.Width;
            int h = ClientSize.Height;

            // 1. Outer wood frame
            using (SolidBrush woodBrush = new SolidBrush(Color.FromArgb(101, 67, 33)))
                g.FillRectangle(woodBrush, 0, 45, w, h - 45);

            // 2. Green felt inner play area
            using (SolidBrush feltBrush = new SolidBrush(Color.FromArgb(0, 120, 0)))
                g.FillRectangle(feltBrush, 40, 85, w - 80, h - 125);

            // 3. Felt shadow/border (darker green outline)
            using (Pen feltBorder = new Pen(Color.FromArgb(0, 80, 0), 3))
                g.DrawRectangle(feltBorder, 40, 85, w - 80, h - 125);

            // 4. Pockets
            int pr = 18;
            foreach (System.Drawing.Point p in GetPocketPositions())
            {
                using (SolidBrush pocketFill = new SolidBrush(Color.Black))
                    g.FillEllipse(pocketFill, p.X - pr, p.Y - pr, pr * 2, pr * 2);
                using (Pen pocketOutline = new Pen(Color.FromArgb(60, 60, 60), 2))
                    g.DrawEllipse(pocketOutline, p.X - pr, p.Y - pr, pr * 2, pr * 2);
            }

            // 5. Centre dashed line
            int lineY = 85 + (h - 125) / 2;
            using (Pen dashPen = new Pen(Color.FromArgb(0, 100, 0), 1))
            {
                dashPen.DashStyle = DashStyle.Dash;
                g.DrawLine(dashPen, 40, lineY, w - 40, lineY);
            }

            // 6. Centre circle
            int cx = w / 2;
            int cy = 85 + (h - 125) / 2;
            using (Pen circlePen = new Pen(Color.FromArgb(0, 100, 0), 1))
                g.DrawEllipse(circlePen, cx - 40, cy - 40, 80, 80);

            // Balls
            foreach (Ball ball in _balls)
                DrawBall(g, ball);

            // Cue stick overlay (drawn on top of everything)
            if (_aiming && _cueBall != null)
                DrawCueStick(g);
        }

        private void DrawBall(Graphics g, Ball ball)
        {
            float x    = (float)ball.X;
            float y    = (float)ball.Y;
            float size = (float)ball.Size;

            using (SolidBrush fill = new SolidBrush(ball.BallColor))
                g.FillEllipse(fill, x, y, size, size);

            using (Pen outline = new Pen(Color.Black, 2))
                g.DrawEllipse(outline, x, y, size, size);

            using (SolidBrush highlight = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                g.FillEllipse(highlight, x + 8f, y + 5f, size / 3f, size / 3f);

            // No number on cue ball (Number == 0)
            if (ball.Number == 0) return;

            // Use dark text on light-coloured balls for contrast
            bool useDark = ball.BallColor == Color.Yellow  || ball.BallColor == Color.Gold
                        || ball.BallColor == Color.White   || ball.BallColor == Color.Cyan
                        || ball.BallColor == Color.Lime;
            Color textColor = useDark ? Color.Black : Color.White;
            float fontSize  = Math.Max(8f, size / 3f);

            using (Font numFont = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment     = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(ball.Number.ToString(), numFont, textBrush,
                             x + size / 2f, y + size / 2f, sf);
            }
        }

        private void DrawCueStick(Graphics g)
        {
            double cueCX = _cueBall.X + _cueBall.Size / 2.0;
            double cueCY = _cueBall.Y + _cueBall.Size / 2.0;
            double dx    = cueCX - _mousePos.X;
            double dy    = cueCY - _mousePos.Y;
            double dist  = Math.Sqrt(dx * dx + dy * dy);
            if (dist < 1) return;

            // Normalised direction: mouse ? ball (= shot direction)
            double nx = dx / dist;
            double ny = dy / dist;

            // Cue stick: thick wood-coloured line from mouse to 80 px past ball centre
            double stickEndX = cueCX + nx * 80;
            double stickEndY = cueCY + ny * 80;
            using (Pen cuePen = new Pen(Color.FromArgb(180, 120, 40), 6))
                g.DrawLine(cuePen,
                    (float)_mousePos.X, (float)_mousePos.Y,
                    (float)stickEndX,   (float)stickEndY);

            // Trajectory hint: thin dotted white line in shot direction, 120 px
            using (Pen trajPen = new Pen(Color.FromArgb(200, 255, 255, 255), 1))
            {
                trajPen.DashStyle = DashStyle.Dot;
                g.DrawLine(trajPen,
                    (float)cueCX, (float)cueCY,
                    (float)(cueCX + nx * 120), (float)(cueCY + ny * 120));
            }

            // Power indicator bar near mouse cursor (matches shot power: clamp(dist,50,300)/30)
            double power = Math.Min(300.0, Math.Max(50.0, dist)) / 30.0;
            int    barW  = (int)((Math.Min(dist, 300.0) - 50.0) / 250.0 * 60.0);
            if (barW < 0) barW = 0;
            int    indX  = _mousePos.X + 15;
            int    indY  = _mousePos.Y - 22;

            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                g.FillRectangle(bgBrush, indX, indY, 62, 14);

            // Bar colour: green ? red with increasing power
            Color barColor = Color.FromArgb(220,
                (int)(255 * power / 15.0),
                (int)(255 * (1.0 - power / 15.0)), 0);
            using (SolidBrush barBrush = new SolidBrush(barColor))
                g.FillRectangle(barBrush, indX + 1, indY + 1, barW, 12);

            // Power label
            using (Font powerFont = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
                g.DrawString(((int)power).ToString(), powerFont, textBrush, indX + 64, indY);
        }

        // ?? timer tick ?????????????????????????????????????????????????????????????
        private static void EnforceMinVelocity(Ball ball)
        {
            const double minMag = 1.5;
            double mag = Math.Sqrt(ball.VelocityX * ball.VelocityX +
                                   ball.VelocityY * ball.VelocityY);
            if (mag > 0.01 && mag < minMag)
            {
                double scale   = minMag / mag;
                ball.VelocityX *= scale;
                ball.VelocityY *= scale;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_paused) return;

            // Move every ball and handle wall bounces (no friction — energy is conserved).
            foreach (Ball ball in _balls)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;

                // Bounce off felt edges
                if (ball.X < FeltLeft)
                {
                    ball.X         = FeltLeft;
                    ball.VelocityX = -ball.VelocityX;
                    ball.BounceCount++;
                }
                else if (ball.X + ball.Size > FeltRight)
                {
                    ball.X         = FeltRight - ball.Size;
                    ball.VelocityX = -ball.VelocityX;
                    ball.BounceCount++;
                }

                if (ball.Y < FeltTop)
                {
                    ball.Y         = FeltTop;
                    ball.VelocityY = -ball.VelocityY;
                    ball.BounceCount++;
                }
                else if (ball.Y + ball.Size > FeltBottom)
                {
                    ball.Y         = FeltBottom - ball.Size;
                    ball.VelocityY = -ball.VelocityY;
                    ball.BounceCount++;
                }
            }

            // Ball-to-ball collision — proper equal-mass elastic resolution
            for (int i = 0; i < _balls.Count; i++)
            {
                for (int j = i + 1; j < _balls.Count; j++)
                {
                    double cx1 = _balls[i].X + _balls[i].Size / 2.0;
                    double cy1 = _balls[i].Y + _balls[i].Size / 2.0;
                    double cx2 = _balls[j].X + _balls[j].Size / 2.0;
                    double cy2 = _balls[j].Y + _balls[j].Size / 2.0;

                    double dx       = cx2 - cx1;
                    double dy       = cy2 - cy1;
                    double distance = Math.Sqrt(dx * dx + dy * dy);
                    double minDist  = (_balls[i].Size + _balls[j].Size) / 2.0;

                    if (distance < minDist && distance > 0.0001)
                    {
                        // Normalize collision axis
                        double nx = dx / distance;
                        double ny = dy / distance;

                        // Separate overlapping balls
                        double overlap = (minDist - distance) / 2.0;
                        _balls[i].X -= nx * overlap;
                        _balls[i].Y -= ny * overlap;
                        _balls[j].X += nx * overlap;
                        _balls[j].Y += ny * overlap;

                        // Relative velocity along collision normal (i relative to j)
                        double dvx = _balls[i].VelocityX - _balls[j].VelocityX;
                        double dvy = _balls[i].VelocityY - _balls[j].VelocityY;
                        double dot = dvx * nx + dvy * ny;

                        // Only resolve if balls are approaching
                        if (dot <= 0) continue;

                        // Exchange velocity components along normal (equal mass elastic)
                        _balls[i].VelocityX -= dot * nx;
                        _balls[i].VelocityY -= dot * ny;
                        _balls[j].VelocityX += dot * nx;
                        _balls[j].VelocityY += dot * ny;

                        // Ensure any ball that received velocity doesn't stall below minimum
                        EnforceMinVelocity(_balls[i]);
                        EnforceMinVelocity(_balls[j]);
                    }
                }
            }

            totalBouncesLabel.Text = "Bounces: " + _balls.Sum(b => b.BounceCount).ToString();
            Invalidate();
        }

        // ?? mouse handlers ?????????????????????????????????????????????????????????
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_aiming)
            {
                _mousePos = e.Location;
                Invalidate();
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _cueBall == null || _paused) return;

            double cueCX = _cueBall.X + _cueBall.Size / 2.0;
            double cueCY = _cueBall.Y + _cueBall.Size / 2.0;
            double dx    = e.X - cueCX;
            double dy    = e.Y - cueCY;

            if (Math.Sqrt(dx * dx + dy * dy) < 60)
            {
                _aiming   = true;
                _mousePos = e.Location;
                Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !_aiming || _cueBall == null) return;

            double cueCX = _cueBall.X + _cueBall.Size / 2.0;
            double cueCY = _cueBall.Y + _cueBall.Size / 2.0;
            double dx    = cueCX - _mousePos.X;   // direction: mouse ? ball = shot direction
            double dy    = cueCY - _mousePos.Y;
            double dist  = Math.Sqrt(dx * dx + dy * dy);

            if (dist > 0)
            {
                double power       = Math.Min(300.0, Math.Max(50.0, dist)) / 30.0;
                _cueBall.VelocityX = (dx / dist) * power;
                _cueBall.VelocityY = (dy / dist) * power;
            }

            _aiming = false;
            Invalidate();
        }

        // ?? toolbar event handlers ?????????????????????????????????????????????????
        private void SpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            _speed           = speedTrackBar.Value;
            lblSpeedVal.Text = _speed.ToString();
            timer1.Interval  = Math.Max(5, 30 - speedTrackBar.Value);
        }

        private void BallTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Ball count is fixed in billiard mode; handler kept for wiring compatibility.
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            _paused          = !_paused;
            timer1.Enabled   = !_paused;
            pauseButton.Text = _paused ? "\u25B6 Resume" : "\u23F8 Pause";
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            _paused          = false;
            timer1.Enabled   = true;
            pauseButton.Text = "\u23F8 Pause";
            InitializeBalls();
            totalBouncesLabel.Text = "Bounces: 0";
        }
    }
}
