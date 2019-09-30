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

        public double Width { get; set; } = 800;
        public double Height { get; set; } = 500;

        public Scene()
        {
            circles = new List<Circle>
            {
                new Circle { Location = new Point(100, 100), Radius = 100, Velocity = new Vector(-5, 3), },
                new Circle { Location = new Point(400, 400), Radius = 50, Velocity = new Vector(5, -2), }
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
                        // Calculate new velocity
                    }
                }
            }
        }

        private void ForceInsideBounds()
        {
            foreach (var circle in circles)
            {
                if (circle.Radius + circle.Location.X > Width || circle.Location.X - circle.Radius < 0)
                {
                    circle.Velocity = new Vector(-circle.Velocity.X, circle.Velocity.Y);
                }

                if (circle.Radius + circle.Location.Y > Height || circle.Location.Y - circle.Radius < 0)
                {
                    circle.Velocity = new Vector(circle.Velocity.X, -circle.Velocity.Y);
                }
            }
        }

        private static Collision Collide(Circle circle, Circle other)
        {
            var locationsDiff = circle.Location - other.Location;
            
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
    }
}