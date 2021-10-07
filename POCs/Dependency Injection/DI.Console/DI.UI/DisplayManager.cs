using System;

namespace DI.UI
{
    public class DisplayManager : IDisplayManager
    {
        public string ShowMenu()
        {
            Console.WriteLine("==================== Available Options ====================");

            foreach (var item in Enum.GetValues(typeof(MenuOption)))
            {
                Console.WriteLine($"{(int)item}. {Enum.GetName(typeof(MenuOption), item)}");
            }

            Console.WriteLine("Enter desired option (number): ");

            return Console.ReadLine();
        }
        
        public Tuple<T, T> ShowInstruction<T>(string option)
        {
            int opt = int.Parse(option);

            Console.WriteLine("Enter First Number: ");
            var fNum = Console.ReadLine();
            var firstNumber = !string.IsNullOrEmpty(fNum) ? (T)Convert.ChangeType(fNum, typeof(T)) :
                (opt == (int)MenuOption.Divide || opt == (int)MenuOption.Modulo) ? (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));

            Console.WriteLine("Enter Second Number: ");
            var sNum = Console.ReadLine();
            var secondNumber = !string.IsNullOrEmpty(sNum) ? (T)Convert.ChangeType(sNum, typeof(T)) :
                (opt == (int)MenuOption.Divide || opt == (int)MenuOption.Modulo) ?  (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));

            return Tuple.Create(firstNumber, secondNumber); ;
        }

        public string ShowResult(string result)
        {
            Console.WriteLine($"Result: {result}\n");
            return ShowMenu();
        }
    }
}
