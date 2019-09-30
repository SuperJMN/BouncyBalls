using System;
using System.Windows;

namespace Project1
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            var win = new MyWindow {Background = null};

            app.Run(win);
        }
    }
}