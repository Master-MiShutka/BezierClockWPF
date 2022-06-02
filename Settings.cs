using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace Bezier
{
    public class Settings : INotifyPropertyChanged
    {
        private static Settings _instance = new Settings();

        public static Settings Instance { get { return _instance; } }

        private Brush digitColor = Brushes.Green;
        private double digitStrokeThickness = 10.0d;

        private bool drawControlLines = false;
        private Brush controlLinesColor = Brushes.Black;
        private double controlLinesStrokeThickness = 1.0d;
        private double controlCurveVertexSize = 10.0d;

        private int width = 800;
        private int height = 600;

        private int animationType = 3;
        private bool continualAnimation = false;
        private double scaling = 1.0;

        public static HashSet<HashItem> h = new HashSet<HashItem>();

        #region Свойства

        public DataRepository DataRepository = new DataRepository(DataRepositoryType.XMLResource);

        public Brush DigitColor
        {
            get { return digitColor; }
            set
            {
                if (digitColor != value)
                {
                    digitColor = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DigitColor"));
                }
            }
        } // DigitStrokeThickness
        public double DigitStrokeThickness
        {
            get { return digitStrokeThickness; }
            set
            {
                if (digitStrokeThickness != value)
                {
                    digitStrokeThickness = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DigitStrokeThickness"));
                }
            }
        } // DigitStrokeThickness
        public bool DrawControlLines
        {
            get { return drawControlLines; }
            set
            {
                if (drawControlLines != value)
                {
                    drawControlLines = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DrawControlLines"));
                }
            }
        } // DrawControlLines
        public Brush ControlLinesColor
        {
            get { return controlLinesColor; }
            set
            {
                if (controlLinesColor != value)
                {
                    controlLinesColor = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("ControlLinesColor"));
                }
            }
        } // ControlLinesColor
        public double ControlLinesStrokeThickness
        {
            get { return controlLinesStrokeThickness; }
            set
            {
                if (controlLinesStrokeThickness != value)
                {
                    controlLinesStrokeThickness = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("ControlLinesStrokeThickness"));
                }
            }
        } // ControlLinesStrokeThickness
        public double ControlCurveVertexSize
        {
            get { return controlCurveVertexSize; }
            set
            {
                if (controlCurveVertexSize != value)
                {
                    controlCurveVertexSize = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("ControlCurveVertexSize"));
                }
            }
        } // ControlCurveVertexSize
        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                }
            }
        } // Width
        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Height"));
                }
            }
        } // Height
        /// <summary>
        /// // 0 for linear, 1 for quadratic, 2 for cubic, 3 for sinuisoidial
        /// </summary>
        public int AnimationType
        {
            get { return animationType; }
            set
            {
                if (animationType != value)
                {
                    animationType = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("AnimationType"));
                }
            }
        } // AnimationType
        public bool ContinualAnimation
        {
            get { return continualAnimation; }
            set
            {
                if (continualAnimation != value)
                {
                    continualAnimation = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("ContinualAnimation"));
                }
            }
        } // ContinualAnimation
        public double Scaling
        {
            get { return scaling; }
            set
            {
                if (scaling != value)
                {
                    scaling = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Scaling"));
                }
            }
        } // Scaling
        #endregion
        private Settings() { }

        #region Члены INotifyPropertChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
