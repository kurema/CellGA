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
            //RealAddress();
            TestGA1();

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
            var genes = new List<Gene>(samples);
            var evaluationList = new List<double>(samples);

            for (int i = 0; i < samples; i++)
            {
                genes.Add(new Gene());
                genes[i].Init(rd.Next());
            }

            for (int j = 0; j < 10; j++)
            {
                var Evaluations = new Dictionary<Gene, double>();
                Console.WriteLine(j + "..");

                for (int i = 0; i < samples; i++)
                {
                    genes[i].RandomizeSeed(0.001);
                    genes[i].RandomizeTarget(0.001);

                    Evaluations[genes[i]] = Gene.Evaluate(genes[i], 3, 6, 9);
                }

                genes.Sort((a, b) => { return ((Evaluations[b].CompareTo(Evaluations[a]))); });
                for (int i = 0; i < samples; i++)
                {
                    Console.WriteLine(Evaluations[genes[i]]);
                }

                for (int i = 0; i < samples; i++)
                {
                    if (Evaluations[genes[i]] < 0.1 || i > genes.Count * 0.5)
                    {
                        var rand = rd.NextDouble();

                        if (rand < 0.5)
                        {
                            genes[i] = new Gene();
                            genes[i].Init(rd.Next());
                            genes[i].RandomizeSeed(0.01);
                            genes[i].RandomizeTarget(0.01);
                        }
                        else
                        {
                            genes[i] = genes[rd.Next(genes.Count/3)].Duplicate();
                            genes[i].Swap(genes[rd.Next(genes.Count/3)]);
                            genes[i].RandomizeSeed(0.01);
                            genes[i].RandomizeTarget(0.01);
                        }
                    }
                }
                Console.WriteLine("Best:" + Evaluations[genes[0]]);
            }
            Console.WriteLine();
            Console.WriteLine(genes[0].GetBoxel(3).ToString(0));
            Console.WriteLine();
            Console.WriteLine(genes[0].GetBoxel(6).ToString(0));
            Console.WriteLine();
            Console.WriteLine(genes[0].GetBoxel(9).ToString(0));
        }

        public class Gene
        {
            private Random Random;

            public int[] Seed;
            public int Target;

            int MaxValue=3;

            public void Init(int seed)
            {
                this.Random = new Random(seed);

                Seed = new int[2048];
                for(int i = 0; i < 2048; i++)
                {
                    Seed[i] = Random.Next(MaxValue);
                }

                Target = Random.Next(MaxValue - 1) + 1;
            }

            public void RandomizeSeed(double rate)
            {
                for (int i = 0; i < this.Seed.Count() * rate; i++)
                {
                    Seed[Random.Next(2048)] = Random.Next(MaxValue);
                }
            }

            public void Swap(Gene target)
            {
                int minSeedCnt = Math.Min(target.Seed.Count(), this.Seed.Count());
                if (this != target)
                {
                    int a = Random.Next(minSeedCnt);
                    int b = Random.Next(minSeedCnt);

                    for(int i= Math.Min(a, b); i < Math.Max(a, b); i++)
                    {
                        int temp = target.Seed[i];
                        //target.Seed[i] = this.Seed[i];
                        this.Seed[i] = temp;
                    }
                }
            }

            public void RandomizeTarget(double rate)
            {
                if (Random.NextDouble() < rate)
                {
                    Target = Random.Next(MaxValue - 1) + 1;
                }
            }

            public Gene Duplicate()
            {
                var result = new Gene();
                result.Init(Random.Next());
                result.Target = this.Target;
                result.Seed = new int[this.Seed.Count()];
                result.MaxValue = this.MaxValue;
                Array.Copy(this.Seed, result.Seed, this.Seed.Count());
                return result;
            }

            public CellGA.RhinoTools.CellObject.BoxelUnbounded GetBoxel(int cnt)
            {
                var map = new CellObject.BoxelUnbounded();
                map.Apply(new CellObject.Rules.BuildCylinder(1, 0, 0, 1));

                for (int i = 0; i < cnt; i++)
                {
                    map.Apply(new CellObject.Rules.GeneralHorizontalAndUpDown(this.Target, this.Seed));
                }
                return map;
            }

            public static double Evaluate(Gene g,params int[] size)
            {
                double result = 0;
                foreach(var item in size)
                {
                    var temp= Evaluate(g.GetBoxel(item));
                    result += temp * temp;
                }
                return result;
            }

            public static double Evaluate(CellObject.BoxelUnbounded map)
            {
                int x1, x2, y1, y2;
                int z = 0;
                map.GetSize(z, out x1, out y1, out x2, out y2);
                double e1 = 0, e2 = 0, e3 = 0;
                int count = 0;
                int c1 = 0, c2 = 0, c3 = 0;

                for (int x = x1; x < x2; x++)
                {
                    for (int y = y1; y < y2; y++)
                    {
                        var neigh = map.GetNeighbor(x, y, z);

                        switch (neigh.Self)
                        {
                            case 1:
                                e1 += Math.Pow(0.1, Math.Abs(neigh.CountNeighbor(2) - 8));
                                c1++;
                                break;
                            case 2:
                                e2 += Math.Pow(0.1, Math.Abs(neigh.CountNeighbor(1) - 1));
                                c2++;
                                break;
                            case 3:
                                c3++;
                                break;
                        }
                        count++;
                    }
                }
                //return e1 / c1 + e2 / c2;
                return (e1 + e2) / count;
            }
        }
    }
}
