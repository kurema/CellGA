using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurema.CellGA
{
    public static class Functions
    {
        public static double Distance(params double[] crd)
        {
            double sum = 0;
            for(int i = 0; i < crd.Count(); i += 2)
            {
                sum += Math.Pow(crd[i] - crd[i + 1], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}
