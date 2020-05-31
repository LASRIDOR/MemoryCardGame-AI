﻿using System;
using System.Collections.Generic;

namespace B20_Ex02
{
    public class UI
    {
        private static readonly int sr_PageWidth = 50;
        internal static readonly int sr_SpaceForSingleCubeCols = 4;
        internal static readonly int sr_SpaceForSingleCubeRows = 2;
        private static readonly int sr_SpacesBetweenCoordinatesAndBoardAndEdges = 3;
        private static readonly int sr_LogRows = 6;
        private static readonly int sr_LogCols = 6;
        private static readonly int sr_PhyRows = (sr_LogRows * sr_SpaceForSingleCubeRows) + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
        private static readonly int sr_PhyCols = (sr_LogCols * sr_SpaceForSingleCubeCols) + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
        private static readonly char sr_SignOfPlaceForGameIcon = 'S';
        private static readonly char[,] sr_PresentationBoard;
        internal static readonly List<char> sr_RowSymbol;
        internal static readonly List<char> sr_ColSymbol;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private List<char> m_IconSymbol;

        static UI()
        {
            sr_PresentationBoard = new char[sr_PhyRows, sr_PhyCols];
            sr_RowSymbol = new List<char>(6);
            sr_ColSymbol = new List<char>(6);
            makeRowColSymbol();
            makePresentationBoard();
        }

        private static void makeRowColSymbol()
        {
            char startOfAlphabet = 'A';
            char startOfNumbers = '1';

            for (int i = 0; i < 6; i++)
            {
                sr_ColSymbol.Add(startOfAlphabet);
                sr_RowSymbol.Add(startOfNumbers);
                startOfAlphabet++;
                startOfNumbers++;
            }
        }

        private static void makePresentationBoard()
        {
            List<char>.Enumerator rowListEnumerator = sr_RowSymbol.GetEnumerator();
            List<char>.Enumerator colListEnumerator = sr_ColSymbol.GetEnumerator();

            rowListEnumerator.MoveNext();
            colListEnumerator.MoveNext();

            for (int i = 0; i < sr_PhyRows; i++)
            {
                for (int j = 0; j < sr_PhyCols; j++)
                {
                    if (i == 0 && j % sr_SpaceForSingleCubeCols == 0 && j > 1)
                    {
                        sr_PresentationBoard[i, j] = colListEnumerator.Current;
                        colListEnumerator.MoveNext();
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && i > 1 && j == 0)
                    {
                        sr_PresentationBoard[i, j] = rowListEnumerator.Current;
                        rowListEnumerator.MoveNext();
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 0 && j > 1 && i > 0)
                    {
                        sr_PresentationBoard[i, j] = '=';
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && i > 1 && j % sr_SpaceForSingleCubeCols == 2)
                    {
                        sr_PresentationBoard[i, j] = '|';
                    }
                    else if (i % sr_SpaceForSingleCubeRows == 1 && j % sr_SpaceForSingleCubeCols == 0 && i > 1 && j > 1)
                    {
                        sr_PresentationBoard[i, j] = sr_SignOfPlaceForGameIcon;
                    }
                    else
                    {
                        sr_PresentationBoard[i, j] = ' ';
                    }
                }
            }
        }

        private enum eTurn
        {
            PlayerOne = 1,
            PlayerTwo = 2
        }

        private enum eMoveNum
        {
            FirstMove = 1,
            SecondMove = 2
        }

        public void PlayMatchGame()
        {
            printSign("Welcome To Dori's World");
            m_PlayerOne = playerOneLogin();
            m_PlayerTwo = playerTwoLogin(m_PlayerOne.NameOfPlayer);
            bool v_WantToPlayAnotherGame;

            do
            {
                GameBoard board = makeGameBoard();

                m_PlayerOne.NewGame(null, null);
                m_PlayerTwo.NewGame(board.NumOfRows, board.NumOfCols);
                makeRandSymbolListOfIconAccordingToSizeOfBoard(board.NumOfRows, board.NumOfCols);
                printBoard(board);
                gameRoutineAndKeepScore(board);
                announceOnTheWinner(m_PlayerOne, m_PlayerTwo);
                v_WantToPlayAnotherGame = askForAnotherGame();
                m_IconSymbol.Clear();
            }
            while (v_WantToPlayAnotherGame == true);
        }

        private Player playerOneLogin()
        {
            string nameOfPlayerOne;

            printSign("Player One Login");
            printPlayerLogin();
            nameOfPlayerOne = System.Console.ReadLine();
            exitIfQ(nameOfPlayerOne);
            Ex02.ConsoleUtils.Screen.Clear();

            return new Player(nameOfPlayerOne, false);
        }

        private Player playerTwoLogin(string io_NameOfPlayerOne)
        {
            string nameOfPlayerTwo = null;
            bool v_WantToPlayVsCompter;

            printSign("Player Two Login");
            v_WantToPlayVsCompter = playerOneChoosingCompetition(io_NameOfPlayerOne);

            if (v_WantToPlayVsCompter == false)
            {
                printPlayerLogin();
                nameOfPlayerTwo = System.Console.ReadLine();
                exitIfQ(nameOfPlayerTwo);
            }

            Ex02.ConsoleUtils.Screen.Clear();

            return new Player(nameOfPlayerTwo, v_WantToPlayVsCompter);
        }

        private bool playerOneChoosingCompetition(string i_NameOfPlayerOne)
        {
            string playerChoice;
            bool v_ValidInput;

            do
            {
                printChoosingOfCompetitionForPlayerOne(i_NameOfPlayerOne);
                playerChoice = System.Console.ReadLine();
                exitIfQ(playerChoice);
                v_ValidInput = CheckInput.IsValidPlayerOneEnemyChoice(playerChoice);
            }
            while (v_ValidInput == false);

            return playerChoice == "1";
        }

        private GameBoard makeGameBoard()
        {
            string sizeOfBoard;
            string[] seperator = { "x" };
            string[] seperateSizeOfBoard;
            int rowOfBoard;
            int colOfBoard;
            bool v_SizeIsValid;

            printSign("Choosing Board Size");

            do
            {
                printChoseSizeOfBoardForPlayerOne();
                sizeOfBoard = System.Console.ReadLine();
                exitIfQ(sizeOfBoard);
                v_SizeIsValid = CheckInput.IsValidBoardSize(sizeOfBoard);
            }
            while (v_SizeIsValid == false);

            seperateSizeOfBoard = sizeOfBoard.Split(seperator, 2, StringSplitOptions.RemoveEmptyEntries);
            rowOfBoard = int.Parse(seperateSizeOfBoard[0]);
            colOfBoard = int.Parse(seperateSizeOfBoard[1]);
            Ex02.ConsoleUtils.Screen.Clear();

            return new GameBoard(rowOfBoard, colOfBoard);
        }

        private void gameRoutineAndKeepScore(GameBoard io_Board)
        {
            eTurn playerTurn = eTurn.PlayerOne;

            while (io_Board.checkIfGamehasFinished() == false)
            {
                playerMakeMoveHisTurn(io_Board, playerTurn);
                switchTurn(ref playerTurn);
            }
        }

        private void playerMakeMoveHisTurn(GameBoard io_Board, eTurn i_PlayingPlayer)
        {
            eMoveNum moveNum = eMoveNum.FirstMove;
            Coordinate firstMoveCoordinate =
                askPlayingPlayerForMoveCheckMoveAndMakeCoordinate(io_Board, i_PlayingPlayer, moveNum, null);
            int symbolOfFirstMove;

            symbolOfFirstMove = representTheBoardWithMove(io_Board, firstMoveCoordinate);

            moveNum = eMoveNum.SecondMove;
            Coordinate secondMoveCoordinate = askPlayingPlayerForMoveCheckMoveAndMakeCoordinate(io_Board, i_PlayingPlayer, moveNum, symbolOfFirstMove);

            int symbolOfSecondMove;

            symbolOfSecondMove = representTheBoardWithMove(io_Board, secondMoveCoordinate);

            if (symbolOfSecondMove != symbolOfFirstMove)
            {
                System.Threading.Thread.Sleep(2000);
                cancelLastPlayingPlayerPlay(io_Board, firstMoveCoordinate, secondMoveCoordinate);
            }
            else
            {
                if (i_PlayingPlayer == eTurn.PlayerOne)
                {
                    m_PlayerOne.GivePlayerOnePoint();
                }
                else
                {
                    m_PlayerTwo.GivePlayerOnePoint();
                }
            }

            if (m_PlayerTwo.IsAi() == true)
            {
                m_PlayerTwo.AiBrain.SetCardRevealedFromLastMove(firstMoveCoordinate, symbolOfFirstMove, secondMoveCoordinate, symbolOfSecondMove);
            }
        }

        private Coordinate askPlayingPlayerForMoveCheckMoveAndMakeCoordinate(GameBoard io_Board, eTurn io_PlayingPlayer, eMoveNum i_MoveNum, int? i_SymbolOfFirstMoveCardRevealed)
        {
            Coordinate moveCoordinate;
            bool v_AlreadyExposed;

            do
            {
                printMakeAMove(io_PlayingPlayer);
                moveCoordinate = askPlayingPlayerForMoveAndMakeCoordinate(io_Board, io_PlayingPlayer, i_MoveNum, i_SymbolOfFirstMoveCardRevealed);
                v_AlreadyExposed = CheckInput.IssueErrorMessageExposedCube(CheckIfCoordinateisExposed(io_Board, moveCoordinate));
            }
            while (v_AlreadyExposed == true);

            return moveCoordinate;
        }

        private bool CheckIfCoordinateisExposed(GameBoard io_Board, Coordinate io_CurrMoveCoordinate)
        {
            return io_Board.CheckIfAlreadyExposed(io_CurrMoveCoordinate);
        }

        private Coordinate askPlayingPlayerForMoveAndMakeCoordinate(GameBoard io_Board, eTurn io_PlayingPlayer, eMoveNum io_MoveNum, int? i_SymbolOfFirstMoveCardRevealed)
        {
            Coordinate moveCoordinate;

            if (io_PlayingPlayer == eTurn.PlayerOne)
            {
                moveCoordinate = getMoveFromHumanPlayerAndMakeCoordinate(io_Board);
            }
            else
            {
                if (m_PlayerTwo.IsAi() == true)
                {
                    printComputerMakingAMove();

                    if (io_MoveNum == eMoveNum.FirstMove)
                    {
                        moveCoordinate = m_PlayerTwo.AiBrain.MakingFirstMove();
                    }
                    else
                    {
                        moveCoordinate = m_PlayerTwo.AiBrain.MakingSecondMove(i_SymbolOfFirstMoveCardRevealed.Value);
                    }

                    System.Console.Write(UI.sr_ColSymbol[moveCoordinate.Col]);
                    System.Console.WriteLine(UI.sr_RowSymbol[moveCoordinate.Row]);
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    moveCoordinate = getMoveFromHumanPlayerAndMakeCoordinate(io_Board);
                }
            }

            return moveCoordinate;
        }

        private Coordinate getMoveFromHumanPlayerAndMakeCoordinate(GameBoard io_Board)
        {
            string moveInputFromPlayer;
            bool v_MoveIsValid;

            do
            {
                moveInputFromPlayer = System.Console.ReadLine();
                exitIfQ(moveInputFromPlayer);
                v_MoveIsValid = CheckInput.IsValidMove(moveInputFromPlayer, io_Board.NumOfRows, io_Board.NumOfCols);
            }
            while (v_MoveIsValid == false);

            return new Coordinate(moveInputFromPlayer);
        }

        private int representTheBoardWithMove(GameBoard io_Board, Coordinate io_MoveCoordinate)
        {
            int symbolMove = io_Board.ExposeSymbolAndTakeValue(io_MoveCoordinate);

            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(io_Board);

            return symbolMove;
        }

        private void cancelLastPlayingPlayerPlay(GameBoard io_Board, Coordinate i_FirstMoveCoordinate, Coordinate i_SecondMoveCoordinate)
        {
            io_Board.HideIcon(i_FirstMoveCoordinate);
            io_Board.HideIcon(i_SecondMoveCoordinate);
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(io_Board);
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
            string theWinner;
            string playerScorePresentation = string.Format("   {0} score is : {1} | {2} score is : {3}  ", io_PlayerOne.NameOfPlayer, io_PlayerOne.Score, io_PlayerTwo.NameOfPlayer, io_PlayerTwo.Score);

            printSign(playerScorePresentation);

            if (io_PlayerOne.Score > io_PlayerTwo.Score)
            {
                theWinner = io_PlayerOne.NameOfPlayer;
            }
            else if (io_PlayerOne.Score < io_PlayerTwo.Score)
            {
                theWinner = io_PlayerTwo.NameOfPlayer;
            }
            else
            {
                theWinner = "Both Of You";
            }

            string winnerAnnouncment = string.Format("The Winner Is {0}", theWinner);

            printSign(winnerAnnouncment);
        }

        private bool askForAnotherGame()
        {
            string anotherGameQuestion = string.Format("{0} And {1}{2}Do You Want To Play Another Game (Yes Or No)", m_PlayerOne.NameOfPlayer, m_PlayerTwo.NameOfPlayer, Environment.NewLine);
            string playersAnswer;
            bool v_PlayerOneAndTwoIsDesicion;
            bool v_validAnswer;

            System.Console.WriteLine(anotherGameQuestion);

            do
            {
                playersAnswer = System.Console.ReadLine();
                v_validAnswer = CheckInput.CheckValidAnswerForAnotherGameQuestion(playersAnswer);
            }
            while (v_validAnswer == false);

            v_PlayerOneAndTwoIsDesicion = playersAnswer == "Yes" || playersAnswer == "yes";

            if (v_PlayerOneAndTwoIsDesicion == true)
            {
                Ex02.ConsoleUtils.Screen.Clear();
            }

            return v_PlayerOneAndTwoIsDesicion;
        }

        private void printChoseSizeOfBoardForPlayerOne()
        {
            string msg =
                string.Format(
                    "{0} I need you to determine the size of the board{1}Max size is 6 and Min size is 4 and make sure you enter an even number of square{1}Example of Input: 6x4", m_PlayerOne.NameOfPlayer, Environment.NewLine);

            System.Console.WriteLine(msg);
        }

        private void printBoard(GameBoard io_Board)
        {
            int numOfRowsInCurrBoard = (io_Board.NumOfRows * sr_SpaceForSingleCubeRows) + sr_SpacesBetweenCoordinatesAndBoardAndEdges;
            int numOfColsInCurrBoard = (io_Board.NumOfCols * sr_SpaceForSingleCubeCols) + sr_SpacesBetweenCoordinatesAndBoardAndEdges;

            for (int i = 0; i < numOfRowsInCurrBoard; i++)
            {
                for (int j = 0; j < numOfColsInCurrBoard; j++)
                {
                    if (sr_PresentationBoard[i, j] == sr_SignOfPlaceForGameIcon)
                    {
                        Coordinate gameBoardCoordinate = Coordinate.FromPresentationBoardCoordinateToGameBoardCoordinate(i, j);
                        printIconInCube(io_Board.GetIconInCoordinate(gameBoardCoordinate));
                    }
                    else
                    {
                        System.Console.Write(sr_PresentationBoard[i, j]);
                    }
                }

                System.Console.Write(Environment.NewLine);
            }
        }

        private void printIconInCube(int i_SymbolOfIcon)
        {
            System.Console.Write(m_IconSymbol[i_SymbolOfIcon]);
        }

        private void printMakeAMove(eTurn io_PlayingPlayer)
        {
            string playingPlayer;

            if (io_PlayingPlayer == eTurn.PlayerOne)
            {
                playingPlayer = m_PlayerOne.NameOfPlayer;
            }
            else
            {
                playingPlayer = m_PlayerTwo.NameOfPlayer;
            }

            string msg = string.Format("{0} Make a Move:{1}", playingPlayer, Environment.NewLine);

            System.Console.WriteLine(msg);
        }

        private void printComputerMakingAMove()
        {
            System.Console.Write("RoboMatch Making His Move");
            System.Threading.Thread.Sleep(400);
            System.Console.Write(".");
            System.Threading.Thread.Sleep(400);
            System.Console.Write(".");
            System.Threading.Thread.Sleep(400);
            System.Console.Write(".");
            System.Threading.Thread.Sleep(400);
            System.Console.Write(".");
            System.Threading.Thread.Sleep(400);
            System.Console.WriteLine(".");
            System.Threading.Thread.Sleep(400);
        }

        private void exitIfQ(string io_Move)
        {
            if (io_Move == "Q")
            {
                Environment.Exit(1);
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
                // range in ASCII table with only symbol (more the max board size)
                int randomNumber = random.Next(0, listOfAllPossibleIcon.Count);
                m_IconSymbol.Add(listOfAllPossibleIcon[randomNumber]);
                listOfAllPossibleIcon.RemoveAt(randomNumber);
            }
        }

        private List<char> makeListOfAllPossibleCharacters(int i_NumOfCharMatchForIcon)
        {
            // range in ASCII table with only symbol (more the max board size)
            List<char> allPossibleChar = new List<char>(i_NumOfCharMatchForIcon);
            char startOfPossibleCharForIcon = '!';

            for (int i = 0; i < i_NumOfCharMatchForIcon; i++)
            {
                allPossibleChar.Add(startOfPossibleCharForIcon);
                startOfPossibleCharForIcon++;
            }

            return allPossibleChar;
        }

        private void printSign(string i_Title)
        {
            string firstAndLastLineOfRectangle = new string('-', sr_PageWidth);
            string spacesWithPlaceToEdgesOfRectangle = new string(' ', sr_PageWidth - 2);

            string oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle =
                new string(' ', (sr_PageWidth - 1 - i_Title.Length) / 2);

            string middleOfRectangle = string.Format("|{0}|", spacesWithPlaceToEdgesOfRectangle);

            string middleOfRectangleTitleLine =
                string.Format("|{0}{1}{0}|", oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle, i_Title);

            System.Console.WriteLine(firstAndLastLineOfRectangle);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(middleOfRectangleTitleLine);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(firstAndLastLineOfRectangle);
        }

        private void printPlayerLogin()
        {
            System.Console.WriteLine("Please Enter Your Name: ");
            System.Console.Write(Environment.NewLine);
        }

        private void printChoosingOfCompetitionForPlayerOne(string i_NameOfPlayerOne)
        {
            string msg = string.Format("{0} Press 1 If You Want To Play Against The Computer(AI){1}press 0 If You Want To Play Against Another Player", i_NameOfPlayerOne, Environment.NewLine);

            System.Console.WriteLine(msg);
        }
    }
}