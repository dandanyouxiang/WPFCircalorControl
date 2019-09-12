using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfWorld
{
    public enum Orientation
    {
        Default,
        In,
        Out
    }

    public class CircularPanel:Panel
    {
        double angleEach;       // 角度
        Size sizeLargest;       // 最大孩子的尺寸  
        double radius;
        double maxnum;
        double minnum;

        // 圆的半径
        double outerEdgeFromCenter;
        double innerEdgeFromCenter;

        public double Padding
        {
            get { return (double)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Padding" /> property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty
            = DependencyProperty.Register("Padding", typeof(double), typeof(CircularPanel),
                new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        public bool ChildrenHitVisible
        {
            get { return (bool)GetValue(ChildrenHitVisibleProperty); }
            set { SetValue(ChildrenHitVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChildrenHitVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildrenHitVisibleProperty =
            DependencyProperty.Register("ChildrenHitVisible", typeof(bool), typeof(CircularPanel), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CircularPanel), new FrameworkPropertyMetadata(
                                                Orientation.Default,
                                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public SweepDirection SweepDirection
        {
            get { return (SweepDirection)GetValue(SweepDirectionProperty); }
            set { SetValue(SweepDirectionProperty, value); }
        }

        public static readonly DependencyProperty SweepDirectionProperty =
            DependencyProperty.Register("SweepDirection", typeof(SweepDirection), typeof(CircularPanel), new FrameworkPropertyMetadata(SweepDirection.Clockwise, FrameworkPropertyMetadataOptions.AffectsMeasure));



        protected override Size MeasureOverride(Size availableSize)
        {
            if (InternalChildren.Count == 0)
                return new Size(0, 0);

            angleEach = 360.0 / InternalChildren.Count;
            sizeLargest = new Size(0, 0);
            

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                sizeLargest.Width = Math.Max(sizeLargest.Width, child.DesiredSize.Width);
                sizeLargest.Height = Math.Max(sizeLargest.Height, child.DesiredSize.Height);
            }

            maxnum = Math.Max(sizeLargest.Width, sizeLargest.Height);
            minnum = Math.Min(sizeLargest.Width, sizeLargest.Height);

            innerEdgeFromCenter = maxnum / 2 / Math.Tan(Math.PI * angleEach / 2 / 180);
            outerEdgeFromCenter = Math.Sqrt(Math.Pow(innerEdgeFromCenter + minnum, 2) + Math.Pow(maxnum / 2, 2));

            if (!IsValid(availableSize.Width) || !IsValid(availableSize.Height))
                radius = outerEdgeFromCenter;
            else
                radius = Math.Min(availableSize.Width, availableSize.Height) / 2;// Math.Max(outerEdgeFromCenter, Math.Min(availableSize.Width, availableSize.Height) / 2);
            outerEdgeFromCenter = radius + Padding * 2;
            innerEdgeFromCenter = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(maxnum, 2)/4) - maxnum;
            return new Size(outerEdgeFromCenter * 2, outerEdgeFromCenter * 2);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point ptCenter = new Point(finalSize.Width / 2, finalSize.Height / 2);
            double translate = innerEdgeFromCenter + maxnum / 2 + Padding;
            double currentAngle = 0;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                UIElement child = InternalChildren[i];
                double x = 0;
                double y = 0;
                if(SweepDirection == SweepDirection.Clockwise)
                    x = ptCenter.X + translate * Math.Sin(Math.PI * currentAngle / 180) - maxnum / 2;
                else
                    x = ptCenter.X - translate * Math.Sin(Math.PI * currentAngle / 180) - maxnum / 2;
                y = ptCenter.Y - translate * Math.Cos(Math.PI * currentAngle / 180) - maxnum / 2;

                double w = Math.Max(0.0d, maxnum - Padding * 2);
                w = Math.Max(w, maxnum);
                Rect rect = new Rect(x, y, w,w);
                child.Arrange(rect);
                child.IsHitTestVisible = ChildrenHitVisible;
                SetRenderTransform(child, currentAngle);
                currentAngle += angleEach;
            }
            return finalSize;
        }

        void SetRenderTransform(UIElement child,double angle)
        {
            switch (Orientation)
            {
                case Orientation.Default: angle = 0d;
                    break;
                case Orientation.In:
                    angle = angle - 180;
                    break;
                case Orientation.Out:
                    break;
                default:
                    break;
            }
            if (SweepDirection == SweepDirection.Counterclockwise)
                angle = 360 - angle;
            child.RenderTransformOrigin = new Point(0.5, 0.5);
            child.RenderTransform = Transform.Identity;
            child.RenderTransform = new RotateTransform(angle);
        }

         bool IsValid(double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value))
                return false;
            return true;
        }


        // 显示饼图切线
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if(false)
            {
                Point ptCenter = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
                Pen pen = new Pen(SystemColors.WindowTextBrush, 1);
                pen.DashStyle = DashStyles.Dash;

                // 显示圆
                dc.DrawEllipse(null, pen, ptCenter, outerEdgeFromCenter, outerEdgeFromCenter);
                dc.DrawEllipse(null, pen, ptCenter, innerEdgeFromCenter, innerEdgeFromCenter);
            }
        }
    }
}
