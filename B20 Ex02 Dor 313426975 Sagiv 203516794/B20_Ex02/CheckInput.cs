using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class CheckInput
    {

        // check
        //public static bool CheckValidCorrdinateInBoardGame(char io_Row, char io_Col)
        //{
         //   return UI.r_ColSymbol.Contains(io_Row) && UI.r_RowSymbol.Contains(io_Col);
        //}

        internal static bool IsValidPlayerOneEnemyChoice(string io_Choice)
        {
            bool v_ValidEnemyCoice = io_Choice == "1" || io_Choice == "0";

            if (v_ValidEnemyCoice == false)
            {
                System.Console.WriteLine("Your Choice Is Wrong Try Again And Please Make Sure You Choose 1 Or 0");
            }

            return v_ValidEnemyCoice;
        }
        internal static bool IsValidBoardSize(string io_BoardSize)
        {
            string[] seperateBoardSize = new string[io_BoardSize.Length];

            for (int i = 0; i < io_BoardSize.Length; i++)
            {
                seperateBoardSize[i] = io_BoardSize[i].ToString();
            }

            int rowOfBoard;
            int colOfBoard;
            bool v_IsSizeThree = (io_BoardSize.Length == 3);
            bool v_FirstNumValid;
            bool v_SeperatorXValid;
            bool v_TwoNumValid;
            bool v_FirstCharIsNumbers;
            bool v_LasriCharIsNumbers;
            bool v_IsEven;
            bool v_IsInOfRange;
            bool v_isValidBoardSize;

            if (v_IsSizeThree == true)
            {
                v_FirstNumValid = seperateBoardSize[0] == "4" || seperateBoardSize[0] == "5" ||
                                       seperateBoardSize[0] == "6";
                v_SeperatorXValid = seperateBoardSize[1] == "x";
                v_TwoNumValid = seperateBoardSize[2] == "4" || seperateBoardSize[2] == "5" ||
                                     seperateBoardSize[2] == "6";
                v_IsInOfRange = v_FirstNumValid == true && v_TwoNumValid == true;
                v_FirstCharIsNumbers = int.TryParse(seperateBoardSize[0], out rowOfBoard);
                v_LasriCharIsNumbers = int.TryParse(seperateBoardSize[2], out colOfBoard);
                v_IsEven = ((rowOfBoard * colOfBoard) % 2 == 0);
                v_isValidBoardSize = v_IsSizeThree && v_FirstNumValid && v_SeperatorXValid && v_TwoNumValid &&
                                          v_FirstCharIsNumbers && v_LasriCharIsNumbers && v_IsEven && v_IsInOfRange;
                if(v_isValidBoardSize == false)
                {
                    if (v_IsInOfRange == false)
                    {
                        System.Console.WriteLine("Out Of Range Chose Between 4 And 6");
                    }

                    if (v_SeperatorXValid == false)
                    {
                        System.Console.WriteLine("Your seperator Between The Numbers Is not Valid");
                    }

                    if (v_IsEven == false)
                    {
                        System.Console.WriteLine("You Need To Have An Even Numbers Of Square");
                    }
                }
            }
            else
            {
                printInputIsValid();
                v_isValidBoardSize = false;
            }

            return v_isValidBoardSize;
        }

        public static bool IsValidMove(string i_Move, int i_NumOfRows, int i_NumOfCols)
        {
            bool v_IsSize = (i_Move.Length == 2);
            bool v_FirstLetterIsvalid = false;
            bool v_SecondLetterIsvalid = false;

            if (v_IsSize == true)
            {
                v_FirstLetterIsvalid = (UI.r_ColSymbol.Contains(i_Move[0]));
                v_SecondLetterIsvalid = (UI.r_RowSymbol.Contains(i_Move[1]));

                if (v_FirstLetterIsvalid == false || v_SecondLetterIsvalid == false)
                {
                    System.Console.WriteLine("You Choice a Wrong Symbol of Row Or Col");
                }
            }
            else
            {
                printInputIsValid();
            }

            return v_IsSize && v_FirstLetterIsvalid && v_SecondLetterIsvalid;
        }

        public static bool IssueErrorMessageExposedCube(bool io_AlreadyExposed)
        {
            if (io_AlreadyExposed == true)
            {
                System.Console.WriteLine("This Cube Already Been Exposed Try Again");
            }

            return io_AlreadyExposed;
        }

        private static void printInputIsValid()
        {
            System.Console.WriteLine("Your Choice Is Wrong Try Again (invalid input)");
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
