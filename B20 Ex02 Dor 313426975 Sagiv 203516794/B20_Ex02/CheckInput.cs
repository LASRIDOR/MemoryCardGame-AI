using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    class CheckInput
    {
        private static readonly List<char> r_RowSymbolList;
        private static readonly List<char> r_ColSymbolList;

        // check
        public bool CheckValidCorrdinateInBoardGame(T io_Row, T io_Col)
        {
            return r_RowSymbolList.Contains(io_Row) && r_ColSymbolList.Contains(io_Col);
        }

        public static bool PlayerEnemyChoice(string io_Choice)
        {
            return io_Choice == "1" || io_Choice == "0";
        }
        public static bool BoardSize(string io_BoardSize)
        {
            string[] seperateBoardSize = new string[io_BoardSize.Length];
            int i = 0;

            while(io_BoardSize != null)
            {
                seperateBoardSize.SetValue(io_BoardSize.Remove(0, 1),i);
                i++;
            }

            bool v_IsSizeThree = seperateBoardSize.Length == 3;
            bool v_FirstNumValid = seperateBoardSize[0] == "4" || seperateBoardSize[0] == "6" ;
            bool v_SeperatorXValid = seperateBoardSize[1] == "x";
            bool v_TwoNumValid = seperateBoardSize[2] == "4" || seperateBoardSize[2] == "6";

            return v_IsSizeThree && v_FirstNumValid && v_SeperatorXValid && v_TwoNumValid;
        }

        public static bool CheckValidMove(string i_Move, int i_NumOfRows, int i_NumOfCols)
        {
            bool v_IsSize = (i_Move.Length == 2);
            bool v_FirstLetterIsvalid = i_Move[0] >= r_ColSymbolList[0] && i_Move[0] <= r_ColSymbolList[i_NumOfCols];
            bool v_SecondLetterIsvalid = i_Move[1] >= r_RowSymbolList[0] && i_Move[1] <= r_RowSymbolList[i_NumOfRows];

            return v_IsSize && v_FirstLetterIsvalid && v_SecondLetterIsvalid;
        }
        private static void inputIsValid()
        {
            System.Console.WriteLine("Invalid Input Please Try Again");
        }
    }
}
