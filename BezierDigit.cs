using System;

namespace Bezier
{
    public class BezierDigit
    {
        private double[][] controls;
        private double vertexX, vertexY;

        private double minX = 0.0;
        private double maxX = 0.0;
        private double minY = 0.0;
        private double maxY = 0.0;

        private double width, height;
        public BezierDigit(int digitNumber)
        {
            controls = new double[4][];

            Digit d = Settings.Instance.DataRepository.Digits[digitNumber];
            vertexX = d.VertexX;
            vertexY = d.VertexY;
            for (int i = 0; i < 4; i++ )
                controls[i] = d.Controls[i];           

            #region Width
            minX = vertexX;
            maxX = vertexX;

            for (int i=0; i<4; i++)
            {
                double[] control = Control(i);
                for (int j=0; j<6; j=j+2)
                {
                    if (control[j] < minX) minX = control[j];
                    if (control[j] > maxX) maxX = control[j];
                }
            }
            minX -= Settings.Instance.ControlCurveVertexSize;
            maxX += Settings.Instance.ControlCurveVertexSize;
            width = maxX - minX;
            #endregion

            #region Height
            minY = vertexY;
            maxY = vertexY;

            for (int i = 0; i < 4; i++)
            {
                double[] control = Control(i);
                for (int j = 1; j < 6; j = j + 2)
                {
                    if (control[j] < minY) minY = control[j];
                    if (control[j] > maxY) maxY = control[j];
                }
            }
            minY -= Settings.Instance.ControlCurveVertexSize;
            maxY += Settings.Instance.ControlCurveVertexSize;
            height = maxY - minY;
            #endregion*/
        }
        // Возвращает контрольные точки с учетом масштаба
        public double[] Control(int index)
        {
            double[] scaledControl = new double[6];
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                    scaledControl[i] = controls[index][i] * Settings.Instance.Scaling;
                else
                    scaledControl[i] = controls[index][i] * Settings.Instance.Scaling;
            }
            return scaledControl;
        }
        public double VertexX
        {
            get
            {
                return vertexX * Settings.Instance.Scaling;
            }
        }
        public double VertexY
        {
            get
            {
                return vertexY * Settings.Instance.Scaling;
            }
        }
        public double Width
        {
            get { return width; }
        }
        public double Height
        {
            get { return height; }
        }
    }
}