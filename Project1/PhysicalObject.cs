using System.Windows;

namespace Project1
{
    public class PhysicalObject
    {	
        public double Restitution { get; set; }
        public Vector Velocity { get; set; }
        public double Mass { get; protected set; }
    }
}