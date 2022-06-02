using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bezier
{
    public static class MathHelper
    {
        public static double Lerp(double lhs, double rhs, double t)
        {
            return (1.0d- t) * lhs + t * rhs;
        }
    }
}
