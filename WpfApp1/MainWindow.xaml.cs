using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfWorld;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double r = Math.Min(this.container.ActualHeight, this.container.ActualWidth) / 2;
            Paint(r - 60, r - 10, 6, true);
        }

        /// <summary>
        /// 半径溢出10像素
        /// </summary>
        /// <param name="inRadius">内圆半径</param>
        /// <param name="outRadius">外圆半径</param>
        /// <param name="count">数量</param>
        /// <param name="AutoRotate">内容是否随圆形旋转</param>
        void Paint(double inRadius, double outRadius, int count, bool AutoRotate)
        {
            //Path p = this.red1;
            double overflow = 20;  //偏移量
            double iDiameter = inRadius * 2;
            double oDiameter = outRadius * 2;
            double iWidth = outRadius - inRadius;
            Vector iPoint = new Vector(iWidth, iWidth);
            List<Point> inPoints = GetPoints(inRadius, count, overflow / 2);
            List<Point> outPoints = GetPoints(outRadius, count, overflow / 2);

            double transformY = (outRadius - inRadius) / 2 + inRadius; //内容在Y轴偏移量
            double angleUnit = 360 / count; //单位角度，每份占用角度

            //绘制圆弧
            for (int i = 0; i < count; i++)
            {
                //if (i > 0) break;

                Point ip1 = inPoints[i] + iPoint;
                Point ip2 = inPoints[i + 1 >= count ? 0 : i + 1] + iPoint;
                Point op1 = outPoints[i];
                Point op2 = outPoints[i + 1 >= count ? 0 : i + 1];

                Path path = new Path
                {
                    Width = overflow + oDiameter, Height = overflow + oDiameter,
                    HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment =  VerticalAlignment.Center, Stretch = Stretch.None,
                    Fill = new SolidColorBrush { Color = Colors.White, Opacity = 0.1}, Stroke = Brushes.White, StrokeThickness = 4d,
                    Data = new PathGeometry(
                        new PathFigure[]
                        {
                            new PathFigure(
                                start: op1,
                                segments: new PathSegment[]     //op1 -> op2 -> ip2 -> ip1
                                {
                                    new ArcSegment(op2, new Size(outRadius, outRadius), 0, false, SweepDirection.Clockwise, true),
                                    new LineSegment(ip2, false),
                                    new ArcSegment(ip1, new Size(inRadius, inRadius), 0, false, SweepDirection.Counterclockwise, false),
                                    new LineSegment(op1, false),
                                },
                                closed: true
                                ),
                        }
                   ),
                };

                Panel.SetZIndex(path, 0);

                Console.WriteLine(path.Data);
                
                path.MouseEnter += Btn_MouseEnter;
                path.MouseLeave += Btn_MouseLeave;
                path.LostFocus += Btn_LostFocus;


                //内容
                Border b = new Border
                {
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new TransformGroup()
                    {
                        Children = new TransformCollection(
                            new Transform[]
                            {
                                new TranslateTransform(0, transformY),
                                new RotateTransform(angleUnit * (0.5 + i))
                            }
                        ),
                    },
                    Child = new Border
                    {
                        Background = Brushes.Transparent,
                        RenderTransformOrigin = new Point(0.5, 0.5),
                        RenderTransform = AutoRotate ? null : new RotateTransform(0 - (angleUnit * (0.5 + i))),
                        Child = new TextBlock { Text = "菜单" + i.ToString() },
                    }
                };
                Panel.SetZIndex(b, 1);
                this.container.Children.Add(path);
                this.container.Children.Add(b);
            }
        }

        private void Btn_LostFocus(object sender, RoutedEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill.Opacity = 0.1;
            p.StrokeThickness = 4d;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill.Opacity = 0.1;
            p.StrokeThickness = 4d;
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill.Opacity = 0.5;
            p.StrokeThickness = 6d;
        }

        /// <summary>
        /// 将圆均分成指定数量并返回坐标点集合
        /// </summary>
        /// <param name="r">半径</param>
        /// <param name="count">数量</param>
        /// <param name="overflow">偏移量</param>
        /// <returns></returns>
        List<Point> GetPoints(double r, int count, double overflow = 0)
        {
            List<Point> points = new List<Point>();
            double radians = (Math.PI / 180) * Math.Round(360.0 / count); //弧度
            double i = count - 1;
            for (; i >= 0; i--)
            {
                double x = r + r * Math.Sin(radians * i);
                double y = r + r * Math.Cos(radians * i);
                points.Add(new Point(x + overflow, y + overflow));
            }
            return points;
        }

        private void CircularControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CircularControl c = (sender) as CircularControl;
            CircularControlItem ci = c.SelectedItem as CircularControlItem;
            TextBlock t = ci.Content as TextBlock;
            MessageBox.Show(t.Text);
        }
    }
}
