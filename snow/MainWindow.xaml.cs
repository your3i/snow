using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Timers;

namespace snow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Snowflake[] snowflakes;
        private int count = 250;

        public int Count
        {
            get { return count; }
            set { count = value;}
        }

        protected partial class Snowflake {
            static Random rnd;

            public double x = 0;
            public double y = 0;
            public double vx = 0;
            public double vy = 0;
            public double r = 0;
            public double o = 0;

            public void Reset(double width, double height) {
                if (null == rnd) {
                    rnd = new Random();
                }

                this.x = rnd.NextDouble() * width;
                this.y = rnd.NextDouble() * -height;
                this.vy = 1 + rnd.NextDouble() * 3;
                this.vx = 0.5 - rnd.NextDouble();
                this.r = 1 + rnd.NextDouble() * 2;
                this.o = 0.5 + rnd.NextDouble() * 0.5;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        System.Windows.Forms.NotifyIcon trayIcon;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //trayIcon = new System.Windows.Forms.NotifyIcon(new System.ComponentModel.Container());

            //trayIcon.Icon = new System.Drawing.Icon(GetType(), "icon/icon.ico");
            //trayIcon.Visible = true;

            int tempWidth = 0;
            int tempHeight = 0;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                tempWidth += screen.Bounds.Width;
                tempHeight += screen.Bounds.Height;
            }

            this.Width = tempWidth;
            this.Height = tempHeight;
            this.Top = 0;
            this.Left = 0;
            this.Background = null;

            this.makeSnowflakes();
            CompositionTarget.Rendering += update;
        }

        private void makeSnowflakes() {
            Snowflake snowflake = null;

            snowflakes = new Snowflake[count];
            for (int i = 0; i < count; i++) {
                snowflake = new Snowflake();
                snowflake.Reset(sky.ActualWidth, sky.ActualHeight);
                snowflakes[i] = snowflake;
            }
        }

        private void update(object sender, EventArgs e)
        {
            SolidColorBrush colorBrush = null;
            Snowflake snowflake = null;
            Ellipse snowEllipse = null;

            sky.Children.Clear();

            for (int i = 0; i < count; i++) {
                snowflake = snowflakes[i];
                snowflake.x += snowflake.vx;
                snowflake.y += snowflake.vy;

                snowEllipse = new Ellipse();
                snowEllipse.Width = 2 * snowflake.r;
                snowEllipse.Height = 2 * snowflake.r;

                sky.Children.Add(snowEllipse);
                Canvas.SetLeft(snowEllipse, snowflake.x);
                Canvas.SetTop(snowEllipse, snowflake.y);

                colorBrush = new SolidColorBrush();
                colorBrush.Color = Color.FromArgb((byte)(snowflake.o * 255), 255, 255, 255);
                snowEllipse.Fill = colorBrush;
                
                if (snowflake.y > sky.ActualHeight)
                {
                    snowflake.Reset(sky.ActualWidth, sky.ActualHeight);
                }
            }
        }
    }
}
