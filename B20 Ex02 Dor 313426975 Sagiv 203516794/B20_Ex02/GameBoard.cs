﻿using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class GameBoard
    {
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private Cube[,] m_Board;

        public GameBoard(int i_Rows, int i_Cols)
        {
            r_NumOfCols = i_Cols;
            r_NumOfRows = i_Rows;
            m_Board = new Cube[r_NumOfRows, r_NumOfCols];
            boardPreparartion();
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

            public bool IsHidden
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

            return m_Board[i_rowNum, i_colNum].SymbolOfIcon;
        }

        public int GetIconInCoordinate(int i_rowNum, int i_colNum)
        {
            return m_Board[i_rowNum, i_colNum].SymbolOfIcon;
        }

        public void HideIcon(int i_rowNum, int i_colNum)
        {
            m_Board[i_rowNum,i_colNum].HideCube();
        }

        public bool gameHasFinished()
        {
            bool v_GameOver = true;

            foreach (Cube cube in m_Board)
            {
                if (cube.IsHidden == true)
                {
                    v_GameOver = false;
                    break;
                }
            }

            return v_GameOver;
        }

        public bool alreadyExposed(int io_Row, int io_Col)
        {
            return m_Board[io_Row, io_Col].IsHidden == false;
        }

        private void boardPreparartion()
        {
            int numOfIcons = r_NumOfCols * r_NumOfRows;
            int numOfInserts = numOfIcons / 2;

            List<int> listOfIcon = new List<int>(numOfIcons);

            for (int i = 1; i < numOfInserts + 1; i++)
            {
                listOfIcon.Add(i);
                listOfIcon.Add(i);
            }

            mixingCardBeforeStart(ref listOfIcon);
        }

        private void mixingCardBeforeStart(ref List<int> i_ListOfIcon)
        {
            Random random = new Random();

            for (int i = 0; i < r_NumOfRows; i++)
            {
                for (int j = 0; j < r_NumOfCols; j++)
                {
                    // 0 saved to space symbol
                    int randomNumber = random.Next(0, i_ListOfIcon.Count);
                    m_Board[i, j] = new Cube(i_ListOfIcon[randomNumber], true);
                    i_ListOfIcon.RemoveAt(randomNumber);
                }
            }
        }

    }
}
