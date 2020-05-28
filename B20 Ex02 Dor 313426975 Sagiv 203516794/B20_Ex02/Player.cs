using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace B20_Ex02
{
    public class Player
    {
        private readonly string m_NameOfPlayer;
        private int m_Score;
        private Nullable<Ai> m_AiBrain;

        public Player.Ai AiBrain
        {
            get
            {
                return m_AiBrain.Value;
            }
        }

        public Player(string i_NameOfPlayer,bool isAi)
        {
            if (isAi == true)
            {
                m_NameOfPlayer = "RoboMatch";
                m_AiBrain = new Ai();
            }
            else
            {
                m_NameOfPlayer = i_NameOfPlayer;
                m_AiBrain = null;
            }
        }

        public string NameOfPlayer
        {
            get { return m_NameOfPlayer; }
        }

        public int Score
        {
            get { return m_Score; }
        }

        // for kriot
        public bool isAi()
        {
            if (m_AiBrain.HasValue == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public struct Ai
        {
            private enum ePlayingStrategy
            {
                Zero_Move = 0,
                One_Move = 1,
                Two_Move = 2,
                Three_Move = 3
            }

            private Dictionary<int,string> m_DictionaryMemoryOfCardsInTime;
            private int m_PairsOnTable;
            private ePlayingStrategy m_MyNextKindMove;
            private List<string> m_ListOfSureWins;

            public string MaikngFirstMove()
            {
                string firstMove;

                if (m_MyNextKindMove == ePlayingStrategy.Two_Move || m_MyNextKindMove == ePlayingStrategy.One_Move)
                {
                    firstMove = getCardForMove(false);
                }
                else // m_MyNextKindMove == ePlayingStrategy.Zero_Move
                {
                    firstMove = getCardForMove(true);
                }

                return firstMove;
            }

            public string MaikngSecondMove(string i_FirstMoveCardRevealed, int i_SymbolOfFirstMoveCardRevealed)
            {
                string secondMove;

                if (m_MyNextKindMove == ePlayingStrategy.Two_Move || m_MyNextKindMove == ePlayingStrategy.One_Move)
                {
                    if (m_DictionaryMemoryOfCardsInTime.ContainsKey(i_SymbolOfFirstMoveCardRevealed) == true)
                    {
                        secondMove = m_DictionaryMemoryOfCardsInTime[i_SymbolOfFirstMoveCardRevealed];
                        m_DictionaryMemoryOfCardsInTime.Remove(i_SymbolOfFirstMoveCardRevealed);
                    }
                    else
                    {
                        if (m_MyNextKindMove == ePlayingStrategy.Two_Move)
                        {
                            secondMove = getCardForMove(false);
                        }
                        else
                        {
                            secondMove = getCardForMove(true);
                        }
                        cardsRevealed(i_FirstMoveCardRevealed, i_SymbolOfFirstMoveCardRevealed);
                    }
                }
                else if(m_MyNextKindMove == ePlayingStrategy.Zero_Move)
                {
                    secondMove = getCardForMove(true);
                }
                else// m_MyNextKindMove == ePlayingStrategy.Three_Move
                {
                    secondMove = m_ListOfSureWins[0];
                    m_ListOfSureWins.RemoveAt(0);
                }

                return secondMove;
            }

            private string getCardForMove(bool i_needKnownCard)
            {
                string move = null;

                foreach (char c in UI.r_ColSymbol)
                {
                    foreach (char c1 in UI.r_RowSymbol)
                    {
                        move = c.ToString() + c1.ToString();

                        if (m_DictionaryMemoryOfCardsInTime.ContainsValue(move) == i_needKnownCard)
                        {
                            break;
                        }
                    }

                }

                if (move == null)
                {
                    throw new Exception("Ai Didn't Find Any Card to make a move");
                }

                return move;
            }

            public void cardsRevealed(string i_Coordinate, int i_SymbolOfCoordinate)
            {
                if (m_DictionaryMemoryOfCardsInTime.ContainsKey(i_SymbolOfCoordinate))
                {
                    m_ListOfSureWins.Add(m_DictionaryMemoryOfCardsInTime[i_SymbolOfCoordinate]);
                    m_ListOfSureWins.Add(i_Coordinate);
                    m_MyNextKindMove = ePlayingStrategy.Three_Move;
                }
                else
                {
                    m_DictionaryMemoryOfCardsInTime.Add(i_SymbolOfCoordinate, i_Coordinate);

                    if ((m_DictionaryMemoryOfCardsInTime.Count == 0 ||
                         m_PairsOnTable + m_DictionaryMemoryOfCardsInTime.Count % 2 == 1) &&
                        (m_PairsOnTable != 6 && m_DictionaryMemoryOfCardsInTime.Count != 1))
                    {
                        m_MyNextKindMove = ePlayingStrategy.Two_Move;
                    }
                    else if ((m_DictionaryMemoryOfCardsInTime.Count >= 1 &&
                              m_PairsOnTable + m_DictionaryMemoryOfCardsInTime.Count % 2 == 0) ||
                             (m_PairsOnTable == 6 && m_DictionaryMemoryOfCardsInTime.Count == 1))
                    {
                        m_MyNextKindMove = ePlayingStrategy.One_Move;
                    }
                    else if (m_DictionaryMemoryOfCardsInTime.Count >= 2*(m_PairsOnTable + 1)/3 &&
                             m_PairsOnTable + m_DictionaryMemoryOfCardsInTime.Count % 2 == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.Zero_Move;
                    }
                }
            }

            public void resetMermory(int i_NumOfSquares)
            {
                m_DictionaryMemoryOfCardsInTime = new Dictionary<int, string>(i_NumOfSquares);
                m_ListOfSureWins = new List<string>(i_NumOfSquares);
                m_PairsOnTable = i_NumOfSquares;
                m_MyNextKindMove = ePlayingStrategy.Two_Move;
            }

        }

        // fix
       // public string MakeFirstMove()
        //{
        //    return AiBrain.MaikngFirstMove();
        //}

        // fix
       // public string MakeSecondMove(string i_FirstMoveCardRevealed, int i_symbolOfFirstMove)
        //{
         //   return m_Ai.Value.MaikngSecondMove(i_FirstMoveCardRevealed, i_symbolOfFirstMove);
        //}

        public void NewGame(int? i_NumOfRow, int? i_NumOfCol)
        {
            if (m_AiBrain.HasValue == true)
            {
                AiBrain.resetMermory(i_NumOfCol.Value * i_NumOfRow.Value);
            }
            m_Score = 0;
        }

        public void GivePlayerOnePoint()
        {
            m_Score++;
        }
    }
}
