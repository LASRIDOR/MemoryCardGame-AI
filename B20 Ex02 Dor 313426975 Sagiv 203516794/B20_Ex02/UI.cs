using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;

namespace B20_Ex02
{

    public class UI
    {
        private readonly int r_MaxNumOfRow = 15;
        private readonly int r_MaxNumOfCol = 27;
        private readonly char r_SignOfPlaceForGameIcon = 'S';
        private char[,] m_PresentationBoard;
        private readonly List<char> r_RowSymbol;
        private readonly List<char> r_ColSymbol;
        private readonly List<char> r_IconSymbol;

        // make func to inc score and reset when game ends;
        private static int s_ScoreForPlayerOne;
        private static int s_ScoreForPlayerTwo;


        public UI()
        {
            makePresentationBoard();
        }

        public enum eTurn
        {
            PlayerOne = 1,
            PlayerTwo = 2
        }

        /*
        private static readonly string    sr_SignOfPlaceForGameIcon = "Place For Icon";
        private static readonly int       sr_SpaceForSingleCubeCols = 4;
        private static readonly int       sr_SpaceForSingleCubeRows = 2;
        private static readonly int       sr_SpacesBetweenCoordinatesAndBoardAndEdges = 3;
        private readonly int              r_NumOfRowInBoard;
        private readonly int              r_NumOfColsInBoard;
        private static string[,]          m_Board;
        */
        /*
        UI(int i_NumOfRowInBoard,int i_NumOfColInBoard)
        {
            r_NumOfRowInBoard = i_NumOfRowInBoard * sr_SpaceForSingleCubeRows + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
            r_NumOfColsInBoard = i_NumOfColInBoard * sr_SpaceForSingleCubeCols+ sr_SpacesBetweenCoordinatesAndBoardAndEdges;
            m_Board = new string[r_NumOfRowInBoard,r_NumOfColsInBoard];
            buildBoard();
        }
        */
        public void PlayMatchGame()
        {
            Player playerOne = playerOneLogin();
            Player playerTwo = playerTwoLogin(playerOne.nameOfPlayer);
            bool v_WantToPlay = true;

            while (v_WantToPlay == true)
            {
                GameBoard board = makeGameBoard(playerOne.nameOfPlayer);
                printBoard(board);

                // create another func for playing routinefor this below while game has not finished and keep score
                eTurn playerTurn = eTurn.PlayerOne;
                playerMakeAMoveInTurn(board, playerOne, playerTwo, playerTurn);
                switchTurn(ref playerTurn);



                r_IconSymbol.Clear();
            }
        }

        private Player playerOneLogin()
        {
            string nameOfPlayerOne;

            UI.printWelcomeSign("Welcome To Dor's World");
            UI.printPlayerLogin();
            nameOfPlayerOne = System.Console.ReadLine();

            return new Player(nameOfPlayerOne, false);
        }

        private Player playerTwoLogin(string io_NameOfPlayerOne)
        {
            string nameOfPlayerTwo = null;
            bool v_IsWantToPlayVsCompter;

            v_IsWantToPlayVsCompter = ChoosingOfCompetitionForPlayerOne(io_NameOfPlayerOne);

            if (v_IsWantToPlayVsCompter == false)
            {
                UI.printPlayerLogin();
                nameOfPlayerTwo = System.Console.ReadLine();
            }

            return new Player(nameOfPlayerTwo, v_IsWantToPlayVsCompter);
        }

        private bool ChoosingOfCompetitionForPlayerOne(string io_NameOfPlayerOne)
        {
            string playerChoice;
            bool v_ValidInput;

            do
            {
                UI.printChoosingOfCompetitionForPlayerOne(io_NameOfPlayerOne);
                playerChoice = System.Console.ReadLine();
                v_ValidInput = CheckInput.PlayerEnemyChoice(playerChoice);
            } while (v_ValidInput == false) ;

            return (playerChoice == "1");
        }

        private GameBoard makeGameBoard(string io_NameOfPlayerOne)
        {
            string sizeOfBoard;
            string[] seperator = {"x"};
            string[] seperateSizeOfBoard;
            int rowOfBoard;
            int colOfBoard;

            printChoseSizeOfBoard(io_NameOfPlayerOne);
            sizeOfBoard = System.Console.ReadLine();
            seperateSizeOfBoard = sizeOfBoard.Split(seperator,2,StringSplitOptions.RemoveEmptyEntries);
            rowOfBoard = int.Parse(seperateSizeOfBoard[0]);
            colOfBoard = int.Parse(seperateSizeOfBoard[1]);

            return new GameBoard(rowOfBoard,colOfBoard);
        }
        private void makePresentationBoard()
        {
            char startOfAlphabet = 'A';
            char startOfNumbers = '1';


            for (int i = 0; i < r_MaxNumOfRow; i++)
            {
                for (int j = 0; j < r_MaxNumOfCol; j++)
                {
                    if (i == 0 && j % 4 == 0 && j > 1)
                    {
                        m_PresentationBoard[i, j] = startOfAlphabet;
                        startOfAlphabet++;
                    }
                    else if (i % 2 == 1 && i > 1 && j == 0)
                    {
                        m_PresentationBoard[i, j] = startOfNumbers;
                        startOfNumbers++;
                    }
                    else if (i % 2 == 0 && j > 1 && i > 0)
                    {
                        m_PresentationBoard[i, j] = '=';
                    }
                    else if (i % 2 == 1 && i > 1 && j % 4 == 2)
                    {
                        m_PresentationBoard[i, j] = '|';
                    }
                    else if (i % 2 == 1 && j % 4 == 0 && i > 1 && j > 1)
                    {
                        m_PresentationBoard[i, j] = r_SignOfPlaceForGameIcon;
                    }
                    else
                    {
                        m_PresentationBoard[i, j] = ' ';
                    }
                }
            }
        }

        private void playerMakeAMoveInTurn(GameBoard io_Board, Player io_PlayerOne, Player io_PlayerTwo, eTurn i_PlayerTurn)
        {
            if (i_PlayerTurn == eTurn.PlayerOne)
            {
                humanMove(io_Board, io_PlayerOne);
            }
            else
            {
                if (io_PlayerTwo.isAi() == true)
                {
                    aiMove(io_Board, io_PlayerTwo);
                }
                else
                {
                    humanMove(io_Board, io_PlayerTwo);
                }
            }
        }

        private void humanMove(GameBoard io_Board, Player io_Player)
        {
            string firstMove = choseMove(io_Board, io_Player);
            int symbolOfFirstMove;

            symbolOfFirstMove = makeAndRepresentTheBoardWithMove(io_Board, firstMove);

            string secondMove = choseMove(io_Board, io_Player);
            int symbolOfSecondMove;

            symbolOfSecondMove = makeAndRepresentTheBoardWithMove(io_Board, secondMove);

            if (symbolOfSecondMove != symbolOfFirstMove)
            {
                System.Threading.Thread.Sleep(200);
                cancelLastMove(io_Board, firstMove, secondMove);
            }
            else
            {
                givePlayerAscore
            }
        }

        private string choseMove(GameBoard io_Board,Player io_Player)
        {
            string move;
            bool v_MoveIsValid = true;

            do
            {
                printMakeAMove(io_Player, io_Board.NumOfRows, io_Board.NumOfCols);
                move = System.Console.ReadLine();
                exitIfQ(move);
                v_MoveIsValid = CheckInput.CheckValidMove(move, io_Board.NumOfRows, io_Board.NumOfCols);
            } while (v_MoveIsValid == false);

                return move;
        }

        private int makeAndRepresentTheBoardWithMove(GameBoard io_Board, string io_move)
        {
            int rowChose = io_move[0] - 'A';
            int colChose = io_move[1] - '1';
            int symbolMove = io_Board.ExposeSymbolAndTakeValue(rowChose, colChose);

            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(io_Board);

            return symbolMove;
        }

        private void cancelLastMove(GameBoard io_Board, string io_FirstMove, string io_SecondMove)
        {
            int firstMoveRowChose = io_FirstMove[0] - 'A';
            int firstMoveColChose = io_FirstMove[1] - '1';
            int secondMoveRowChose = io_SecondMove[0] - 'A';
            int secondMoveColChose = io_SecondMove[1] - '1';

            io_Board.HideIcon(firstMoveRowChose,firstMoveColChose);
            io_Board.HideIcon(secondMoveRowChose, secondMoveColChose);
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(io_Board);
        }

        private void aiMove(GameBoard io_Board, Player io_Player)
        {
            string firstMove = io_Player.MakeFirstMove();
            int symbolOfFirstMove;

            printComputerMakingAMove();
            symbolOfFirstMove = makeAndRepresentTheBoardWithMove(io_Board, firstMove);
            System.Threading.Thread.Sleep(200);

            string secondMove = io_Player.MakeSecondMove(symbolOfFirstMove);
            int symbolOfSecondMove;

            printComputerMakingAMove();
            symbolOfSecondMove = makeAndRepresentTheBoardWithMove(io_Board, secondMove);
            System.Threading.Thread.Sleep(200);

            if (symbolOfSecondMove != symbolOfFirstMove)
            {
                System.Threading.Thread.Sleep(200);
                cancelLastMove(io_Board, firstMove, secondMove);
            }
        }

        private void switchTurn(ref eTurn o_PlayerCurrentTurn)
        {
            if (o_PlayerCurrentTurn == eTurn.PlayerOne)
            {
                o_PlayerCurrentTurn = eTurn.PlayerTwo;
            }
            else
            {
                o_PlayerCurrentTurn = eTurn.PlayerOne;
            }
        }

        private static void printWelcomeSign(string i_Title)
        {
            System.String firstAndLastLineOfRectangle = new System.String('-', sr_PageWidth);
            System.String spacesWithPlaceToEdgesOfRectangle = new System.String(' ', sr_PageWidth - 2);
            System.String oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle =
                new System.String(' ', (sr_PageWidth - 2 - (i_Title.Length)) / 2);
            System.String middleOfRectangle = System.String.Format("|{0}|", spacesWithPlaceToEdgesOfRectangle);
            System.String middleOfRectangleTitleLine = System.String.Format("|{0}{1}{0}|",
                oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle, i_Title);

            System.Console.WriteLine(firstAndLastLineOfRectangle);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(middleOfRectangleTitleLine);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(firstAndLastLineOfRectangle);

        }

        private static void printPlayerLogin()
        {
            System.Console.WriteLine("Please Enter Your Name: ");
            System.Console.Write(Environment.NewLine);
        }

        private static void printChoosingOfCompetitionForPlayerOne(string io_NameOfPlayerOne)
        {
            string msg = String.Format(@"{0} Press 1 If You Want To Play Against The Computer(AI) And 0 If You Want To Play Against Another Player",
                    io_NameOfPlayerOne);

            System.Console.WriteLine(msg);
        }

        private static void printChoseSizeOfBoard(string io_NameOfPlayerOne)
        {
            string msg = String.Format(@"{0} I need you to determine the size of the board
(Max size is 6 & Min size is 4 & make sure you enter an even number) Example of Input: 6x4", io_NameOfPlayerOne);

            System.Console.WriteLine(msg);
        }

        private void printBoard(GameBoard io_board)
        {
            int indexRow = 0;
            int indexCol = 0;
            foreach (char c in m_PresentationBoard)
            {
                if (c == r_SignOfPlaceForGameIcon)
                {
                    if (indexCol == io_board.NumOfCols)
                    {
                        indexRow++;
                        indexCol = 0;
                    }

                    printIconInCube(io_board.Board[indexRow,indexCol].SymbolOfIcon);
                    indexCol++;
                }
                else
                {
                    System.Console.Write(c);
                }
            }
        }

        private void printIconInCube(int i_SymbolOfIcon)
        {
            System.Console.WriteLine(r_IconSymbol[i_SymbolOfIcon]);
        }

        private void printMakeAMove(Player io_Player, int i_NumOfRows, int i_NumOfCols)
        {
            string msg = String.Format(@"{0} Make a Move:{1}", io_Player.nameOfPlayer,Environment.NewLine);

            System.Console.WriteLine();
        }

        private void printComputerMakingAMove()
        {
            System.Console.WriteLine("RoboMatch Making His Move");
            System.Threading.Thread.Sleep(50);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(50);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(50);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(50);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(50);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(50);
        }

        private void exitIfQ(string i_Move)
        {
            if (i_Move == "Q")
            {
                Environment.Exit(1);
            }
        }
        /*
        public static void PrintBoard(BoardGame io_Board)
        {
            System.Collections.IEnumerator boardIterator = m_Board.GetEnumerator();
            int i = 0;
            int j = 0;

            foreach (string s in m_Board)
            {
                if (s == sr_SignOfPlaceForGameIcon)
                {
                    System.Console.WriteLine(io_Board.Board[i, j].Icon.ToString());
                }
                else
                {
                    System.Console.WriteLine(s);
                }
            }
        }

        private void buildBoard()
        {
            char startOfAlphabet = 'A';
            char startOfNumbers = '1';


            for (int i = 0; i < r_NumOfRowInBoard; i++)
            {
                for (int j = 0; j < r_NumOfColsInBoard; j++)
                {
                    if (i == 0 && j % 4 == 0 && j > 1)
                    {
                        m_Board[i, j] = startOfAlphabet.ToString();
                        startOfAlphabet++;
                    }
                    else if (i % 2 == 1 && i > 1 && j == 0)
                    {
                        m_Board[i, j] = startOfNumbers.ToString();
                        startOfNumbers++;
                    }
                    else if (i % 2 == 0 && j > 1 && i > 0)
                    {
                        m_Board[i, j] = "=";
                    }
                    else if (i % 2 == 1 && i > 1 && j % 4 == 2)
                    {
                        m_Board[i, j] = "|";
                    }
                    else if(i % 2 == 1 && j % 4 == 0 && i > 1 && j > 1)
                    {
                        m_Board[i,j] = sr_SignOfPlaceForGameIcon;
                    }
                    else
                    {
                        m_Board[i, j] = " ";
                    }
                }
            }
        }
    }
}
*/
    }
}