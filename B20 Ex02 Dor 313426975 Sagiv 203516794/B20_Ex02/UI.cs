using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Text;

namespace B20_Ex02
{

    public class UI
    {
        private static readonly int sr_PageWidth = 50;
        private static readonly int sr_SpaceForSingleCubeCols = 4;
        private static readonly int sr_SpaceForSingleCubeRows = 2;
        private static readonly int sr_SpacesBetweenCoordinatesAndBoardAndEdges = 3;
        private static readonly int r_LogRows = 6;
        private static readonly int r_LogCols = 6;
        private static readonly int r_PhyRows = r_LogRows * sr_SpaceForSingleCubeRows + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
        private static readonly int r_PhyCols = r_LogCols* sr_SpaceForSingleCubeCols + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
        private static readonly char r_SignOfPlaceForGameIcon = 'S';
        private static readonly char[,] m_PresentationBoard;
        internal static readonly List<char> r_RowSymbol;
        internal static readonly List<char> r_ColSymbol;
        private List<char> m_IconSymbol;

        static UI()
        {
            UI.printSign("Welcome To Dor's World");
            m_PresentationBoard = new char[r_PhyRows, r_PhyCols];
            r_RowSymbol = new List<char>(6);
            r_ColSymbol = new List<char>(6);
            makeRowColSymbol();
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
            Player playerTwo = playerTwoLogin(playerOne.NameOfPlayer);
            bool v_WantToPlayAnotherGame ;

            do
            {
                GameBoard board = makeGameBoard(playerOne.NameOfPlayer);

                makeRandSymbolListOfIconAccordingToSizeOfBoard(board.NumOfRows, board.NumOfCols);
                printBoard(board);
                gameRoutineAndKeepScore(board, playerOne, playerTwo);
                announceOnTheWinner(playerOne,playerTwo);
                v_WantToPlayAnotherGame = askForAnotherGame(playerOne, playerTwo);

                m_IconSymbol.Clear();
            } while (v_WantToPlayAnotherGame == true) ;
        }

        private Player playerOneLogin()
        {
            string nameOfPlayerOne;

            printSign("Player One Login");
            UI.printPlayerLogin();
            nameOfPlayerOne = System.Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();

            return new Player(nameOfPlayerOne, false);
        }

        private Player playerTwoLogin(string io_NameOfPlayerOne)
        {
            string nameOfPlayerTwo = null;
            bool v_IsWantToPlayVsCompter;

            printSign("Player Two Login");
            v_IsWantToPlayVsCompter = ChoosingOfCompetitionForPlayerOne(io_NameOfPlayerOne);

            if (v_IsWantToPlayVsCompter == false)
            {
                UI.printPlayerLogin();
                nameOfPlayerTwo = System.Console.ReadLine();
            }

            Ex02.ConsoleUtils.Screen.Clear();

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
                v_ValidInput = CheckInput.IsValidPlayerOneEnemyChoice(playerChoice);
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
            bool v_SizeIsValid;

            printSign("Choosing Board Size");

            do
            {
                printChoseSizeOfBoard(io_NameOfPlayerOne);
                sizeOfBoard = System.Console.ReadLine();
                v_SizeIsValid = CheckInput.IsValidBoardSize(sizeOfBoard);
            } while (v_SizeIsValid == false);

            seperateSizeOfBoard = sizeOfBoard.Split(seperator, 2, StringSplitOptions.RemoveEmptyEntries);
            rowOfBoard = int.Parse(seperateSizeOfBoard[0]);
            colOfBoard = int.Parse(seperateSizeOfBoard[1]);

            Ex02.ConsoleUtils.Screen.Clear();

            return new GameBoard(rowOfBoard,colOfBoard);
        }

        private void gameRoutineAndKeepScore(GameBoard io_Board,Player io_PlayerOne,Player io_PlayerTwo)
        {
            eTurn playerTurn = eTurn.PlayerOne;

            while (io_Board.gameHasFinished() == false)
            {
                playerPlayHisTurn(io_Board, io_PlayerOne, io_PlayerTwo, playerTurn);
                switchTurn(ref playerTurn);
            }
        }

        private void playerPlayHisTurn(GameBoard io_Board, Player io_PlayerOne, Player io_PlayerTwo, eTurn i_PlayerTurn)
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
                io_Player.GivePlayerOnePoint();
            }
        }

        // Fix: check if chose move has chosen
        private string choseMove(GameBoard io_Board,Player io_Player)
        {
            string move;
            bool v_MoveIsValid;
            bool v_AlreadyExposed;
            bool v_ValidPlay;

            do
            {
                do
                {
                    printMakeAMove(io_Player, io_Board.NumOfRows, io_Board.NumOfCols);
                    move = System.Console.ReadLine();
                    exitIfQ(move);
                    v_MoveIsValid = CheckInput.IsValidMove(move, io_Board.NumOfRows, io_Board.NumOfCols);
                } while (v_MoveIsValid == false);

                int colChose = move[0] - 'A';
                int rowChose = move[1] - '1';
                v_AlreadyExposed = CheckInput.checkForExposedCube(io_Board.alreadyExposed(rowChose,colChose));
                v_ValidPlay = (v_AlreadyExposed == false && v_MoveIsValid == true);
            } while (v_ValidPlay == false);

            return move;
        }

        private int makeAndRepresentTheBoardWithMove(GameBoard io_Board, string io_move)
        {
            int colChose = io_move[0] - 'A';
            int rowChose = io_move[1] - '1';

            int symbolMove = io_Board.ExposeSymbolAndTakeValue(rowChose, colChose);

            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(io_Board);

            return symbolMove;
        }

        private void cancelLastMove(GameBoard io_Board, string io_FirstMove, string io_SecondMove)
        {
            int firstMoveColChose = io_FirstMove[0] - 'A';
            int firstMoveRowChose = io_FirstMove[1] - '1';
            int secondMoveColChose = io_SecondMove[0] - 'A';
            int secondMoveRowChose = io_SecondMove[1] - '1';

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

        private void announceOnTheWinner(Player io_PlayerOne, Player io_PlayerTwo)
        {
            string theWinner = "Both Of You";
            string winnerAnnouncment = string.Format("The Winner Is {0}", theWinner);
            string playerScorePresentation = string.Format("   {0} score is : {1} | {2} score is : {3}  ", io_PlayerOne.NameOfPlayer,io_PlayerOne.Score,io_PlayerTwo.NameOfPlayer,io_PlayerTwo.Score);

            printSign(playerScorePresentation);

            if (io_PlayerOne.Score > io_PlayerTwo.Score)
            {
                theWinner = io_PlayerOne.NameOfPlayer;
            }
            else if (io_PlayerOne.Score < io_PlayerTwo.Score)
            {
                theWinner = io_PlayerTwo.NameOfPlayer;
            }


            printSign(winnerAnnouncment);
        }

        private bool askForAnotherGame(Player io_PlayerOne, Player io_PlayerTwo)
        {
            string AnotherGameQuestion = string.Format(@"{0} And {1}
Do You Want To Play Another Game (Yes Or No)", io_PlayerOne, io_PlayerTwo);
            string playersAnswer;
            bool v_PlayerOneAndTwoIsDesicion;
            bool v_validAnswer;

            System.Console.WriteLine(AnotherGameQuestion);

            do
            {
                playersAnswer = System.Console.ReadLine();
                v_validAnswer = CheckInput.CheckValidAnswerForAnotherGameQuestion(playersAnswer);
            } while (v_validAnswer == false);

            v_PlayerOneAndTwoIsDesicion = (playersAnswer == "Yes" || playersAnswer == "yes");

            if (v_PlayerOneAndTwoIsDesicion == true)
            {
                Ex02.ConsoleUtils.Screen.Clear();
            }

            return v_PlayerOneAndTwoIsDesicion;
        }

        private static void printSign(string i_Title)
        {
            System.String firstAndLastLineOfRectangle = new System.String('-', sr_PageWidth);
            System.String spacesWithPlaceToEdgesOfRectangle = new System.String(' ', sr_PageWidth - 2);
            System.String oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle =
                new System.String(' ', (sr_PageWidth - 1 - (i_Title.Length)) / 2);
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

        private void printBoard(GameBoard io_Board)
        {
            int numOfRowsInCurrBoard = io_Board.NumOfRows * sr_SpaceForSingleCubeRows + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
            int numOfColsInCurrBoard = io_Board.NumOfCols * sr_SpaceForSingleCubeCols + sr_SpacesBetweenCoordinatesAndBoardAndEdges;

            for (int i = 0; i < numOfRowsInCurrBoard; i++)
            {
                for (int j = 0; j < numOfColsInCurrBoard; j++)
                {
                    if (m_PresentationBoard[i, j] == r_SignOfPlaceForGameIcon)
                    {
                        printIconInCube(getIconInGameBoard(io_Board, i, j));
                    }
                    else
                    {
                        System.Console.Write(m_PresentationBoard[i,j]);
                    }
                }

                System.Console.Write(Environment.NewLine);
            }
        }

        private void printIconInCube(int i_SymbolOfIcon)
        {
            System.Console.Write(m_IconSymbol[i_SymbolOfIcon]);
        }

        private void printMakeAMove(Player io_Player, int i_NumOfRows, int i_NumOfCols)
        {
            string msg = String.Format(@"{0} Make a Move:{1}", io_Player.NameOfPlayer,Environment.NewLine);

            System.Console.WriteLine(msg);
        }

        private int getIconInGameBoard(GameBoard io_Board, int io_CurrRowCoordinateOfPresentationBoard, int io_CurrColCoordinateOfPresentationBoard)
        {
            return io_Board.GetIconInCoordinate(io_CurrRowCoordinateOfPresentationBoard / sr_SpaceForSingleCubeRows - 1,
                io_CurrColCoordinateOfPresentationBoard / sr_SpaceForSingleCubeCols - 1);
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

        private static void makeRowColSymbol()
        {
            char startOfAlphabet = 'A';
            char startOfNumbers = '1';

            for (int i = 0; i < 6; i++)
            {
                r_ColSymbol.Add(startOfAlphabet);
                r_RowSymbol.Add(startOfNumbers);
                startOfAlphabet++;
                startOfNumbers++;
            }
        }

        private static void makePresentationBoard()
        {
            List<char>.Enumerator rowListEnumerator = (List<char>.Enumerator)r_RowSymbol.GetEnumerator();
            List<char>.Enumerator colListEnumerator = (List<char>.Enumerator)r_ColSymbol.GetEnumerator();

            rowListEnumerator.MoveNext();
            colListEnumerator.MoveNext();

            for (int i = 0; i < r_PhyRows; i++)
            {
                for (int j = 0; j < r_PhyCols; j++)
                {
                    if (i == 0 && j % sr_SpaceForSingleCubeCols == 0 && j > 1)
                    {
                        m_PresentationBoard[i, j] = colListEnumerator.Current;
                        colListEnumerator.MoveNext();
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && i > 1 && j == 0)
                    {
                        m_PresentationBoard[i, j] = rowListEnumerator.Current;
                        rowListEnumerator.MoveNext();
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 0 && j > 1 && i > 0)
                    {
                        m_PresentationBoard[i, j] = '=';
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && i > 1 && j % sr_SpaceForSingleCubeCols == 2)
                    {
                        m_PresentationBoard[i, j] = '|';
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && j % sr_SpaceForSingleCubeCols == 0 && i > 1 && j > 1)
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

        private void makeRandSymbolListOfIconAccordingToSizeOfBoard(int i_NumOfRows, int i_NumOfCols)
        {
            List<char> listOfAllPossibleIcon;
            int numOfCharMatchForIcon = '~' - '!';
            int numOfIconNeeded = i_NumOfCols * i_NumOfRows / 2;
            Random random = new Random();

            listOfAllPossibleIcon = makeListOfAllPossibleCharacters(numOfCharMatchForIcon);

            // plus one for space icon in symbol 0 (board return 0 if icon is hidden)
            m_IconSymbol = new List<char>(numOfIconNeeded + 1);
            m_IconSymbol.Add(' ');

            for (int i = 0; i < numOfIconNeeded; i++)
            {
                // range in ascii table with only symbol (more the max board size)
                int randomNumber = random.Next(0,listOfAllPossibleIcon.Count);
                m_IconSymbol.Add(listOfAllPossibleIcon[randomNumber]);
                listOfAllPossibleIcon.RemoveAt(randomNumber);
            }
        }

        private List<char> makeListOfAllPossibleCharacters(int i_NumOfCharMatchForIcon)
        {
            // range in ascii table with only symbol (more the max board size)
            List<char> allPossibleChar = new List<char>(i_NumOfCharMatchForIcon);
            char startOfPossibleCharForIcon = '!';

            for (int i = 0; i < i_NumOfCharMatchForIcon; i++)
            {
                allPossibleChar.Add(startOfPossibleCharForIcon);
                startOfPossibleCharForIcon++;
            }

            return allPossibleChar;
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