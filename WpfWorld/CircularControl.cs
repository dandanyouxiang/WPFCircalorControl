using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace WpfWorld
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfWorld"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfWorld;assembly=WpfWorld"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CircularControl/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_Circular", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ItemsPresenter", Type = typeof(CircularPanel))]
    //[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CircularControlItem))]
    public class CircularControl : ListBox
    {
        static CircularControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularControl), new FrameworkPropertyMetadata(typeof(CircularControl)));
        }


        public SweepDirection SweepDirection
        {
            get { return (SweepDirection)GetValue(SweepDirectionProperty); }
            set { SetValue(SweepDirectionProperty, value); }
        }

        public static readonly DependencyProperty SweepDirectionProperty =
            DependencyProperty.Register("SweepDirection", typeof(SweepDirection), typeof(CircularControl), new FrameworkPropertyMetadata(SweepDirection.Counterclockwise, FrameworkPropertyMetadataOptions.AffectsMeasure));



        public new double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickniss.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(CircularControl),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public new Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(CircularControl),
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(CircularControl),
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(CircularControl), 
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        public Brush MouseOverFill
        {
            get { return (Brush)GetValue(MouseOverFillProperty); }
            set { SetValue(MouseOverFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverFillProperty =
            DependencyProperty.Register("MouseOverFill", typeof(Brush), typeof(CircularControl),
                new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        public Orientation Orientation
        {
            set { SetValue(OrientationProperty, value); }
            get { return (Orientation)GetValue(OrientationProperty); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CircularControl), 
                new FrameworkPropertyMetadata(Orientation.Default, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        public new double Padding
        {
            get { return (double)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Padding.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(double), typeof(CircularControl),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public override void OnApplyTemplate()
        {
            _circularGrid = GetTemplateChild("PART_Circular") as Grid;
            _circularPanel = GetTemplateChild("PART_ItemsPresenter") as CircularPanel;
            base.OnApplyTemplate();
        }

        CircularPanel _circularPanel = null;
        Grid _circularGrid = null;

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _overflow = BorderThickness;
            if(_circularPanel != null)
                _circularPanel.Margin = new Thickness(Padding + _overflow * 2);
            Paint(arrangeBounds);
            return base.ArrangeOverride(arrangeBounds);
        }

        #region Path
        double _overflow = 10d;
        double _iWidth = 50;
        double maxSize = 0d;

        void Paint(Size size)
        {
            if (this._circularGrid != null)
            {
                foreach (var child in Items)
                {
                    UIElement fe = child as UIElement;
                    if (fe != null)
                    {
                        maxSize = Math.Max(maxSize, Math.Max(fe.DesiredSize.Width, fe.DesiredSize.Height));
                    }
                }

                _iWidth = Padding * 2 + maxSize + _overflow * 1.5;

                double r = Math.Min(size.Width, size.Height) / 2;
                Paint(r - _overflow - _iWidth, r - _overflow, this.Items.Count, _overflow, true);
            }
        }

        /// <summary>
        /// 半径溢出10像素
        /// </summary>
        /// <param name="inRadius">内圆半径</param>
        /// <param name="outRadius">外圆半径</param>
        /// <param name="count">数量</param>
        /// <param name="AutoRotate">内容是否随圆形旋转</param>
        void Paint(double inRadius, double outRadius, int count, double overflow, bool AutoRotate)
        {
            this._circularGrid.Children.Clear();
            //Path p = this.red1;
            double iDiameter = inRadius * 2;
            double oDiameter = outRadius * 2;
            Vector iPoint = new Vector(_iWidth, _iWidth);
            List<Point> inPoints = GetPoints(inRadius, count, overflow);
            List<Point> outPoints = GetPoints(outRadius, count, overflow);

            double transformY = (outRadius - inRadius) / 2 + inRadius; //内容在Y轴偏移量
            double angleUnit = 360 / count; //单位角度，每份占用角度
            
            //绘制圆弧
            for (int i = 0; i < count; i++)
            {
                FrameworkElement current = Items[i] as FrameworkElement;
                //if (i > 0) break;

                Point ip1 = inPoints[i] + iPoint;
                Point ip2 = inPoints[i + 1 >= count ? 0 : i + 1] + iPoint;
                Point op1 = outPoints[i];
                Point op2 = outPoints[i + 1 >= count ? 0 : i + 1];
                

                Path path = new Path
                {
                    Width = overflow * 2 + oDiameter,
                    Height = overflow * 2 + oDiameter,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.None,
                    Fill = Fill,
                    Stroke = BorderBrush,
                    StrokeThickness = BorderThickness,
                    Data = new PathGeometry(
                        new PathFigure[]
                        {
                            new PathFigure(
                                start: op1,
                                segments: new PathSegment[]     //op1 -> op2 -> ip2 -> ip1
                                {
                                    new ArcSegment(op2, new Size(outRadius, outRadius), 0, false, SweepDirection == SweepDirection.Clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true),
                                    new LineSegment(ip2, false),
                                    new ArcSegment(ip1, new Size(inRadius, inRadius), 0, false, SweepDirection == SweepDirection.Clockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, false),
                                    new LineSegment(op1, false),
                                },
                                closed: true
                                ),
                        }
                   ),
                };

                path.MouseEnter += Btn_MouseEnter;
                path.MouseLeave += Btn_MouseLeave;
                path.LostFocus += Btn_LostFocus;
                path.MouseLeftButtonUp += Path_MouseLeftButtonUp;

                this._circularGrid.Children.Add(path);
            }
        }

        private void Path_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            for (int i = 0; i < _circularGrid.Children.Count; i++)
            {
                if(p == _circularGrid.Children[i])
                {
                    base.SelectedValue = Items[i];
                    break;
                }
            }
        }

        private void Btn_LostFocus(object sender, RoutedEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill = Fill;
            p.Stroke = BorderBrush;
            p.StrokeThickness = BorderThickness;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill = Fill;
            p.Stroke = BorderBrush;
            p.StrokeThickness = BorderThickness;
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            Path p = (Path)sender;
            p.Fill = MouseOverFill;
            p.Stroke = MouseOverBorderBrush;
            p.StrokeThickness = BorderThickness + 2.0d;
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
            double i = 0;
            for (; i < count; i++)
            {
                double x = r + r * Math.Sin(radians * i - radians /2) * (SweepDirection == SweepDirection.Clockwise ? 1 : -1);
                double y = r - r * Math.Cos(radians * i - radians /2);
                points.Add(new Point(x + overflow, y + overflow));
            }
            return points;
        }
        #endregion
    }
}
