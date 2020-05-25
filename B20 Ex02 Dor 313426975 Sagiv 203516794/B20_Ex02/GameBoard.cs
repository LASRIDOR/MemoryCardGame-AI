using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class GameBoard
    {
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private Cube[,] m_Board;
        private List<int> m_IconList;

        public GameBoard(int i_Rows, int i_Cols)
        {
            r_NumOfCols = i_Cols;
            r_NumOfRows = i_Rows;
            m_Board = new Cube[r_NumOfRows, r_NumOfCols];
            makeListOfIcon();
            mixingCardBeforeStart();
        }

        public Cube[,] Board
        {
            get { return m_Board; }
        }

        public int NumOfRows
        {
            get { return r_NumOfRows; }
        }
        public int NumOfCols
        {
            get { return r_NumOfCols; }
        }

        private struct Cube
        {
            private readonly int m_SymbolOfIcon;
            private bool v_IsHidden;
            public Cube(int i_Icon, bool i_IsHidden)
            {
                m_SymbolOfIcon = i_Icon;
                v_IsHidden = i_IsHidden;
            }

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
                        result = m_SymbolOfIcon;
                    }

                    return result;
                }
            }

            public void ExposeCube()
            {
                v_IsHidden = false;
            }

            public void HideCube()
            {
                v_IsHidden = true;
            }
        }

        public int ExposeSymbolAndTakeValue(int i_rowNum, int i_colNum)
        {
            m_Board[i_rowNum,i_colNum].ExposeCube();
            return m_Board[i_rowNum, i_colNum].SymbolOfIcon
        }

        public void HideIcon(int i_rowNum, int i_colNum)
        {
            m_Board[i_rowNum,i_colNum].HideCube();
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
                    // 0 saved to space symbol
                    int randomNumber = random.Next(1, m_IconList.Capacity+1);
                    m_Board[i, j] = new Cube(m_IconList[randomNumber], true);
                    m_IconList.RemoveAt(randomNumber);
                }
            }
        }

    }
}
