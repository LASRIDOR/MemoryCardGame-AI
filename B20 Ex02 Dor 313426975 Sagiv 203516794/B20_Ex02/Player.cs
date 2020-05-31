using System;
using System.Collections.Generic;

namespace B20_Ex02
{
    public class Player
    {
        private readonly string r_NameOfPlayer;
        private int m_Score;
        private readonly Ai m_AiBrain;

        public Ai AiBrain
        {
            get { return m_AiBrain;}
        }

        public Player(string i_NameOfPlayer, bool isAi)
        {
            if (isAi == true)
            {
                r_NameOfPlayer = "RoboMatch";
                m_AiBrain = new Ai();
            }
            else
            {
                r_NameOfPlayer = i_NameOfPlayer;
                m_AiBrain = null;
            }
        }

        public string NameOfPlayer
        {
            get { return r_NameOfPlayer; }
        }

        public int Score
        {
            get { return m_Score; }
        }

        public bool IsAi()
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
                ZeroMove = 0,
                OneMove = 1,
                TwoMove = 2,
                ThreeMove = 3
            }

            private Dictionary<int, Coordinate> m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch;
            private int m_PairsOnTable;
            private int m_NumOfRows;
            private int m_NumOfCols;
            private ePlayingStrategy m_MyNextKindMove;
            private List<Coordinate> m_ListCoordinateForSureWinsMove;
            private List<Coordinate> m_ListCoordinateExposedCard;

            public Coordinate MakingFirstMove()
            {
                Coordinate firstMove;

                if (m_MyNextKindMove == ePlayingStrategy.TwoMove || m_MyNextKindMove == ePlayingStrategy.OneMove)
                {
                    firstMove = getCardForMove(false);
                }
                else if (m_MyNextKindMove == ePlayingStrategy.ZeroMove)
                {
                    firstMove = getCardForMove(true);
                }
                else
                {
                    firstMove = m_ListCoordinateForSureWinsMove[0];
                    m_ListCoordinateForSureWinsMove.RemoveAt(0);
                }

                return firstMove;
            }

            public Coordinate MakingSecondMove(int i_SymbolOfFirstMoveCardRevealed)
            {
                Coordinate secondMove;

                if (m_MyNextKindMove == ePlayingStrategy.TwoMove || m_MyNextKindMove == ePlayingStrategy.OneMove)
                {
                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_SymbolOfFirstMoveCardRevealed) == true)
                    {
                        secondMove = m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch[i_SymbolOfFirstMoveCardRevealed];
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_SymbolOfFirstMoveCardRevealed);
                    }
                    else
                    {
                        if (m_MyNextKindMove == ePlayingStrategy.TwoMove)
                        {
                            secondMove = getCardForMove(false);
                        }
                        else
                        {
                            secondMove = getCardForMove(true);
                        }
                    }
                }
                else if(m_MyNextKindMove == ePlayingStrategy.ZeroMove)
                {
                    secondMove = getCardForMove(true);
                }
                else
                {
                    secondMove = m_ListCoordinateForSureWinsMove[0];
                    m_ListCoordinateForSureWinsMove.RemoveAt(0);
                }

                return secondMove;
            }

            private Coordinate getCardForMove(bool i_NeedKnownCard)
            {
                Coordinate move;
                bool v_FoundMove = false;
                Random rand = new Random();

                do
                {
                    int randRowCoordinate = rand.Next(0, m_NumOfRows);
                    int randColCoordinate = rand.Next(0, m_NumOfCols);

                    move = new Coordinate(UI.sr_ColSymbol[randColCoordinate].ToString() + UI.sr_RowSymbol[randRowCoordinate].ToString());

                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(move) == i_NeedKnownCard)
                    {
                        if (m_ListCoordinateExposedCard.Contains(move) == false)
                        {
                            v_FoundMove = true;
                        }
                    }
                }
                while (v_FoundMove == false);

                return move;
            }

            public void CardsRevealed(Coordinate i_FirstMoveCoordinate, int i_FirstMoveSymbol, Coordinate i_SecondMoveCoordinate, int i_SecondMoveSymbol)
            {
                if (i_FirstMoveSymbol == i_SecondMoveSymbol)
                {
                    addAndRemoveIfExistMatchCard(i_FirstMoveCoordinate, i_SecondMoveCoordinate, i_FirstMoveSymbol);
                    calculateNextMove();
                }
                else
                {
                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_FirstMoveSymbol) == true)
                    {
                        if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(i_FirstMoveCoordinate) == false)
                        {
                            addToSureWinListKnownCard(i_FirstMoveSymbol, i_FirstMoveCoordinate);
                        }
                    }
                    else
                    {
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Add(i_FirstMoveSymbol, i_FirstMoveCoordinate);
                        calculateNextMove();
                    }

                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsKey(i_SecondMoveSymbol) == true)
                    {
                        if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.ContainsValue(i_SecondMoveCoordinate) == false)
                        {
                            addToSureWinListKnownCard(i_SecondMoveSymbol, i_SecondMoveCoordinate);
                        }
                    }
                    else
                    {
                        m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Add(i_SecondMoveSymbol, i_SecondMoveCoordinate);
                        calculateNextMove();
                    }
                }
            }

            private void addAndRemoveIfExistMatchCard(Coordinate i_FirstMoveCoordinate, Coordinate i_SecondMoveCoordinate, int i_SymbolOfMoves)
            {
                m_ListCoordinateExposedCard.Add(i_FirstMoveCoordinate);
                m_ListCoordinateExposedCard.Add(i_SecondMoveCoordinate);
                m_ListCoordinateForSureWinsMove.Remove(i_FirstMoveCoordinate);
                m_ListCoordinateForSureWinsMove.Remove(i_SecondMoveCoordinate);
                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_SymbolOfMoves);
            }

            private void addToSureWinListKnownCard(int i_MoveSymbol, Coordinate i_MoveCoordinate)
            {
                m_ListCoordinateForSureWinsMove.Add(m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch[i_MoveSymbol]);
                m_ListCoordinateForSureWinsMove.Add(i_MoveCoordinate);
                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Remove(i_MoveSymbol);
                m_MyNextKindMove = ePlayingStrategy.ThreeMove;
            }

            private void calculateNextMove()
            {
                if (m_ListCoordinateForSureWinsMove.Count > 0)
                {
                    m_MyNextKindMove = ePlayingStrategy.ThreeMove;
                }
                else
                {
                    if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count == 0 ||
                         (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.TwoMove;
                    }
                    else if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count >= 1 &&
                         (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 0)
                    {
                        m_MyNextKindMove = ePlayingStrategy.OneMove;
                    }
                    else if (m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count >=
                             2 * (m_PairsOnTable + 1) / 3 &&
                             (m_PairsOnTable + m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count) % 2 == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.ZeroMove;
                    }

                    if (m_PairsOnTable == 6 && m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch.Count == 1)
                    {
                        m_MyNextKindMove = ePlayingStrategy.OneMove;
                    }
                }
            }

            public void ResetMermory(int i_NumOfRow, int i_NumOfCol)
            {
                m_DictionaryMemoryOfUnrevealedKnownCardWithoutMatch = new Dictionary<int, Coordinate>(i_NumOfRow * i_NumOfCol);
                m_ListCoordinateForSureWinsMove = new List<Coordinate>(i_NumOfRow * i_NumOfCol);
                m_ListCoordinateExposedCard = new List<Coordinate>(i_NumOfRow * i_NumOfCol);
                m_PairsOnTable = i_NumOfRow * i_NumOfCol;
                m_MyNextKindMove = ePlayingStrategy.TwoMove;
                m_NumOfRows = i_NumOfRow;
                m_NumOfCols = i_NumOfCol;
            }
        }

        public void NewGame(int? i_NumOfRow, int? i_NumOfCol)
        {
            if (m_AiBrain != null)
            {
                AiBrain.ResetMermory(i_NumOfRow.Value, i_NumOfCol.Value);
            }

            m_Score = 0;
        }

        public void GivePlayerOnePoint()
        {
            m_Score++;
        }
    }
}
