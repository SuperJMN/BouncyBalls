using System.Windows;

namespace Project1
{
    class Collision
    {
        public Circle A { get; set; }
        public Circle B { get; set; }
        public double Penetration { get; set; }
        public Vector Normal { get; set; }
    };
}