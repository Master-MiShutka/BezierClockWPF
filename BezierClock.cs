using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bezier
{
    public class BezierClock
    {
        private BezierDigitAnimator hoursTensDigit;
        private BezierDigitAnimator hoursUnitsDigit;

        private BezierDigitAnimator minutesTensDigit;
        private BezierDigitAnimator minutesUnitsDigit;

        private BezierDigitAnimator secondsTensDigit;
        private BezierDigitAnimator secondsUnitsDigit;

        private Path colon1;
        private Path colon2;

        private const int animTypeChangeCounterMax = 50;
        private int animTypeChangeCounter = animTypeChangeCounterMax;

        private TextBlock tbInfo = new TextBlock();

        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerSecond = new DispatcherTimer();

        private Grid host;

        public BezierClock(Grid grid)
        {
            /*for (int i = 0; i < 10; i++)
                new BezierDigit(i).Modify(0);*/
            
            if (grid == null) throw new ArgumentNullException("Clock's Host is null");

            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            Grid host = new Grid();
            host.HorizontalAlignment = HorizontalAlignment.Left;

            grid.Children.Add(host);
            grid.Children.Add(tbInfo);

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Grid.SetRow(host, 0);
            Grid.SetRow(tbInfo, 1);           

            Settings.Instance.PropertyChanged += Settings_PropertyChanged;

            tbInfo.Margin = new Thickness(10);
            tbInfo.HorizontalAlignment = HorizontalAlignment.Center;
            tbInfo.FontFamily = new FontFamily("Centuri Gothic");
            tbInfo.FontSize = 18.0;
            tbInfo.Foreground = Settings.Instance.DigitColor;

            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Start();

            timerSecond.Tick += timerSecond_Tick;
            timerSecond.Interval = TimeSpan.FromSeconds(1);
            timerSecond.Start();

            this.host = host;
            Init();
        }
        private void Init()
        {
            host.Children.Clear();

            host.ColumnDefinitions.Clear();
            host.RowDefinitions.Clear();

            double width = new BezierDigit(4).Width / 3.0;
            double height = new BezierDigit(4).Height;

            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width, GridUnitType.Pixel) });

            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width, GridUnitType.Pixel) });

            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            host.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            hoursTensDigit = new BezierDigitAnimator(35995.0, 5.0, CreateHostPanel(host, 0));
            hoursUnitsDigit = new BezierDigitAnimator(3595.0, 5.0, CreateHostPanel(host, 1));
            minutesTensDigit = new BezierDigitAnimator(595.0, 5.0, CreateHostPanel(host, 3));
            minutesUnitsDigit = new BezierDigitAnimator(55.0, 5.0, CreateHostPanel(host, 4));
            secondsTensDigit = new BezierDigitAnimator(5.0, 5.0, CreateHostPanel(host, 6));
            secondsUnitsDigit = new BezierDigitAnimator(0.0, 1.0, CreateHostPanel(host, 7));

            colon1 = CreateColon(width, height);
            colon2 = CreateColon(width, height);

            Panel p1 = CreateHostPanel(host, 2);
            Panel p2 = CreateHostPanel(host, 5);
            
            p1.Children.Add(colon1);
            p2.Children.Add(colon2);           
        }

        private Panel CreateHostPanel(Panel owner, int colIndex)
        {
            Grid grid = new Grid();
            owner.Children.Add(grid);
            grid.Margin = new Thickness(2);
//            grid.Background = (DrawingBrush)grid.TryFindResource("GridBrush"); //Brushes.Bisque;
            Grid.SetColumn(grid, colIndex);

            return grid;
        }

        private Path CreateColon(double width, double height)
        {
            Path path = new Path();

            GeometryGroup controlPointsVertexGeometryGroup = new GeometryGroup();
            controlPointsVertexGeometryGroup.FillRule = FillRule.Nonzero;

            double pointsRadius = height / 50.0;

            EllipseGeometry controlPointsVertexCircle = new EllipseGeometry();

            double h2h = height / 3.0;

            controlPointsVertexCircle = new EllipseGeometry(new Point(width / 2.0, h2h), pointsRadius, pointsRadius);
            controlPointsVertexGeometryGroup.Children.Add(controlPointsVertexCircle);
            controlPointsVertexCircle = new EllipseGeometry(new Point(width / 2.0, 2.0 * h2h), pointsRadius, pointsRadius);
            controlPointsVertexGeometryGroup.Children.Add(controlPointsVertexCircle);

            path.Data = controlPointsVertexGeometryGroup;
            path.Stroke = Settings.Instance.DigitColor;
            path.Fill = Brushes.Transparent;
            path.StrokeThickness = Settings.Instance.DigitStrokeThickness;

            return path;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Draw();
        }
        void timerSecond_Tick(object sender, EventArgs e)
        {
            if (colon1.Visibility == Visibility.Hidden)
            {
                colon1.Visibility = Visibility.Visible;
                colon2.Visibility = Visibility.Visible;
            }
            else
            {
                colon1.Visibility = Visibility.Hidden;
                colon2.Visibility = Visibility.Hidden;
            }
        }
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnimationType")
            {
                animTypeChangeCounter = animTypeChangeCounterMax;
            }
            Init();
        }
        //private int frameCount = 0;
        public void Draw()
        {           
            if (animTypeChangeCounter > 0)
            {                
                String[] states = { "линейная", "квадратичная", "кубическая", "синусоидная" };
                string s = states[Settings.Instance.AnimationType];
                tbInfo.Text = s;

                Color c = (Settings.Instance.DigitColor as SolidColorBrush).Color;
                SolidColorBrush brush = new SolidColorBrush(c);

                double t2 = (double)animTypeChangeCounter / (double)animTypeChangeCounterMax;
                brush.Opacity = MathHelper.Lerp(0.1, 1.0, t2);
                tbInfo.Foreground = brush;
            }

            if (animTypeChangeCounter > 0) animTypeChangeCounter--;
            if (animTypeChangeCounter == 0)
            {
                tbInfo.Text = string.Empty;
                tbInfo.Foreground = Settings.Instance.DigitColor;
            }

            // Seconds
            var d = DateTime.Now;
            int millis = d.Millisecond;
            int secondTotal = d.Second;
            int secondsUnit = secondTotal % 10;
            int secondsTen = (secondTotal % 100 - secondTotal % 10) / 10;
            double secondsUnitRatio = millis / 1000.0;
            double secondsTenRatio = (secondsUnit * 1000 + millis) / 10000.0;
            secondsUnitsDigit.AnimationStartRatio = getAnimStartRatio(1.0);
            secondsUnitsDigit.Update(secondsUnit, getNextInt(secondsUnit, 9), secondsUnitRatio);

            secondsTensDigit.AnimationStartRatio = getAnimStartRatio(10.0);
            secondsTensDigit.Update(secondsTen, getNextInt(secondsTen, 5), secondsTenRatio);

            // Minutes
            int minuteTotal = d.Minute;
            int minutesUnit = minuteTotal % 10;
            int minutesTen = (minuteTotal % 100 - minuteTotal % 10) / 10;
            double minutesUnitRatio = (secondTotal * 1000 + millis) / 60000.0;
            double mintuesTenRatio = (minutesUnit * 60000 + secondTotal * 1000 + millis) / 600000.0;
            minutesTensDigit.AnimationStartRatio = getAnimStartRatio(600.0);
            minutesTensDigit.Update(minutesTen, getNextInt(minutesTen, 5), mintuesTenRatio);
            minutesUnitsDigit.AnimationStartRatio = getAnimStartRatio(60.0);
            minutesUnitsDigit.Update(minutesUnit, getNextInt(minutesUnit, 9), minutesUnitRatio);

            // Hours
            int hoursTotal = d.Hour;
            int hoursUnit = hoursTotal % 10;
            int hoursTen = (hoursTotal % 100 - hoursTotal % 10) / 10;
            double hoursUnitRatio = (minuteTotal * 60000 + secondTotal * 1000 + millis) / 3600000.0;
            double hoursTenRatio;
            int hoursUnitNext;
            if (hoursTen == 2 && hoursUnit == 3)
            {
                hoursUnitNext = 0;
                hoursTenRatio = (hoursUnit * 3600000 + minuteTotal * 60000 + secondTotal * 1000 + millis) / (4 * 3600000.0); // because only 20, 21, 22, 23 and not up to 29
                hoursTensDigit.AnimationStartRatio = getAnimStartRatio(3600.0 * 4.0);
            }
            else
            {
                hoursUnitNext = getNextInt(hoursUnit, 9);
                hoursTenRatio = (hoursUnit * 3600000 + minuteTotal * 60000 + secondTotal * 1000 + millis) / 36000000.0;
                hoursTensDigit.AnimationStartRatio = getAnimStartRatio(3600.0 * 10.0);
            }
            hoursTensDigit.Update(hoursTen, getNextInt(hoursTen, 2), hoursTenRatio);
            hoursUnitsDigit.AnimationStartRatio = getAnimStartRatio(3600.0);
            hoursUnitsDigit.Update(hoursUnit, hoursUnitNext, hoursUnitRatio);
        }
        private double getAnimStartRatio(double totalDuration)
        {
              if (AnimDurationUser > totalDuration)
              {
                return 0.0;
              }
              else
              {
                  return 1.0 - (AnimDurationUser / totalDuration);
              }
        }
        private int getNextInt(int current, int max)
        {
            if (current >= max)
            {
                return 0;
            }
            else
            {
                return current + 1;
            }
        }

        public int AnimDurationUser = 1;
    }
}
