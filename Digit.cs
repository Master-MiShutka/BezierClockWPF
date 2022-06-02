using System.Collections;
using System.Collections.Generic;

namespace Bezier
{
    public class Digit
    {
        public double[][] Controls { get; set; }
        public double VertexX { get; set; }
        public double VertexY { get; set; }

        public override int GetHashCode()
        {
            int[] hashes = new int[6] {
                ((IStructuralEquatable)Controls[0]).GetHashCode(EqualityComparer<double>.Default),
                ((IStructuralEquatable)Controls[1]).GetHashCode(EqualityComparer<double>.Default),
                ((IStructuralEquatable)Controls[2]).GetHashCode(EqualityComparer<double>.Default),
                ((IStructuralEquatable)Controls[3]).GetHashCode(EqualityComparer<double>.Default),
                VertexX.GetHashCode(),
                VertexY.GetHashCode(),
            };
            int hash = ((IStructuralEquatable)hashes).GetHashCode(EqualityComparer<int>.Default);
            return hash;
        }       
    }
}
