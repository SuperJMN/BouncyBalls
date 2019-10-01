using System;
using System.Windows;

namespace Project1
{
    public class Circle : PhysicalObject
    {
        public Circle(double radius)
        {
            Radius = radius;
            Mass = Math.Pow(radius, 3) * 4 / 3 * Math.PI;
        }

        public double Radius { get; }

        public Point Location { get; set; }
    }
}