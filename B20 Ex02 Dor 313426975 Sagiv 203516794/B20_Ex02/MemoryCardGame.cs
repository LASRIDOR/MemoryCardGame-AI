using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace B20_Ex02
{
    public class MemoryCardGame
    {
        private Nullable<BoardGame> m_BoardGameboardGame;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;

        public MemoryCardGame(string i_NameOfPlayerOne, string i_NameOfPlayerTwo)
        {
            m_PlayerOne = new Player(i_NameOfPlayerOne);
            m_PlayerTwo = new Player(i_NameOfPlayerTwo);
            m_BoardGameboardGame = null;
        }

        public void NewGame(int i_RowNum, int i_ColNum)
        {
            clearBoard();
            m_BoardGameboardGame = new BoardGame(i_RowNum, i_ColNum);
            bool v_WantToContinue = true;

            while (v_WantToContinue == true)
            {

            }
        }

        private struct Player
        {
            private readonly string m_nameOfPlayer;
            private Nullable<Ai> m_Ai;

            public bool isAi()
            {
                if (m_Ai == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public Player(string i_NameOfPlayer)
            {
                if (i_NameOfPlayer == null)
                {
                    m_nameOfPlayer = "RoboMatch";
                    m_Ai = new Ai();
                }
                else
                {
                    m_nameOfPlayer = i_NameOfPlayer;
                    m_Ai = null;
                }
            }

            private struct Ai
            {
                private int memoryOfCardsInTime;
                private int counterToRenewMemory;

            }
        }


        public struct BoardGame
        {
            private readonly int r_NumOfRows;
            private readonly int r_NumOfCols;
            private Cube[,] m_Board;
            private List<int> m_IconList;

            public Cube[,] Board
            {
                get { return m_Board; }
            }


            public struct Cube
            {
                private readonly int m_Icon;
                private bool v_IsHidden;

                public bool IsHidden
                {
                    set { v_IsHidden = value; }
                    get { return v_IsHidden; }
                }

                public int Icon
                {
                    get { return m_Icon; }
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
        private void clearBoard()
        {
            if (m_BoardGameboardGame != null)
            {
                m_BoardGameboardGame.Value.Board.
            }

        }
    }
}
