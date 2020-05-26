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
        public bool CheckValidCorrdinateInBoardGame(char io_Row, char io_Col)
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

            for(int i = 0 ; i < io_BoardSize.Length;i++)
            {
                seperateBoardSize[i] = io_BoardSize[i].ToString();
            }

            int rowOfBoard;
            int colOfBoard;

            bool v_IsSizeThree = seperateBoardSize.Length == 3;
            bool v_FirstNumValid = seperateBoardSize[0] == "4" || seperateBoardSize[0] == "5" || seperateBoardSize[0] == "6" ;
            bool v_SeperatorXValid = seperateBoardSize[1] == "x";
            bool v_TwoNumValid = seperateBoardSize[2] == "4" || seperateBoardSize[2] == "5" || seperateBoardSize[2] == "6";
            bool v_FirstCharIsNumbers = int.TryParse(seperateBoardSize[0],out rowOfBoard);
            bool v_LasriCharIsNumbers = int.TryParse(seperateBoardSize[2], out colOfBoard);
            bool v_IsEven = ((rowOfBoard * colOfBoard) % 2 == 0);

            return v_IsSizeThree && v_FirstNumValid && v_SeperatorXValid && v_TwoNumValid && v_IsEven;
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
        public static bool CheckValidAnswerForAnotherGameQuestion(string i_PlayersAnswer)
        {
            bool v_validAnswer = i_PlayersAnswer == "Yes" || i_PlayersAnswer == "yes" || i_PlayersAnswer == "No" || i_PlayersAnswer == "no";

            if (v_validAnswer == false)
            {
                System.Console.WriteLine("Didn't Understand What You are saying try Again (answer can be only yes or no)");
            }

            return v_validAnswer;
        }
    }
}
