# GitHub
[c-sharp-unit-tests](https://github.com/museMKUltra/c-sharp-unit-tests)

# HackMD
[Documents](https://hackmd.io/X61mGzQdTLObVizIZ1lDaw?view)

# Unit Testing for C# Developers
[Code with Mosh](https://codewithmosh.com/courses)

## Day15
#### Getting Started 

### What is Automated Testing
> The practice of writing code to test our code, and then run those tests in an automated fashion which is repeatable.

### Benefits of Automated Testing
* Test your code frequently, in less time
* Catch bugs before deploying
* Deploy with confidence
* Refactor with confidence
    * **Refactoring** means changing the structure of the code without changing its behavior.
* Focus more on the quality

### Types of Tests
* Unit test
    * Cheap to write
    * Execute fast
    * Don't give a lot of confidence
> Tests a unit of an application without its **external** dependency.
* Integration test
    * Take longer to execute
    * Give more confidence
> Tests the application with its **external** dependencies.
* End-to-end test
    * Give you the greatest confidence
    * Very slow
    * Very brittle
> Drives an application through its UI.

### Test Pyramid
* Favor unit tests to e2e tests
* Cover unit test gaps with integration tests
* Use end-to-end tests sparingly

### The Tooling
> Focus on the **fundamentals** not the tooling.
:::info
*ReSharper* which released by *JetBrains* is an extension of *Visual Studio*, or you can use cross-platform .NET IDE *Rider* directly.
:::
:::success
We use *NUnit* to practice unit tests in the *Ride*.
:::

### Writing Your First Unit Test
```csharp
public class User
{
    public bool IsAdmin { get; set; }
}

public class Reservation
{
    public User MadeBy { get; set; }

    public bool CanBeCancelledBy(User user)
    {
        return user.IsAdmin || MadeBy == user;
    }
}
```
```csharp
[TestFixture]
public class ReservationTests
{
    /*
     * 1. name of method to test
     * 2. the scenario for testing
     * 3. expected behavior
     */
    [Test]
    public void CanBeCancelledBy_UserIsAdmin_ReturnTrue()
    {
        // Arrange
        var reservation = new Reservation();

        // Act
        var result = reservation.CanBeCancelledBy(new User {IsAdmin = true});

        // Assert
        Assert.IsTrue(result);
        // Assert.That(result, Is.True);
        // Assert.That(result == true);
    }
}
```

### What is Test-driven Development
> With TDD you write your tests before writing the production code.
* Process of TDD
    1. Write a failing test
    2. Write the simplest code to make the test pass
    3. Refactor if necessary
* Benefits of TDD
    * Testable source code
    * Full coverage by tests
    * Simpler implementation
:::info
In this course, we focus on the **code first** so that you can master the fundamentals of testing, then you will be ready to start **test first**.
:::

---
#### Fundamentals of Unit Testing

### Characteristics of Good Unit Tests
* First-class citizens
* Clean, readable and maintainable
* No logic
* Isolated
* Not too specific/general

### What to Test and What Not to Test
> Testable code is clean, clean code is testable.

Test **outcome** of the method
* Query 
    * Verify the function which returns right value
    * You might have multiple executions, then you have to test all of them.
* Command
    * Verify the changes of system which perform an action
        * Changing the statement of objects in memory
        * Writing to the database
        * Call the web server
        * Sending the messages
    * Such functions may return value as well

**Don't** test
* Language features
* 3rd-party code

### Naming and Organizing Tests
> The name of your tests should clearly specify the business rule you're testing.
* Test projects
    * TestNinja -> TestNinja.UnitTests
* Test classes
    * Reservation -> ReservationTests
* Test methods
    * [MethodName]_ [Scenario]_ [ExpectedBehavior]
* How many tests?
    * Number of test >= Number of execution paths

### Writing a Simple Unit Test
```csharp
public class Math
{
    public int Max(int a, int b)
    {
        return a > b ? a : b;
    }
}
```
```csharp
[TestFixture]
public class MathTests
{
    [Test]
    public void Max_FirstArgumentIsGreater_ReturnTheFirstArguments()
    {
        var math = new Math();
        
        var result = math.Max(2, 1);

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Max_SecondArgumentIsGreater_ReturnTheSecondArguments()
    {
        var math = new Math();
        
        var result = math.Max(1, 2);

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Max_ArgumentsAreEqual_ReturnTheSameArgument()
    {
        var math = new Math();
        
        var result = math.Max(1, 1);

        Assert.That(result, Is.EqualTo(1));
    }
}
```

### Basic Techniques
* [SetUp] -> call that method before running each test
* [TearDown] -> call that method after each test
* [Ignore] -> comment out the test temporary

### Black-box testing
> Consider **possible situations** to test. For *Max* testing, there are three cases, greater than, less than, and equal, not just focus on max value.

### Parameterized tests
> A clean way to test **different cases** with multiple parameters so that you don't have to write lots of separated test cases.

```csharp
[TestFixture]
public class MathTests
{
    private Math _math;
    
    [SetUp]
    public void SetUp()
    {
        _math = new Math();
    }
    
    [Test]
    // [Ignore("ignoring message...")]
    [TestCase(2, 1, 2)]
    [TestCase(1, 2, 2)]
    [TestCase(1, 1, 1)]
    public void Max_WhenCalled_ReturnTheGreaterArgument(int a, int b, int expectedResult)
    {
        var result = _math.Max(a, b);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}
```
:::info
*NUnit* has a easier way for **parameterized test** than *MSTest* to use.
:::

### Trustworthy tests 
> Without TDD, after your test passed, go to the production code and make a small change to **create a bug** temporary, if the test still pass, then you probably didn’t test the right thing.

### Cost of Bugs
> Focus on delivering **quality** software with less defects. Be **pragmatic** to choose the cost upfront by writing test, or pay a far greater cost to fix bugs after releasing your software.
