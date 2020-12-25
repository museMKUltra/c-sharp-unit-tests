# GitHub
[c-sharp-unit-tests](https://github.com/museMKUltra/c-sharp-unit-tests)

# Unit Testing for C# Developers
[Code with Mosh](https://codewithmosh.com/courses)

## Day15
Getting Started 

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