﻿using System;
using System.Collections.Generic;

namespace B20_Ex02
{
    public class GameBoard
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
            private readonly int r_SymbolOfIcon;
            private bool v_IsHidden;

            public Cube(int i_Icon, bool i_IsHidden)
            {
                r_SymbolOfIcon = i_Icon;
                v_IsHidden = i_IsHidden;
            }

            public bool IsHidden
            {
                get { return v_IsHidden; }
                set { IsHidden = value; }
            }

            // hide icon if havent exposed yet with 0 int (in ui logical 0 represent space)
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
                        result = r_SymbolOfIcon;
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

        public int ExposeSymbolAndTakeValue(Coordinate i_MoveCoordinate)
        {
            m_Board[i_MoveCoordinate.Row, i_MoveCoordinate.Col].ExposeCube();

            return m_Board[i_MoveCoordinate.Row, i_MoveCoordinate.Col].SymbolOfIcon;
        }

        public int GetIconInCoordinate(Coordinate i_MoveCoordinate)
        {
            return m_Board[i_MoveCoordinate.Row, i_MoveCoordinate.Col].SymbolOfIcon;
        }

        public void HideIcon(Coordinate i_MoveCoordinate)
        {
            m_Board[i_MoveCoordinate.Row, i_MoveCoordinate.Col].HideCube();
        }

        public bool checkIfGamehasFinished()
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

        public bool CheckIfAlreadyExposed(Coordinate i_CurrMoveCoordinate)
        {
            return m_Board[i_CurrMoveCoordinate.Row, i_CurrMoveCoordinate.Col].IsHidden == false;
        }

        // make a mix of int according to the size of the board and divide them randomaly between the cubes
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

        private void mixingCardBeforeStart(ref List<int> io_ListOfIcon)
        {
            Random random = new Random();

            for (int i = 0; i < r_NumOfRows; i++)
            {
                for (int j = 0; j < r_NumOfCols; j++)
                {
                    // 0 saved to space symbol
                    int randomNumber = random.Next(0, io_ListOfIcon.Count);
                    m_Board[i, j] = new Cube(io_ListOfIcon[randomNumber], true);
                    io_ListOfIcon.RemoveAt(randomNumber);
                }
            }
        }
    }
}
