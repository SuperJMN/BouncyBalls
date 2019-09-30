using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;

namespace Project1
{
    public class MyWindow : Window
    {
        readonly Scene scene = new Scene();

        public MyWindow()
        {
            var updater = Observable
                .Interval(TimeSpan.FromMilliseconds(10))
                .ObserveOnDispatcher()
                .Subscribe(_ =>
                    {
                        scene.Update();
                        InvalidateVisual();
                    }
                );
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            scene.Draw(drawingContext);
        }
    }
}