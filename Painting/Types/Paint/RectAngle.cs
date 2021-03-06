﻿using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Rectangle : Shape
    {
        public Rectangle(float width, Colour lineColour, Coordinate position, Coordinate unturnedSize, Colour mainColour, float rotation)
            : base(position, unturnedSize, mainColour, rotation)
        {
            Width = width;
            LineColour = lineColour;
        }

        public float Width { get; set; }
        public Colour LineColour { get; set; }

        public override bool Equals(object obj) => obj is Rectangle && Equals((Rectangle) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ (LineColour?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Rotation.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(Rectangle other)
            =>
            (other != null) && base.Equals(other) && (Math.Abs(Width - other.Width) < 0.001) &&
            Equals(LineColour, other.LineColour) && Math.Abs(Rotation - other.Rotation) < 0.001;

        public void Paint(Graphics p, Coordinate rotationCenterPointFromPosition)
        {
            var trans = p.Transform.Clone();
            if (Rotation != 0)
            {
                p.TranslateTransform(rotationCenterPointFromPosition.X, rotationCenterPointFromPosition.Y);
                p.RotateTransform(Rotation);
                p.TranslateTransform(-rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y);
            }
            if (MainColour.Visible)
                p.FillRectangle(new SolidBrush(MainColour.Color), Position.X, Position.Y, UnturnedSize.X, UnturnedSize.Y);
            if (LineColour.Visible)
                p.DrawRectangle(new Pen(LineColour.Color, Width), Position.X, Position.Y, UnturnedSize.X, UnturnedSize.Y);
            p.Transform = trans;
        }
    }
}