using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bezier
{
    public class BezierDigitAnimator
    {      
        private Path digitCurvePath = new Path();
        private Path digitCurveVertexPath = new Path();
        private Path digitControlPointsVertexPath = new Path();
        private Path digitControlPointsLinesPath = new Path();

        private Settings settings = Settings.Instance;
        public BezierDigitAnimator(double pauseDuration, double animDuration, Panel host)
        {
            this.AnimationStartRatio = pauseDuration / (pauseDuration + animDuration);

            UpdateGeometry();

            if (host == null) throw new ArgumentNullException("Digit Host is null");

            host.Children.Add(digitCurvePath);
            host.Children.Add(digitControlPointsLinesPath);
            host.Children.Add(digitCurveVertexPath);
            host.Children.Add(digitControlPointsVertexPath);            

            Settings.Instance.PropertyChanged += Settings_PropertyChanged;
        }
        public double AnimationStartRatio; // ratio after which we start the animation

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateGeometry();
        }

        private void UpdateGeometry()
        {
            PenLineCap plc = PenLineCap.Round;

            digitCurvePath.Stroke = settings.DigitColor;
            digitCurvePath.StrokeThickness = settings.DigitStrokeThickness;
            digitCurvePath.StrokeStartLineCap = plc;
            digitCurvePath.StrokeEndLineCap = plc;

            if (settings.DrawControlLines)
            {
                SolidColorBrush strokeColor = settings.ControlLinesColor as SolidColorBrush;

                SolidColorBrush fillBrush = new SolidColorBrush(Colors.Beige);

                digitCurveVertexPath.Stroke = strokeColor;
                digitCurveVertexPath.Fill = fillBrush;
                digitCurveVertexPath.StrokeThickness = settings.ControlLinesStrokeThickness;
                digitCurveVertexPath.StrokeStartLineCap = plc;
                digitCurveVertexPath.StrokeEndLineCap = plc;

                digitControlPointsVertexPath.Stroke = strokeColor;
                digitControlPointsVertexPath.Fill = fillBrush;
                digitControlPointsVertexPath.StrokeThickness = settings.ControlLinesStrokeThickness;
                digitControlPointsVertexPath.StrokeStartLineCap = plc;
                digitControlPointsVertexPath.StrokeEndLineCap = plc;

                digitControlPointsLinesPath.Stroke = strokeColor;
                digitControlPointsLinesPath.StrokeThickness = settings.ControlLinesStrokeThickness;
                digitControlPointsLinesPath.StrokeStartLineCap = plc;
                digitControlPointsLinesPath.StrokeEndLineCap = plc;

                digitCurveVertexPath.Visibility = Visibility.Visible;
                digitControlPointsVertexPath.Visibility = Visibility.Visible;
                digitControlPointsLinesPath.Visibility = Visibility.Visible;
            }
            else
            {
                digitCurveVertexPath.Visibility = Visibility.Hidden;
                digitControlPointsVertexPath.Visibility = Visibility.Hidden;
                digitControlPointsLinesPath.Visibility = Visibility.Hidden;
            }
        }
        
        public void Update(int current, int next, double ratio)
        {
            double animationRatio = 0.0;
            if (ratio > AnimationStartRatio) { animationRatio = (ratio - AnimationStartRatio) / (1.0 - AnimationStartRatio); }
            if (settings.ContinualAnimation) { animationRatio = ratio; }
            if (ratio < 0.0) { animationRatio = 0.0; }
            if (ratio > 1.0) { animationRatio = 1.0; }
            if (settings.AnimationType == 1)
            { // quad
                animationRatio = Math.Pow(animationRatio, 2.0);
                ratio = Math.Pow(ratio, 2.0); // we don't need ratio any more
            }
            else if (settings.AnimationType == 2)
            { // cub
                animationRatio = animationRatio * Math.Pow(animationRatio, 2.0);
                ratio = ratio * Math.Pow(ratio, 2.0);
            }
            else if (settings.AnimationType == 3)
            { // sin
                animationRatio = 0.5 * (-Math.Cos(animationRatio * Math.PI) + 1.0);
                ratio = 0.5 * (-Math.Cos(ratio * Math.PI) + 1.0);
            }

            Digit digit = GetInterpolatedDigit(current, next, animationRatio);

            // Кривая
            MakeBezierDigitPathGeometry(digit);
            // Вершины кривой
            MakeBezierDigitCurveVertexGeometry(digit);
            // Вспомогательные линии
            MakeBezierDigitCotrolPointsVertexGeometry(digit);
            // Вершины вспомогательных линий
            MakeBezierDigitCotrolPointsLinesGeometry(digit);

            //UpdateGeometry();
        }
        private void MakeBezierDigitPathGeometry(Digit digit)
        {
            PathFigure digitFigure = new PathFigure();
            digitFigure.StartPoint = new Point(digit.VertexX, digit.VertexY);

            PathSegmentCollection digitPathSegmentCollection = new PathSegmentCollection();

            for (int controlsId = 0; controlsId < 4; controlsId++)
            {
                PointCollection digitPointCollection = new PointCollection();
                double[] control = digit.Controls[controlsId];
                for (int i = 0; i < 6; i = i + 2)
                {
                    digitPointCollection.Add(new Point(control[i], control[i + 1]));
                }

                PolyBezierSegment digitPathSegment = new PolyBezierSegment(digitPointCollection, true);

                digitPathSegmentCollection.Add(digitPathSegment);
            }

            digitFigure.Segments = digitPathSegmentCollection;
            
            PathFigureCollection digitPathFigureCollection = new PathFigureCollection();
            digitPathFigureCollection.Add(digitFigure);

            PathGeometry digitPathGeometry = new PathGeometry();
            digitPathGeometry.Figures = digitPathFigureCollection;

            digitCurvePath.Data = digitPathGeometry;
        }
        private void MakeBezierDigitCurveVertexGeometry(Digit digit)
        {
            GeometryGroup controlCurveVertexGeometryGroup = new GeometryGroup();
            controlCurveVertexGeometryGroup.FillRule = FillRule.Nonzero;

            double rectSize = settings.ControlCurveVertexSize;
            double rectRadius = rectSize / 5.0d;

            RectangleGeometry controlCurveVertexRect = new RectangleGeometry();
            for (int controlsId = 0; controlsId < 4; controlsId++)
            {
                double[] control = digit.Controls[controlsId];

                controlCurveVertexRect = new RectangleGeometry(new Rect(new Point(control[4] - rectSize / 2, control[5] - rectSize / 2),
                        new Size(rectSize, rectSize)), rectRadius, rectRadius);
                controlCurveVertexGeometryGroup.Children.Add(controlCurveVertexRect);

                if (controlsId == 0)
                {
                    controlCurveVertexRect = new RectangleGeometry(new Rect(new Point(digit.VertexX - rectSize / 2, digit.VertexY - rectSize / 2),
                            new Size(rectSize, rectSize)), rectRadius, rectRadius);
                    controlCurveVertexGeometryGroup.Children.Add(controlCurveVertexRect);
                }
            }
            digitCurveVertexPath.Data = controlCurveVertexGeometryGroup;
        }
        private void MakeBezierDigitCotrolPointsVertexGeometry(Digit digit)
        {
            GeometryGroup controlPointsVertexGeometryGroup = new GeometryGroup();
            controlPointsVertexGeometryGroup.FillRule = FillRule.Nonzero;

            double pointsRadius = settings.ControlCurveVertexSize / 3.0d;

            EllipseGeometry controlPointsVertexCircle = new EllipseGeometry();
            for (int controlsId = 0; controlsId < 4; controlsId++)
            {
                double[] control = digit.Controls[controlsId];

                controlPointsVertexCircle = new EllipseGeometry(new Point(control[2], control[3]), pointsRadius, pointsRadius);
                controlPointsVertexGeometryGroup.Children.Add(controlPointsVertexCircle);

                if (controlsId < 3)
                {
                    controlPointsVertexCircle = new EllipseGeometry(new Point(digit.Controls[controlsId + 1][0], digit.Controls[controlsId + 1][1]), pointsRadius, pointsRadius);
                    controlPointsVertexGeometryGroup.Children.Add(controlPointsVertexCircle);
                }

                if (controlsId == 0)
                {
                    controlPointsVertexCircle = new EllipseGeometry(new Point(control[0], control[1]), pointsRadius, pointsRadius);
                    controlPointsVertexGeometryGroup.Children.Add(controlPointsVertexCircle);
                }
            }
            digitControlPointsVertexPath.Data = controlPointsVertexGeometryGroup;
        }
        private void MakeBezierDigitCotrolPointsLinesGeometry(Digit digit)
        {
            GeometryGroup controlPointsLinesGeometryGroup = new GeometryGroup();
            controlPointsLinesGeometryGroup.FillRule = FillRule.Nonzero;

            LineGeometry controlPointsLine = new LineGeometry();
            for (int controlsId = 0; controlsId < 4; controlsId++)
            {
                double[] control = digit.Controls[controlsId];

                controlPointsLine = new LineGeometry(new Point(control[2], control[3]), new Point(control[4], control[5]));
                controlPointsLinesGeometryGroup.Children.Add(controlPointsLine);

                if (controlsId < 3)
                {
                    controlPointsLine = new LineGeometry(new Point(digit.Controls[controlsId + 1][0], digit.Controls[controlsId + 1][1]), new Point(control[4], control[5]));
                    controlPointsLinesGeometryGroup.Children.Add(controlPointsLine);
                }

                if (controlsId == 0)
                {
                    controlPointsLine = new LineGeometry(new Point(digit.VertexX, digit.VertexY), new Point(control[0], control[1]));
                    controlPointsLinesGeometryGroup.Children.Add(controlPointsLine);
                }
            }
            digitControlPointsLinesPath.Data = controlPointsLinesGeometryGroup;
        }
        private Digit GetInterpolatedDigit(int currentDigitNumber, int nextDigitNumber, double ratio)
        {                                  
            Digit result = new Digit();

            BezierDigit current = new BezierDigit(currentDigitNumber);
            BezierDigit next = new BezierDigit(nextDigitNumber);

            result.VertexX =  Math.Round(MathHelper.Lerp(current.VertexX, next.VertexX, ratio), 1);
            result.VertexY =  Math.Round(MathHelper.Lerp(current.VertexY, next.VertexY, ratio), 1);

            result.Controls = new double[4][];

            for (int i = 0; i < 4; i++)
            {
                result.Controls[i] = new double[6];
                for (int j = 0; j < 6; j++)
                {
                    if (j % 2 == 0)
                        result.Controls[i][j] = Math.Round(MathHelper.Lerp(current.Control(i)[j], next.Control(i)[j], ratio), 1);
                    else
                        result.Controls[i][j] =  Math.Round(MathHelper.Lerp(current.Control(i)[j], next.Control(i)[j], ratio), 1);
                }
            }

            HashItem hs = new HashItem() { c = currentDigitNumber, n = nextDigitNumber, r = ratio, d = result };
            Settings.h.Add(hs);

            return result;
        }
    }
}