using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class Player
    {
        private readonly string m_nameOfPlayer;
        private Nullable<Ai> m_Ai;

        public Player(string i_NameOfPlayer,bool isAi)
        {
            if (isAi == true)
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

        public string nameOfPlayer
        {
            get { return m_nameOfPlayer; }
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
    }
}
