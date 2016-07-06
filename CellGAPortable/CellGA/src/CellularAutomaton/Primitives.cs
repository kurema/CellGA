using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CellGA.Primitives
{
    public class Collections
    {
        /// <summary>
        /// 添字を負数にできる二次元可変長配列です。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IUnbounded2dArray<T> : IEnumerable<AddressValuePair2d<T>>
        {
            T DefaultValue { get; }
            T Get(int x, int y);
            void Set(int x, int y, T value);
            IUnbounded2dArray<T> Duplicate();
            void GetSize(out int x1, out int y1, out int x2, out int y2);
        }

        public struct AddressValuePair2d<T>
        {
            public int X;
            public int Y;
            public T Value;
        }

        public struct Address2d
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// 添字を負数にできる二次元可変長配列です。範囲外の値の設定時にそれを含む正方形の領域を初期化します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Unbounded2dArraySquare<T> : IUnbounded2dArray<T>
        {
            public List<T> Content { get; private set; } = new List<T>();

            public T DefaultValue { get; private set; }

            public T Get(int x, int y)
            {
                var index = GetRealAddress(x, y);
                if (index < Content.Count)
                {
                    return Content[index];
                }
                else
                {
                    return DefaultValue;
                }
            }

            public void Set(int x, int y, T value)
            {
                var index = GetRealAddress(x, y);
                if (index < Content.Count)
                {
                    Content[index] = value;
                    return;
                }
                for (int i = Content.Count; i < index; i++)
                {
                    Content.Add(DefaultValue);
                }
                Content.Add(value);
            }

            public Unbounded2dArraySquare(T defaultValue)
            {
                this.DefaultValue = defaultValue;
            }

            public Unbounded2dArraySquare(Unbounded2dArraySquare<T> org)
            {
                this.DefaultValue = org.DefaultValue;
                this.Content = new List<T>(org.Content);
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

            private static Address2d Get2dAddress(int index)
            {
                if (index == 0) { return new Address2d() { X = 0, Y = 0 }; }

                int MaxNum = (int)Math.Floor(Math.Sqrt(index) / 2.0 + 0.5);
                int CoreSize = (int)(MaxNum * 2) - 1;

                index -= CoreSize * CoreSize;
                if (index <= CoreSize)
                {
                    return new Address2d() { X = -MaxNum, Y = index - MaxNum };
                }
                else if (index <= CoreSize + (CoreSize + 1))
                {
                    return new Address2d() { Y = -MaxNum, X = index - (CoreSize + 1) - MaxNum + 1 };
                }
                else if (index <= CoreSize + (CoreSize + 1) * 2)
                {
                    return new Address2d() { X = MaxNum, Y = index - (CoreSize + 1) * 2 - MaxNum + 1 };
                }
                else
                {
                    return new Address2d() { Y = MaxNum, X = index - (CoreSize + 1) * 3 - MaxNum };
                }
            }

            public IEnumerator<AddressValuePair2d<T>> GetEnumerator()
            {
                for (int i = 0; i < Content.Count; i++)
                {
                    var address = Get2dAddress(i);
                    yield return new AddressValuePair2d<T> { X = address.X, Y = address.Y, Value = Content[i] };
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IUnbounded2dArray<T> Duplicate()
            {
                return new Unbounded2dArraySquare<T>(this);
            }

            public void GetSize(out int x1, out int y1, out int x2, out int y2)
            {
                int MaxNum = (int)Math.Floor(Math.Sqrt(Content.Count - 1) / 2.0 + 0.5);
                x1 = -MaxNum;
                y1 = -MaxNum;
                x2 = MaxNum;
                y2 = MaxNum;
            }
        }
    }

}

