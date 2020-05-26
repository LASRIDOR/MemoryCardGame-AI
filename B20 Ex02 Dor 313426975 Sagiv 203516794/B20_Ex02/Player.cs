using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Player
    {
        private readonly string m_NameOfPlayer;
        private int m_Score;
        private Nullable<Ai> m_Ai;

        public Player(string i_NameOfPlayer,bool isAi)
        {
            if (isAi == true)
            {
                m_NameOfPlayer = "RoboMatch";
                m_Ai = new Ai();
            }
            else
            {
                m_NameOfPlayer = i_NameOfPlayer;
                m_Ai = null;
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

        private struct Ai
        {
            private int memoryOfCardsInTime;
            private int counterToRenewMemory;

        }

        // fix
        public string MakeFirstMove()
        {
            string move = null;

            return move;
        }

        // fix
        public string MakeSecondMove(int i_symbolOfFirstMove)
        {
            string move = null;

            return move;
        }

        public void NewGame()
        {
            m_Score = 0;
        }

        public void GivePlayerOnePoint()
        {
            m_Score++;
        }
    }
}
