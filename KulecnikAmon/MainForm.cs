/*
 * Vytvoøeno aplikací SharpDevelop.
 * Uživatel: amonv
 * Datum: 11.11.2022
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KulecnikAmon
{
    public class MainForm : Form
    {
        // ?? constants ?????????????????????????????????????????????????????????????
        private const int    MinSpeed    = 1;
        private const int    MaxSpeed    = 20;
        private const double MinVelocity = 1.0;
        private const double MaxVelocity = 4.0;

        private static readonly Color[] BallPalette =
        {
            Color.Red, Color.DodgerBlue, Color.Lime, Color.Orange,
            Color.MediumPurple, Color.Cyan, Color.HotPink, Color.Gold
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
        private List<Ball> _balls  = new List<Ball>();
        private int        _speed  = 5;
        private bool       _paused = false;
        private Random     _rng    = new Random();

        // ?? toolbar controls ???????????????????????????????????????????????????????
        private Label    lblSpeed;
        private TrackBar speedTrackBar;
        private Label    lblSpeedVal;
        private Label    lblBalls;
        private TrackBar ballTrackBar;
        private Label    lblBallCount;
        private Button   pauseButton;
        private Button   resetButton;
        private Label    totalBouncesLabel;

        // ?? constructor ????????????????????????????????????????????????????????????
        public MainForm()
        {
            components = new Container();

            // Timer
            timer1          = new Timer(components);
            timer1.Interval = 10;
            timer1.Tick    += Timer_Tick;
            timer1.Enabled  = true;

            // Form properties
            this.Text             = "Bouncing Ball Simulator";
            this.BackColor        = Color.FromArgb(30, 30, 30);
            this.ClientSize       = new System.Drawing.Size(1034, 537);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode    = AutoScaleMode.Font;
            this.Paint           += MainForm_Paint;

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

            // ?? Balls label ???????????????????????????????????????????????????????
            lblBalls           = new Label();
            lblBalls.Text      = "Balls:";
            lblBalls.ForeColor = Color.White;
            lblBalls.Location  = new System.Drawing.Point(290, 12);
            lblBalls.AutoSize  = true;

            // ?? Ball count trackbar ???????????????????????????????????????????????
            ballTrackBar               = new TrackBar();
            ballTrackBar.Minimum       = 1;
            ballTrackBar.Maximum       = 10;
            ballTrackBar.Value         = 5;
            ballTrackBar.Width         = 120;
            ballTrackBar.Height        = 30;
            ballTrackBar.Location      = new System.Drawing.Point(335, 8);
            ballTrackBar.TickFrequency = 1;
            ballTrackBar.ValueChanged += BallTrackBar_ValueChanged;

            // ?? Ball count value label ????????????????????????????????????????????
            lblBallCount           = new Label();
            lblBallCount.Text      = "5";
            lblBallCount.ForeColor = Color.White;
            lblBallCount.Width     = 20;
            lblBallCount.Location  = new System.Drawing.Point(460, 12);

            // ?? Pause button ??????????????????????????????????????????????????????
            pauseButton           = new Button();
            pauseButton.Text      = "\u23F8 Pause";
            pauseButton.Width     = 90;
            pauseButton.Height    = 28;
            pauseButton.Location  = new System.Drawing.Point(495, 9);
            pauseButton.FlatStyle = FlatStyle.Flat;
            pauseButton.BackColor = Color.FromArgb(60, 60, 60);
            pauseButton.ForeColor = Color.White;
            pauseButton.Click    += PauseButton_Click;

            // ?? Reset button ??????????????????????????????????????????????????????
            resetButton           = new Button();
            resetButton.Text      = "\u21BA Reset";
            resetButton.Width     = 80;
            resetButton.Height    = 28;
            resetButton.Location  = new System.Drawing.Point(592, 9);
            resetButton.FlatStyle = FlatStyle.Flat;
            resetButton.BackColor = Color.FromArgb(60, 60, 60);
            resetButton.ForeColor = Color.White;
            resetButton.Click    += ResetButton_Click;

            // ?? Total bounces label ???????????????????????????????????????????????
            totalBouncesLabel           = new Label();
            totalBouncesLabel.Text      = "Bounces: 0";
            totalBouncesLabel.ForeColor = Color.White;
            totalBouncesLabel.Width     = 130;
            totalBouncesLabel.Location  = new System.Drawing.Point(690, 12);
            totalBouncesLabel.AutoSize  = false;

            this.Controls.Add(lblSpeed);
            this.Controls.Add(speedTrackBar);
            this.Controls.Add(lblSpeedVal);
            this.Controls.Add(lblBalls);
            this.Controls.Add(ballTrackBar);
            this.Controls.Add(lblBallCount);
            this.Controls.Add(pauseButton);
            this.Controls.Add(resetButton);
            this.Controls.Add(totalBouncesLabel);

            InitializeBalls(5);
        }

        // ?? ball initialisation ????????????????????????????????????????????????????
        private void InitializeBalls(int count)
        {
            _balls.Clear();

            int areaW = ClientSize.Width  > 100 ? ClientSize.Width  : 800;
            int areaH = ClientSize.Height > 100 ? ClientSize.Height : 537;

            for (int i = 0; i < count; i++)
            {
                int size = _rng.Next(20, 56);

                double vx = MinVelocity + _rng.NextDouble() * (MaxVelocity - MinVelocity);
                double vy = MinVelocity + _rng.NextDouble() * (MaxVelocity - MinVelocity);
                if (_rng.Next(2) == 0) vx = -vx;
                if (_rng.Next(2) == 0) vy = -vy;

                _balls.Add(new Ball
                {
                    X           = _rng.Next(0, Math.Max(1, areaW - size)),
                    Y           = _rng.Next(45, Math.Max(46, areaH - size)),
                    VelocityX   = vx,
                    VelocityY   = vy,
                    Size        = size,
                    BallColor   = BallPalette[i % BallPalette.Length],
                    BounceCount = 0
                });
            }
        }

        // ?? painting ??????????????????????????????????????????????????????????????
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(30, 30, 30));

            foreach (Ball ball in _balls)
                DrawBall(e.Graphics, ball);
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
        }

        // ?? timer tick ?????????????????????????????????????????????????????????????
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_paused) return;

            // Move every ball and handle wall bounces.
            foreach (Ball ball in _balls)
            {
                ball.X += ball.VelocityX * _speed * 0.25;
                ball.Y += ball.VelocityY * _speed * 0.25;

                if (ball.X < 0)
                {
                    ball.X         = 0;
                    ball.VelocityX = -ball.VelocityX;
                    ball.BounceCount++;
                }
                else if (ball.X + ball.Size > ClientSize.Width)
                {
                    ball.X         = ClientSize.Width - ball.Size;
                    ball.VelocityX = -ball.VelocityX;
                    ball.BounceCount++;
                }

                if (ball.Y < 45)
                {
                    ball.Y         = 45;
                    ball.VelocityY = -ball.VelocityY;
                    ball.BounceCount++;
                }
                else if (ball.Y + ball.Size > ClientSize.Height)
                {
                    ball.Y         = ClientSize.Height - ball.Size;
                    ball.VelocityY = -ball.VelocityY;
                    ball.BounceCount++;
                }
            }

            // Ball-to-ball collision detection (elastic approximation: swap velocities).
            for (int i = 0; i < _balls.Count - 1; i++)
            {
                for (int j = i + 1; j < _balls.Count; j++)
                {
                    Ball a = _balls[i];
                    Ball b = _balls[j];

                    double ax   = a.X + a.Size / 2.0;
                    double ay   = a.Y + a.Size / 2.0;
                    double bx   = b.X + b.Size / 2.0;
                    double by   = b.Y + b.Size / 2.0;
                    double dist = Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
                    double minD = (a.Size + b.Size) / 2.0;

                    if (dist < minD && dist > 0)
                    {
                        double tmpVX = a.VelocityX;
                        double tmpVY = a.VelocityY;
                        a.VelocityX  = b.VelocityX;
                        a.VelocityY  = b.VelocityY;
                        b.VelocityX  = tmpVX;
                        b.VelocityY  = tmpVY;
                    }
                }
            }

            totalBouncesLabel.Text = "Bounces: " + _balls.Sum(b => b.BounceCount).ToString();
            Invalidate();
        }

        // ?? toolbar event handlers ?????????????????????????????????????????????????
        private void SpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            _speed           = speedTrackBar.Value;
            lblSpeedVal.Text = _speed.ToString();
        }

        private void BallTrackBar_ValueChanged(object sender, EventArgs e)
        {
            InitializeBalls(ballTrackBar.Value);
            lblBallCount.Text = ballTrackBar.Value.ToString();
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
            InitializeBalls(ballTrackBar.Value);
        }
    }
}
