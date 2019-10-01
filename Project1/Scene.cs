using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Project1
{
    public class Scene
    {
        readonly List<Circle> circles;
        Random random = new Random((int)DateTime.Now.Ticks);

        public double Width { get; set; } = 900;
        public double Height { get; set; } = 800;

        public Scene()
        {
            circles = new List<Circle>
            {
                new Circle(30) { Location = new Point(3, 100), Velocity = new Vector(-15, 8), Restitution = 0.9},
                new Circle(12) { Location = new Point(77, 6), Velocity = new Vector(25, -15),  Restitution = 0.9},
                new Circle(15) { Location = new Point(77, 6), Velocity = new Vector(65, -25),  Restitution = 0.9},
                new Circle(23) { Location = new Point(66, 77), Velocity = new Vector(32, -35),  Restitution = 0.9},
                new Circle(55) { Location = new Point(66, 44), Velocity = new Vector(27, -5),  Restitution = 0.9},
            };
        }

        public void Update()
        {
            MoveCircles();
            ForceInsideBounds();
            CheckCollisions();
        }

        private void MoveCircles()
        {
            foreach (var circle in circles)
            {
                circle.Location = new Point(circle.Location.X + circle.Velocity.X,
                    circle.Location.Y + circle.Velocity.Y);
            }
        }

        private void CheckCollisions()
        {
            foreach (var circle in circles)
            {
                var others = circles.Except(new[] {circle});

                foreach (var other in others)
                {
                    var collision = Collide(circle, other);
                    if (collision.Penetration > 0)
                    {
                        ResolveCollision(circle, other, collision.Normal);
                    }
                }
            }
        }

        private void ForceInsideBounds()
        {
            foreach (var circle in circles)
            {
                if (circle.Radius + circle.Location.X > Width && circle.Velocity.X > 0 || circle.Location.X - circle.Radius < 0 && circle.Velocity.X < 0)
                {
                    circle.Velocity = new Vector(-circle.Velocity.X, circle.Velocity.Y);
                }

                if (circle.Radius + circle.Location.Y > Height && circle.Velocity.Y > 0 || circle.Location.Y - circle.Radius < 0 && circle.Velocity.Y < 0)
                {
                    circle.Velocity = new Vector(circle.Velocity.X, -circle.Velocity.Y);
                }
            }
        }

        private static Collision Collide(Circle circle, Circle other)
        {
            var locationsDiff = other.Location - circle.Location;
            
            var distance = locationsDiff.Length;
            var penetration = circle.Radius + other.Radius - distance;
            if (penetration < 0)
            {
                return new Collision { Penetration = penetration};
            }

            var collision = new Collision
            {
                Penetration = penetration, 
                Normal = locationsDiff / distance
            };

            return collision;
        }

        public void Draw(DrawingContext drawingContext)
        {
            foreach (var circle in circles)
            {
                drawingContext.DrawEllipse(Brushes.Red, new Pen(Brushes.DarkRed, 2), circle.Location, circle.Radius, circle.Radius);
            }
        }

        void ResolveCollision(PhysicalObject a, PhysicalObject b, Vector normal)
        {
            // Calculate relative velocity
            var rv = b.Velocity - a.Velocity;

            // Calculate relative velocity in terms of the normal direction
            var velAlongNormal = rv * normal;

            // Do not resolve if velocities are separating
            if (velAlongNormal > 0)
                return;

            // Calculate restitution
            var e = Math.Min(a.Restitution, b.Restitution);

            // Calculate impulse scalar
            var j = -(1 + e) * velAlongNormal;
            j /= 1 / a.Mass + 1 / b.Mass;

            // Apply impulse
            var impulse = j * normal;
            a.Velocity -= 1 / a.Mass * impulse;
            b.Velocity += 1 / b.Mass * impulse;
        }
    }
}