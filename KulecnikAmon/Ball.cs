/*
 * Vytvoøeno aplikací SharpDevelop.
 * Uživatel: amonv
 */
using System.Drawing;

namespace KulecnikAmon
{
    /// <summary>
    /// Represents a single bouncing ball.
    /// </summary>
    public class Ball
    {
        /// <summary>Top-left X position (pixels, sub-pixel precision).</summary>
        public double X { get; set; }

        /// <summary>Top-left Y position (pixels, sub-pixel precision).</summary>
        public double Y { get; set; }

        /// <summary>Horizontal velocity in pixels per tick (base, before speed multiplier).</summary>
        public double VelocityX { get; set; }

        /// <summary>Vertical velocity in pixels per tick (base, before speed multiplier).</summary>
        public double VelocityY { get; set; }

        /// <summary>Diameter in pixels (20–60).</summary>
        public int Size { get; set; }

        /// <summary>Fill colour of the ball.</summary>
        public Color BallColor { get; set; }

        /// <summary>Number of times this ball has bounced off a wall.</summary>
        public int BounceCount { get; set; }

        /// <summary>Display number shown on the ball (1-based index).</summary>
        public int Number { get; set; }

        /// <summary>Convenience: radius = Size / 2.0</summary>
        public double Radius { get { return Size / 2.0; } }

        /// <summary>Centre X coordinate.</summary>
        public double CenterX { get { return X + Radius; } }

        /// <summary>Centre Y coordinate.</summary>
        public double CenterY { get { return Y + Radius; } }
    }
}
