using System;

namespace DI.Domain
{
    public class OperationManager : IOperationManager
    {
        public T Calculate<T>(string operation, Tuple<T, T> inputs)
        {
            T result = default(T);
            var operationEnum = (OperationType)Enum.Parse(typeof(OperationType), operation);

            switch (operationEnum)
            {
                case OperationType.Add:
                    result = (T)Convert.ChangeType(Add((long)Convert.ChangeType(inputs.Item1, typeof(long)), (long)Convert.ChangeType(inputs.Item2, typeof(long))), typeof(T));
                    break;
                case OperationType.Subtract:
                    result = (T)Convert.ChangeType(Subtract((long)Convert.ChangeType(inputs.Item1, typeof(long)), (long)Convert.ChangeType(inputs.Item2, typeof(long))), typeof(T));
                    break;
                case OperationType.Multiply:
                    result = (T)Convert.ChangeType(Multiply((long)Convert.ChangeType(inputs.Item1, typeof(long)), (long)Convert.ChangeType(inputs.Item2, typeof(long))), typeof(T));
                    break;
                case OperationType.Divide:
                    result = (T)Convert.ChangeType(Divide((long)Convert.ChangeType(inputs.Item1, typeof(long)), (long)Convert.ChangeType(inputs.Item2, typeof(long))), typeof(T));
                    break;
                case OperationType.Modulo:
                    result = (T)Convert.ChangeType(Modulo((long)Convert.ChangeType(inputs.Item1, typeof(long)), (long)Convert.ChangeType(inputs.Item2, typeof(long))), typeof(T));
                    break;
            }

            return result;
        }

        private long Add(long firstNumber, long secondNumber)
        {
            return firstNumber + secondNumber;
        }

        private long Subtract(long firstNumber, long secondNumber)
        {
            return firstNumber - secondNumber;
        }

        private long Multiply(long firstNumber, long secondNumber)
        {
            return firstNumber * secondNumber;
        }

        private double Divide(double firstNumber, double secondNumber)
        {
            return firstNumber / secondNumber;
        }

        private long Modulo(long firstNumber, long secondNumber)
        {
            long remainder;
            Math.DivRem(firstNumber, secondNumber, out remainder);

            return remainder;
        }        
    }
}
