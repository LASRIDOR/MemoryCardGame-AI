using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class GameBoardTest<>
    {
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private Dictionary<T,B> m_Board;
        private List<int> m_IconList;

        public Cube[,] Board
        {
            get { return m_Board; }
        }


        public struct Cube
        {
            private readonly
            private readonly int m_SymbolOfIcon;
            private bool v_IsHidden;

            private bool IsHidden
            {
                set
                {
                    IsHidden = value;
                }
                get
                {
                    return v_IsHidden;
                }
            }


            public int SymbolOfIcon
            {
                get
                {
                    int result;
                    if (IsHidden == true)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = m_Icon
                    }

                    return result;
                }
            }

            public Cube(int i_Icon, bool i_IsHidden)
            {
                m_Icon = i_Icon;
                v_IsHidden = i_IsHidden;
            }

        }

        public BoardGame(int i_Rows, int i_Cols)
        {
            r_NumOfCols = i_Cols;
            r_NumOfRows = i_Rows;
            m_Board = new Cube[r_NumOfRows, r_NumOfCols];
            makeListOfIcon();
            mixingCardBeforeStart();
        }

        private void makeListOfIcon()
        {
            int numOfNumbers = r_NumOfCols * r_NumOfRows / 2;

            for (int i = 0; i < numOfNumbers; i++)
            {
                m_IconList.Add(0);
                m_IconList.Add(0);
            }
        }

        private void mixingCardBeforeStart()
        {
            Random random = new Random();

            for (int i = 1; i <= r_NumOfRows; i++)
            {
                for (int j = 1; j <= r_NumOfCols; i++)
                {
                    int randomNumber = random.Next(0, m_IconList.Capacity);
                    m_Board[i, j] = new Cube(m_IconList[randomNumber], true);
                    m_IconList.RemoveAt(randomNumber);
                }
            }
        }

        private void clearBoard()
        {
            m_Board.
        }
    }
}
