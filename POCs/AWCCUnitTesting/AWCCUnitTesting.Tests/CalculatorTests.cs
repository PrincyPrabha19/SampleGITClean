using AWCCUnitTesting.Domain;
using AWCCUnitTesting.Domain.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWCCUnitTesting.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Calculator_ReturnSumOfTwoNumbers()
        {
            // Arrange
            int a = 10;
            int b = 20;

            int expectedResult = 30;
            ICalculator calculator = new Calculator();

            // Act
            int actualResult = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

		[TestMethod]
		public void Calculator_ReturnSubtractOfTwoNumbers()
		{
			// Arrange
			int a = 20;
			int b = 5;

			int expectedResult = 15;
			ICalculator calculator = new Calculator();

			// Act
			int actualResult = calculator.Subtract(a, b);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}
	}
}
