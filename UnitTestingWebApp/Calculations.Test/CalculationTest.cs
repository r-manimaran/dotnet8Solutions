using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Calculations.Test
{
    public class CalculationTest
    {
        [Fact]
        public void Add_SimpleValuesShouldCalculate()
        {
           // Arrange
            var expected = 3;
            var calc = new Calculator();

            // Act
            var result = calc.Add(1, 2);
           
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_SimpleValuesShouldCalculate()
        {
            // Arrange
            var expected = 1;
            var calc = new Calculator();
            // Act
            var result = calc.Subtract(3, 2);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
