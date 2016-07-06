using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cellularAutomaton
{
    public class HoneycombCellUnbounded
    {
            
    }

    public class Basic
    {
        public class ExpandableMap<T>
        {
            public T defaultValue;
            private T[,] Core;
            private int CoreX;
            private int CoreY;
            private List<T> Attachment = new List<T>();
            private List<DirectionLengthCombination> AttachmentDirection = new List<DirectionLengthCombination>();

            private enum Direction
            {
                Up,Down,Right,Left
            }

            private struct DirectionLengthCombination
            {
                Direction Direction;
                int Length;
            }

            public T Get(int x,int y)
            {
                int RightCoor = CoreX;
                int TopCoor = CoreY;
                int LeftCoor = CoreX + Core.GetLength(0);
                int BottomCoor = CoreY + Core.GetLength(1);
                for (int i = 0; i < AttachmentDirection.Count; i++)
                {
                    if (RightCoor <= x && x < LeftCoor && TopCoor <= y && y < BottomCoor)
                    {

                    }
                }

                return defaultValue;
            }
        }
    }
}
