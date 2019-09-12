using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    [TemplatePart(Name = "PART_Rim", Type = typeof(Path))]
    public class CircularMenuItem: HeaderedItemsControl
    {
        static CircularMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularMenuItem), new FrameworkPropertyMetadata(typeof(CircularMenuItem)));
        }

        public double ArcWidth
        {
            get { return (double)GetValue(ArcWidthProperty); }
            set { SetValue(ArcWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArcWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArcWidthProperty =
            DependencyProperty.Register("ArcWidth", typeof(double), typeof(CircularMenuItem), new PropertyMetadata(0d));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._contentOutBorder = GetTemplateChild("PART_ContentOutBorder") as Border;
            this._contentInBorder = GetTemplateChild("PART_ContentInBorder") as Border;
            this._partRim = GetTemplateChild("PART_Rim") as Path;
            _parentControl = ItemsControlFromItemContainer(this);
            _index =  _parentControl.Items.IndexOf(this);
            _count = _parentControl.Items.Count;
            if(this._partRim != null)
            {
                double r = Math.Min(this._parentControl.ActualHeight, this._parentControl.ActualWidth) / 2;
                Paint(r - _overflow - iWidth, r - _overflow, 6, _overflow, true);
            }
        }

        int _index = 0;
        int _count = 0;
        double _overflow = 10d;
        ItemsControl _parentControl = null;
        Path _partRim = null;
        Border _contentOutBorder = null;
        Border _contentInBorder = null;
        double iWidth = 50;

        void Paint(double inRadius, double outRadius, int count, double overflow, bool AutoRotate)
        {
            double iDiameter = inRadius * 2;
            double oDiameter = outRadius * 2;
            Vector iPoint = new Vector(iWidth, iWidth);

            double transformY = (outRadius - inRadius) / 2 + inRadius; //内容在Y轴偏移量
            double angleUnit = 360 / count; //单位角度，每份占用角度
            double radians = (Math.PI / 180) * Math.Round(360.0 / _count); //弧度

            //绘制圆弧
            {
                Point ip1 = GetPoint(inRadius, count, _index, overflow, radians) + iPoint;
                Point ip2 = GetPoint(inRadius, count, _index + 1, overflow, radians) + iPoint;
                Point op1 = GetPoint(outRadius, count, _index, overflow, radians);
                Point op2 = GetPoint(outRadius, count, _index + 1 >= count ? 0 : _index + 1, overflow, radians);

                Path path = new Path
                {
                    Width = overflow * 2 + oDiameter,
                    Height = overflow * 2 + oDiameter,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.None,
                    Fill = new SolidColorBrush { Color = Colors.White, Opacity = 0.1 },
                    Stroke = Brushes.White,
                    StrokeThickness = 4d,
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


                //内容
                _contentOutBorder.RenderTransform = new TransformGroup()
                    {
                        Children = new TransformCollection(
                            new Transform[]
                            {
                                new TranslateTransform(0, transformY),
                                new RotateTransform(angleUnit * (0.5 + _index))
                            }
                        ),
                    };
                _contentInBorder.RenderTransform = AutoRotate ? null : new RotateTransform(0 - (angleUnit * (0.5 + _index)));
            }
        }

            Point GetPoint(double r, int count, int index, double overflow, double radians)
        {
            double i = index;
            double x = r + r * Math.Sin(radians * i);
            double y = r + r * Math.Cos(radians * i);
            return new Point(x + overflow, y + overflow);
        }
    }
}
