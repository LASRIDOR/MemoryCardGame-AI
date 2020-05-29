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
        private Ai m_AiBrain;

        public Player.Ai AiBrain
        {
            get
            {
                return m_AiBrain;
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
            if (m_AiBrain == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public class Ai
        {
            private enum ePlayingStrategy
            {
                Zero_Move = 0,
                One_Move = 1,
                Two_Move = 2,
                Three_Move = 3
            }

            private Dictionary<int,string> m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch;
            private int m_PairsOnTable;
            private int m_NumOfRows;
            private int m_NumOfCols;
            private ePlayingStrategy m_MyNextKindMove;
            private List<string> m_ListCoordinateForSureWinsMove;
            private List<string> m_ListCoordinateExposedCard;

            public string MaikngFirstMove()
            {
                string firstMove;

                if (m_MyNextKindMove == ePlayingStrategy.Two_Move || m_MyNextKindMove == ePlayingStrategy.One_Move)
                {
                    firstMove = getCardForMove(false);
                }
                else if(m_MyNextKindMove == ePlayingStrategy.Zero_Move)
                {
                    firstMove = getCardForMove(true);
                }
                else // m_MyNextKindMove == ePlayingStrategy.Three_Move
                {
                   firstMove = m_ListCoordinateForSureWinsMove[0];
                    m_ListCoordinateForSureWinsMove.RemoveAt(0);
                }

                return firstMove;
            }

            public string MaikngSecondMove(string i_FirstMoveCardRevealed, int i_SymbolOfFirstMoveCardRevealed)
            {
                string secondMove;

                if (m_MyNextKindMove == ePlayingStrategy.Two_Move || m_MyNextKindMove == ePlayingStrategy.One_Move)
                {
                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_SymbolOfFirstMoveCardRevealed) == true)
                    {
                        secondMove = m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch[i_SymbolOfFirstMoveCardRevealed];
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_SymbolOfFirstMoveCardRevealed);
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
                    }
                }
                else if(m_MyNextKindMove == ePlayingStrategy.Zero_Move)
                {
                    secondMove = getCardForMove(true);
                }
                else// m_MyNextKindMove == ePlayingStrategy.Three_Move
                {
                    secondMove = m_ListCoordinateForSureWinsMove[0];
                    m_ListCoordinateForSureWinsMove.RemoveAt(0);
                }

                return secondMove;
            }

            private string getCardForMove(bool i_NeedKnownCard)
            {
                string move;
                bool v_FoundMove = false;
                Random rand = new Random();

                do
                {
                    int randRowCoordinate = rand.Next(0, m_NumOfRows);
                    int randColCoordinate = rand.Next(0, m_NumOfCols);

                    move = UI.r_ColSymbol[randColCoordinate].ToString() + UI.r_RowSymbol[randRowCoordinate].ToString();

                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(move) == i_NeedKnownCard)
                    {
                        if (m_ListCoordinateExposedCard.Contains(move) == false)
                        {
                            v_FoundMove = true;
                        }

                    }
                } while (v_FoundMove == false);

                return move;
            }

            public void cardsRevealed(string i_FirstMove, int i_FirstMoveSymbol, string i_SecondMove, int i_SecondMoveSymbol)
            {
                if (i_FirstMoveSymbol == i_SecondMoveSymbol)
                {
                    m_ListCoordinateExposedCard.Add(i_FirstMove);
                    m_ListCoordinateExposedCard.Add(i_SecondMove);
                    m_ListCoordinateForSureWinsMove.Remove(i_SecondMove);
                    m_ListCoordinateForSureWinsMove.Remove(i_FirstMove);
                    m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_FirstMoveSymbol);


                    calculateNextMove();
                }
                else
                {

                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_FirstMoveSymbol) == true)
                    {
                        if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(i_FirstMove) == false)
                        {
                            m_ListCoordinateForSureWinsMove.Add(
                                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch[i_FirstMoveSymbol]);
                            m_ListCoordinateForSureWinsMove.Add(i_FirstMove);
                            m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_FirstMoveSymbol);
                            m_MyNextKindMove = ePlayingStrategy.Three_Move;
                        }
                    }
                    else
                    {
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Add(i_FirstMoveSymbol, i_FirstMove);
                        calculateNextMove();
                    }


                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_SecondMoveSymbol) == true)
                    {
                        if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(i_SecondMove) == false)
                        {
                            m_ListCoordinateForSureWinsMove.Add(
                                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch[i_SecondMoveSymbol]);
                            m_ListCoordinateForSureWinsMove.Add(i_SecondMove);
                            m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_SecondMoveSymbol);
                            m_MyNextKindMove = ePlayingStrategy.Three_Move;
                        }
                    }
                    else
                    {
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Add(i_SecondMoveSymbol, i_SecondMove);
                        calculateNextMove();
                    }
                }
            }

            private void calculateNextMove()
            {
                if (m_ListCoordinateForSureWinsMove.Count > 0)
                {
                    m_MyNextKindMove = ePlayingStrategy.Three_Move;
                }
                else
                {
                    if ((m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count == 0 ||
                         (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 1))
                    {
                        m_MyNextKindMove = ePlayingStrategy.Two_Move;
                    }
                    else if ((m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count >= 1 &&
                         (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 0))
                    {
                        m_MyNextKindMove = ePlayingStrategy.One_Move;
                    }
                    else if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count >=
                             2 * (m_PairsOnTable + 1) / 3 &&
                             (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.Zero_Move;
                    }

                    if (m_PairsOnTable == 6 && m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.One_Move;
                    }
                }
            }

            public void resetMermory(int i_NumOfRow, int i_NumOfCol)
            {
                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch = new Dictionary<int, string>(i_NumOfRow*i_NumOfCol);
                m_ListCoordinateForSureWinsMove = new List<string>(i_NumOfRow * i_NumOfCol);
                m_ListCoordinateExposedCard = new List<string>(i_NumOfRow * i_NumOfCol);
                m_PairsOnTable = i_NumOfRow * i_NumOfCol;
                m_MyNextKindMove = ePlayingStrategy.Two_Move;
                m_NumOfRows = i_NumOfRow;
                m_NumOfCols = i_NumOfCol;
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
            if (m_AiBrain != null)
            {
                AiBrain.resetMermory(i_NumOfRow.Value, i_NumOfCol.Value);
            }
            m_Score = 0;
        }

        public void GivePlayerOnePoint()
        {
            m_Score++;
        }
    }
}
