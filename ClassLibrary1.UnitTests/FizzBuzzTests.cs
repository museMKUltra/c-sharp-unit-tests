using ClassLibrary1.Fundamentals;
using NUnit.Framework;

namespace ClassLibrary1.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        private FizzBuzz _fizzBuzz;

        [SetUp]
        public void SetUp()
        {
            _fizzBuzz = new FizzBuzz();
        }

        [Test]
        public void GetOutput_InputIsDivisibleBy3And5_ReturnFizzBuzz()
        {
            var output = _fizzBuzz.GetOutput(15);

            Assert.That(output, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        public void GetOutput_InputIsDivisibleBy3Only_ReturnFizz()
        {
            var output = _fizzBuzz.GetOutput(3);

            Assert.That(output, Is.EqualTo("Fizz"));
        }

        [Test]
        public void GetOutput_InputIsDivisibleBy5Only_ReturnBuzz()
        {
            var output = _fizzBuzz.GetOutput(5);

            Assert.That(output, Is.EqualTo("Buzz"));
        }

        [Test]
        public void GetOutput_InputIsNotDivisibleBy3Or5_ReturnTheSameNumber()
        {
            var output = _fizzBuzz.GetOutput(1);

            Assert.That(output, Is.EqualTo("1"));
        }
    }
}