
namespace B20_Ex02
{
    public class Program
    {
        public static void Main()
        {

            // wait for enter
            System.Console.WriteLine("Please press 'Enter' to exit...");
            System.Console.ReadLine();
        }

        public static void PlayMatchGame()
        {
            B20_Ex02.MemoryCardGame gameBoxCardGame = new MemoryCardGame();
            bool v_WantToPlay = true;
            string nameOfPlayerOne;
            string nameOfPlayerTwo;
            loginAndStartPlaying(out nameOfPlayerOne, out nameOfPlayerTwo);

            while (v_WantToPlay == true)
            {

            }
        }

        private static void loginAndStartPlaying(out string o_NameOfPlayerOne,out string o_NameOfPlayerTwo)
        {
            string playerIsChoice;
            bool v_IsWantToPlayVsCompter;
            bool v_ValidInput = false;

            UI.PrintWelcomeSign("Welcome To Dor's World");
            UI.PrintPlayerLogin();
            o_NameOfPlayerOne = System.Console.ReadLine();
            UI.PrintChoosingOfCompetitionForPlayerOne(o_NameOfPlayerOne);
            playerIsChoice = System.Console.ReadLine();
            v_ValidInput = playerIsChoice == "1" || playerIsChoice == "0";
            v_IsWantToPlayVsCompter = playerIsChoice == "1";

            while (v_ValidInput == false)
            {
                UI.InputIsValid();
                playerIsChoice = System.Console.ReadLine();
                v_ValidInput = playerIsChoice == "1" || playerIsChoice == "0";
                v_IsWantToPlayVsCompter = playerIsChoice == "1";
            }

            if (v_IsWantToPlayVsCompter == false)
            {
                UI.PrintPlayerLogin();
                o_NameOfPlayerTwo = System.Console.ReadLine();
            }
            else
            {
                o_NameOfPlayerTwo = null;
            }
        }
    }
}
