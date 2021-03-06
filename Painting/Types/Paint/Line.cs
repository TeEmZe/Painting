﻿using System;
using System.Drawing;
using Painting.Util;

namespace Painting.Types.Paint
{
    public class Line : Shape
    {
        private Coordinate _end;

        private Coordinate _position;
        private float _rotation;
        private Coordinate _size;

        public Line(Coordinate start, Coordinate end, Colour lineColour, float width, float rotation=0)
            : base(start, end.Sub(start), lineColour, rotation)
        {
            End = end;
            Width = width;
        }

        public Coordinate End
        {
            get { return _end; }
            set
            {
                _end = value;
                if ((Position == null) || (value == null)) return;
                if (!UnturnedSize.Equals(End.Sub(Position)))
                    UnturnedSize = End.Sub(Position);
                if (UnturnedSize == null)
                    return;
                var n = (float) Physomatik.ToDegree(Math.Asin(UnturnedSize.Y/Length()));
                if (UnturnedSize.X < 0)
                    n = 180 - n;
                if (Math.Abs(Rotation - n) > 0.001)
                    Rotation = n;
            }
        }

        public override float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                if ((Position == null) || (End == null) || (UnturnedSize == null)) return;
                var n = new Coordinate((float) Math.Cos(Physomatik.ToRadian(value))*Length(),
                    (float) Math.Sin(Physomatik.ToRadian(value))*Length()).Add(Position);
                if (!End.Equals(n))
                    End = n;
            }
        }

        public override Coordinate Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if ((End != null) && (Position != null) && !UnturnedSize.Equals(End.Sub(Position)))
                    End = Position.Add(UnturnedSize);
            }
        }

        public float Length() => UnturnedSize.Pyth();

        public override Coordinate UnturnedSize
        {
            get { return _size; }
            set
            {
                _size = value;
                if ((UnturnedSize != null) && (End != null) && !End.Equals(Position.Add(UnturnedSize)))
                    End = Position.Add(UnturnedSize);
            }
        }

        public float Width { get; set; }

        public override bool Equals(object obj) => obj is Line && Equals((Line) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (End?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(Line other)
            =>
            (other != null) && Equals(End, other.End) && MainColour.Equals(other.MainColour) && Equals(UnturnedSize, other.UnturnedSize) &&
            (Math.Abs(Width - other.Width) < 0.001);

        public void Paint(Graphics p)
        {
            if (MainColour.Visible)
                p.DrawLine(new Pen(MainColour.Color, Width), Position.X, Position.Y, End.X, End.Y);
        }
    }
}