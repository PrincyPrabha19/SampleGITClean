namespace AWCCUnitTesting.Domain.Classes
{
    public class Calculator : ICalculator
    {
        public Calculator()
        {

        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}
