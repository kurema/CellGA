using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CellGA.RhinoTools;

namespace TestConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var map = new CellObject.BoxelUnbounded();
            map.Apply(new CellObject.Rules.BuildCylinder(1, 0, 0, 1));
            for (int i = 0; i < 3; i++)
            {
                map.Apply(new CellObject.Rules.CountOdd(1, 1, 1, 3));
            }

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    var dump = map.GetDump();
                    Console.Write(dump[0].Get(i, j) + " ");
                }
                Console.WriteLine();
            }

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    Console.Write(GetRealAddress(j,i).ToString("D2") + " ");
                }
                Console.WriteLine();
            }


            Console.WriteLine("Hello, world!");
            Console.Read();
        }

        private static int GetRealAddress(int x, int y)
        {
            if (x == 0 && y == 0) { return 0; }

            int MaxNum = Math.Max(Math.Abs(x), Math.Abs(y));
            int CoreSize = MaxNum * 2 - 1;
            if (x == -MaxNum && y != MaxNum)
            {
                return CoreSize * CoreSize + (y + MaxNum);
            }
            else if (y == -MaxNum)
            {
                return CoreSize * CoreSize + (CoreSize + 1) + (x + MaxNum - 1);
            }
            else if (x == MaxNum)
            {
                return CoreSize * CoreSize + (CoreSize + 1) * 2 + (y + MaxNum - 1);
            }
            else
            {
                return CoreSize * CoreSize + (CoreSize + 1) * 3 + (x + MaxNum);
            }
        }
    }
}
