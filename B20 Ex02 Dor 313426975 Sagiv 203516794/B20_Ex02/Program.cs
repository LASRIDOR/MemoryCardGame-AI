
namespace B20_Ex02
{
    public class Program
    {
        public static void Main()
        {
            UI setGame = new UI();

            setGame.PlayMatchGame();
            // wait for enter
            System.Console.WriteLine("Please press 'Enter' to exit...");
            System.Console.ReadLine();
        }

    }
}
