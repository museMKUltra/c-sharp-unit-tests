using ClassLibrary1.Fundamentals;
using NUnit.Framework;

namespace ClassLibrary1.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        private Stack<string> _stack;

        [SetUp]
        public void SetUp()
        {
            _stack = new Stack<string>();
        }

        [Test]
        public void Push_ArgIsNull_ThrowArgNullException()
        {
            Assert.That(() => _stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Push_ValidArg_AddObjectToTheStack()
        {
            _stack.Push("a");

            Assert.That(_stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_EmptyStack_ReturnZero()
        {
            Assert.That(_stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Pop_EmptyStack_ThrowInvalidOperationException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_StackWithAFewObjects_ReturnObjectOnTheTop()
        {
            // Arrange
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            // Act
            var result = _stack.Pop();

            // Assert
            Assert.That(result, Is.EqualTo("c"));
        }

        [Test]
        public void Pop_StackWithAFewObjects_RemoveObjectOnTheTop()
        {
            // Arrange
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            // Act
            _stack.Pop();

            // Assert
            Assert.That(_stack.Count, Is.EqualTo(2));
        }

        [Test]
        public void Peek_EmptyStack_ThrowInvalidOperationException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_StackWithObjects_ReturnObjectOnTheTop()
        {
            // Arrange
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            // Act
            var result = _stack.Peek();

            // Assert
            Assert.That(result, Is.EqualTo("c"));
        }

        [Test]
        public void Peek_StackWithObjects_DoesNotRemoveTheObjectOnTheTopOfTheStack()
        {
            // Arrange
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            // Act
            _stack.Peek();

            // Assert
            Assert.That(_stack.Count, Is.EqualTo(3));
        }
    }
}