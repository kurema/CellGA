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
            RealAddress();
            //TestGA1();

            Console.Read();
        }

        public static void TemporaryPlaceholder()
        {
            var map = new CellObject.BoxelUnbounded();
            map.Apply(new CellObject.Rules.BuildCylinder(1, 0, 0, 1));
            map.Apply(new CellObject.Rules.GeneralHorizontalAndUpDown(1, new int[] { }));

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    var dump = map.GetDump();
                    Console.Write(dump[0].Get(i, j) + " ");
                }
                Console.WriteLine();
            }
        }

        public static void RealAddress()
        {
            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    Console.Write(GetRealAddress(j, i).ToString("D2") + " ");
                }
                Console.WriteLine();
            }
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

        public static void TestGA1()
        {
            Random rd = new Random();

            int samples = 30;
            var genes = new Gene[samples];
            for (int i = 0; i < samples; i++)
            {
                genes[i] = new Gene();
                genes[i].Init(rd.Next());
            }

            CellObject.BoxelUnbounded best = new CellObject.BoxelUnbounded();
            double maxVal = double.MinValue;

            for (int j = 0; j < 10; j++)
            {
                Console.WriteLine(j+"..");
                int del = -1;
                double minVal = double.MaxValue;

                double currentBestVal = double.MinValue;

                for (int i = 0; i < samples; i++)
                {
                    genes[i].Randominze(3);
                    var res = genes[i].GetBoxel();
                    var ev = genes[i].Evaluate(res);

                    currentBestVal = Math.Max(currentBestVal,ev);

                    if (ev > maxVal)
                    {
                        maxVal = ev;
                        best = res;
                    }

                    if (ev < minVal)
                    {
                        minVal = ev;
                        del = i;
                    }
                }
                Console.WriteLine(currentBestVal);
                genes[del] = new Gene();
                genes[del].Init(rd.Next());
            }
            Console.WriteLine();
            Console.WriteLine(maxVal);
            Console.WriteLine(best.ToString(0));
        }

        public class Gene
        {
            private Random Random;

            public int[] Seed;
            public int Target;

            int MaxValue=4;

            public void Init(int seed)
            {
                this.Random = new Random(seed);

                Seed = new int[2048];
                for(int i = 0; i < 2048; i++)
                {
                    Seed[i] = Random.Next(MaxValue);
                }

                Target = Random.Next(MaxValue);
            }

            public void Randominze(int cnt)
            {
                for(int i = 0; i < cnt; i++)
                {
                    Seed[Random.Next(2048)] = Random.Next(MaxValue);
                }
            }

            public Gene Duplicate()
            {
                return new Gene() { Seed = this.Seed, Target = this.Target };
            }

            public CellGA.RhinoTools.CellObject.BoxelUnbounded GetBoxel()
            {
                var map = new CellObject.BoxelUnbounded();
                map.Apply(new CellObject.Rules.BuildCylinder(1, 0, 0, 1));

                for (int i = 0; i < Random.Next(10) + 10; i++)
                {
                    map.Apply(new CellObject.Rules.GeneralHorizontalAndUpDown(this.Target, this.Seed));
                }
                return map;
            }

            public double Evaluate(CellObject.BoxelUnbounded map)
            {
                int x1, x2, y1, y2;
                int z = 0;
                map.GetSize(z, out x1, out y1, out x2, out y2);
                double e1 = 0, e2 = 0, e3 = 0;
                int c1 = 1, c2 = 1, c3 = 1;
                for (int x = x1; x < x2; x++)
                {
                    for (int y = y1; y < y2; y++)
                    {
                        var neigh = map.GetNeighbor(x, y, z);

                        switch (neigh.Self)
                        {
                            case 1:
                                c1++;
                                //e1 += 1 - (neigh.CountNeighbor(2) / (8.0 - neigh.CountNeighbor(0) - neigh.CountNeighbor(-1)));
                                e1 += 1 - Math.Pow( Math.Abs(4 - neigh.CountNeighbor(2))/4.0,3);
                                break;
                            case 2:
                                c2++;
                                e2 += 1 - Math.Pow(Math.Abs(4 - neigh.CountNeighbor(1)) / 4.0, 3);
                                //if (neigh.CountNeighbor(3) > 0) {
                                //    e2+=0.5;
                                //}
                                //if (neigh.CountNeighbor(1) > 0)
                                //{
                                //    e2 += 0.5;
                                //}
                                break;
                            case 3:
                                c3++;
                                break;
                        }
                    }
                }
                //return e1 / c1 + e2 / c2;
                return e1 + e2;
            }
        }
    }
}
