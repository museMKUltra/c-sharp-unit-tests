# GitHub
[c-sharp-unit-tests](https://github.com/museMKUltra/c-sharp-unit-tests)

# HackMD
[Documents](https://hackmd.io/X61mGzQdTLObVizIZ1lDaw?view)

# Unit Testing for C# Developers
[Code with Mosh](https://codewithmosh.com/courses)

## Day15

### Getting Started 

#### What is Automated Testing
> The practice of writing code to test our code, and then run those tests in an automated fashion which is repeatable.

#### Benefits of Automated Testing
* Test your code frequently, in less time
* Catch bugs before deploying
* Deploy with confidence
* Refactor with confidence
    * **Refactoring** means changing the structure of the code without changing its behavior.
* Focus more on the quality

#### Types of Tests
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

#### Test Pyramid
* Favor unit tests to e2e tests
* Cover unit test gaps with integration tests
* Use end-to-end tests sparingly

#### The Tooling
> Focus on the **fundamentals** not the tooling.
:::info
*ReSharper* which released by *JetBrains* is an extension of *Visual Studio*, or you can use cross-platform .NET IDE *Rider* directly.
:::
:::success
We use *NUnit* to practice unit tests in the *Ride*.
:::

#### Writing Your First Unit Test
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

#### What is Test-driven Development
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

### Fundamentals of Unit Testing

#### Characteristics of Good Unit Tests
* First-class citizens
* Clean, readable and maintainable
* No logic
* Isolated
* Not too specific/general

#### What to Test and What Not to Test
> Testable code is clean, clean code is testable.

:::info
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
:::
:::warning
**Don't** test
* Language features
* 3rd-party code
:::

#### Naming and Organizing Tests
> The name of your tests should clearly specify the business rule you're testing.
* Test projects
    * TestNinja -> TestNinja.UnitTests
* Test classes
    * Reservation -> ReservationTests
* Test methods
    * [MethodName]_ [Scenario]_ [ExpectedBehavior]
* How many tests?
    * Number of test >= Number of execution paths

#### Writing a Simple Unit Test
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

#### Basic Techniques
* [SetUp] -> call that method before running each test
* [TearDown] -> call that method after each test
* [Ignore] -> comment out the test temporary

#### Black-box testing
> Consider **possible situations** to test. For *Max* testing, there are three cases, greater than, less than, and equal, not just focus on max value.

#### Parameterized tests
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

#### Trustworthy tests 
> Without TDD, after your test passed, go to the production code and make a small change to **create a bug** temporary, if the test still pass, then you probably didn’t test the right thing.

#### Cost of Bugs
> Focus on delivering **quality** software with less defects. Be **pragmatic** to choose the cost upfront by writing test, or pay a far greater cost to fix bugs after releasing your software.

## Day16, Day17

### Core Unit Testing Techniques

#### Testing Strings
```csharp
public class HtmlFormatter
{
    public string FormatAsBold(string content)
    {
        return $"<strong>{content}</strong>";
    }
}
```
```csharp
[TestFixture]
public class HtmlFormatterTests
{
    [Test]
    public void FormatAsBold_WhenCalled_ShouldEncloseTheStringWithStrong()
    {
        var formatter = new HtmlFormatter();

        var result = formatter.FormatAsBold("abc");

        // Specific
        Assert.That(result, Is.EqualTo("<strong>abc</strong>").IgnoreCase);

        // More general
        Assert.That(result, Does.StartWith("<strong>").IgnoreCase);
        Assert.That(result, Does.EndWith("</strong>"));
        Assert.That(result, Does.Contain("abc"));
    }
}
```
:::info
In this case, the *Specific* assertion is okay unless you want to verify something probably changing more times such as error messages, or you can use the *More general* assertions to verify your expectation.
:::

#### Testing Arrays
```csharp
public IEnumerable<int> GetOddNumbers(int limit)
{
    for (var i = 1; i <= limit; i++)
        if (i % 2 != 0)
            yield return i;
}
```
:::info
`yield` is a keyword used by *IEnumerable* to define an iterator removes the need for an explicit extra class.
:::
```csharp
[Test]
public void GetOddNumbers_LimitIsGreaterThanZero_ReturnOddNumbersUpToLimit()
{
    var result = _math.GetOddNumbers(5);

    // too general
    // Assert.That(result, Is.Not.Empty);
    // Assert.That(result.Count(), Is.EqualTo(3));
    // Assert.That(result, Does.Contain(1));
    // Assert.That(result, Does.Contain(3));
    // Assert.That(result, Does.Contain(5));

    Assert.That(result, Is.EqualTo(new[] {1, 3, 5}));

    // other useful assertions
    // Assert.That(result, Is.Ordered);
    // Assert.That(result, Is.Unique);
}
```
:::info
Consider list & collection testing about **order**, **contains** or **counts** to make the balance between specific and general.
:::

#### Testing the Return Type of Methods
```csharp
public class CustomerController
{
    public ActionResult GetCustomer(int id)
    {
        if (id == 0)
            return new NotFound();

        return new Ok();
    }
}

public class ActionResult {}

public class NotFound : ActionResult {}

public class Ok : ActionResult {}
```
```csharp
[Test]
public void GetCustomer_IdIsZero_ReturnNotFound()
{
    var controller = new CustomerController();

    var result = controller.GetCustomer(0);

    // exactly the object
    Assert.That(result, Is.TypeOf<NotFound>());

    // the object or its derivatives
    Assert.That(result, Is.InstanceOf<NotFound>());
}
```
:::info
Depend on your application to test the exactly object by *TypeOf* or its derivatives by *InstanceOf*.
:::

#### Testing Methods
```csharp
public class ErrorLogger
{
    public string LastError { get; set; }
    public event EventHandler<Guid> ErrorLogged;

    public void Log(string error)
    {
        // check null, "", " "
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentNullException();
        LastError = error;

        // write a log to storage
        // ...
        ErrorLogged?.Invoke(this, Guid.NewGuid());
    }
}
```
:::success
*EventHandler* is the concept of `delegate` and `event`, there is more detail demonstration of [Events and Delegates](https://hackmd.io/mzqisVx_Q-uICvcCTput6w?view#Events-and-Delegates) in Mosh C# course.
:::

* Which is Void
```csharp
[Test]
public void Log_WhenCalled_SetTheLastErrorProperty()
{
    var logger = new ErrorLogger();

    logger.Log("a");

    // verify the state fo object for void method
    Assert.That(logger.LastError, Is.EqualTo("a"));
}
```
:::info
By definition void method is a **command function** that often change some kind of state, such as the state of object in memory, the value of one or more properties, and additionally may persist the states to store objects in databases, call a web service, or call a message queue.
:::

* That Throw Exceptions
```csharp
[Test]
[TestCase(null)]
[TestCase("")]
[TestCase(" ")]
public void Log_InvalidError_ThrowArgumentNullException(string error)
{
    var logger = new ErrorLogger();

    // use lambda expression to verify the exception
    Assert.That(() => logger.Log(error), Throws.ArgumentNullException);

    // sometimes you have another exception, then could use this
    // Assert.That(() => logger.Log(error), Throws.Exception.TypeOf<DivideByZeroException>());
}
```
:::info
`Throws.Exception.TypeOf` is another choice to verify the type of exception.
:::

* That Raise an Event
```csharp
[Test]
public void Log_ValidError_RaiseErrorLoggedEvent()
{
    var logger = new ErrorLogger();

    var id = Guid.Empty; // for verification
    // append the reference to delegate method
    logger.ErrorLogged += (sender, args) => { id = args; };

    logger.Log("a");

    Assert.That(id, Is.Not.EqualTo(Guid.Empty));
}
```
:::info
Define the `id` to verify that event raised to change the value by arguments, and I think this part of `delegae` and `event` that build a testable code like `interface`.
:::

* Which is Private
```csharp
public void Log(string error)
{
    ...
    
    // ErrorLogged?.Invoke(this, Guid.NewGuid());
    OnErrorLogged(Guid.NewGuid()); 
}

protected virtual void OnErrorLogged(Guid errorId)
{
    ErrorLogged?.Invoke(this, errorId);
}
```
:::info
Someday you might refactor the *ErrorLogged* to a private method *OnErrorLogged*, but you don't need to write a test for that, because it's the implementation detail, and unit test for raising event is still passing.
:::
:::warning
You shouldn't test private!
:::

#### Code Coverage
* Code Coverage tools
    * Visual Studio Enterprise Edition
    * ReSharper Ultimate
    * NCover
    * JetBrains dotCover
:::info
The high coverage doesn't means that you have returned enough tests for methods, it just execute the high percentage based on current implementation, focus on testing for the **black-boxes** by the inputs.
:::

#### Testing in the Real-world
* Dealing with legacy code without unit tests
    * Refactor the critical part to make it testable
    * Cost of testing all codes must outweigh of benefits
* Startup to release production in limit time
    * At least write tests for key parts
    * Some calculator that might replace manual test
* The only developer in the team
    * Educate your team to write cleaner and more testable code
    * [The Art of Writing Beautiful of C# code](https://codewithmosh.com/p/clean-code)

### Exercises
#### Source Codes
* [Fundamentals](https://github.com/museMKUltra/c-sharp-unit-tests/tree/main/ClassLibrary1/Fundamentals)
    * [FizzBuzz](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Fundamentals/FizzBuzz.cs)
    * [DemeritPointsCalculator](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Fundamentals/DemeritPointsCalculator.cs)
    * [Stack](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Fundamentals/Stack.cs)
* [UnitTests](https://github.com/museMKUltra/c-sharp-unit-tests/tree/main/ClassLibrary1.UnitTests)
    * [FizzBuzzTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/FizzBuzzTests.cs)
    * [DemeritPointsCalculatorTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/DemeritPointsCalculatorTests.cs)
    * [StackTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/StackTests.cs)
#### Scenarios
* FizzBuzz
    1. GetOutput_InputIsDivisibleBy3And5_ReturnFizzBuzz
    2. GetOutput_InputIsDivisibleBy3Only_ReturnFizz
    3. GetOutput_InputIsDivisibleBy5Only_ReturnBuzz
    4. GetOutput_InputIsNotDivisibleBy3Or5_ReturnTheSameNumber
* DemeritPointsCalculator
    1. CalculateDemeritPoints_WhenCalled_ReturnDemeritPoints (multi-cases)
    2. CalculateDemeritPoints_SpeedIsOutOfRange_ThrowArgumentOutOfRangeException (multi-cases)
* Stack
    1. Push_ArgIsNull_ThrowArgNullException
    2. Push_ValidArg_AddObjectToTheStack
    3. Count_EmptyStack_ReturnZero
    4. Pop_EmptyStack_ThrowInvalidOperationException
    5. Pop_StackWithAFewObjects_ReturnObjectOnTheTop
    6. Pop_StackWithAFewObjects_RemoveObjectOnTheTop
    7. Peek_EmptyStack_ThrowInvalidOperationException
    8. Peek_StackWithObjects_ReturnObjectOnTheTop
    9. Peek_StackWithObjects_DoesNotRemoveTheObjectOnTheTopOfTheStack
:::info
Remember to consider about **black-box testing**, and verify the result by the public members rather than private ones. Also separate verification in different scenarios but **parameterized test** in the same scenario.
:::

## Day18

### Breaking the External Dependencies

#### Introduction
> Unit tests should not touch **external resources** that is classified as an integration tests.

#### Loosely-coupled and Testable Code
> First of all, writing a testable code and isolate the part with external resources into separated classes that apply on **dependency injection** just like using `interface`.

#### Refactoring Towards a Loosely-coupled Design
```csharp
public class Video
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsProcessed { get; set; }
}

public class VideoService
{
    public string ReadVideoTitle()
    {
        var str = File.ReadAllText("video.txt");
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";

        return video.Title;
    }
}
```
:::info
*JsonConvert* is a class of *Newtonsoft.Json* namespace which is a package can be installed by **NuGet**.
:::
```csharp
public class VideoService
{
    public string ReadVideoTitle()
    {
        // step1. isolate an class
        var str = new FileReader().Read("video.txt"); 
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";

        return video.Title;
    }
}

public class FileReader
{
    public string Read(string path)
    {
        return File.ReadAllText(path);
    }
}
```
:::info
**Step.1** Refactor the code with external resource to an isolated class.
:::
```csharp
// step2. extract an interface
public interface IFileReader
{
    string Read(string path);
}

public class FileReader : IFileReader
{
    public string Read(string path)
    {
        return File.ReadAllText(path);
    }
}
```
:::info
**Step2.** Declare an `interface` to define the members.
:::
```csharp
// step3. create an fake class
// or you can call it MockFileReader/StubFileReader
public class FakeFileReader: IFileReader
{
    public string Read(string path)
    {
        return "";
    }
}
```
:::info
**Step3.** Create an fake `class` to mock the members.
:::

#### Dependency Injection
* via Method Parameters
```csharp
public class VideoService
{
    public string ReadVideoTitle(IFileReader fileReader)
    {
        var str = fileReader.Read("video.txt");
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";

        return video.Title;
    }
}
```
```csharp
public class Program
{
    public static void Main()
    {
        var service = new VideoService();
        // sending the parameter for production
        var title = service.ReadVideoTitle(new FileReader());
    }
}

[TestFixture]
public class VideoServiceTests
{
    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        var service = new VideoService();

        // sending the parameter for testing
        var result = service.ReadVideoTitle(new FakeFileReader());

        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
```
:::info
Use **different parameters** to test by fake data without external resource.
:::
* via Properties
```csharp
public class VideoService
    {
        public IFileReader FileReader { get; set; }

        public VideoService()
        {
            FileReader = new FileReader();
        }

        public string ReadVideoTitle()
        {
            // use the property
            var str = FileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";

            return video.Title;
        }
    }
```
```csharp
public class Program
{
    public static void Main()
    {
        var service = new VideoService();
        var title = service.ReadVideoTitle(); // without parameter
    }
}

[TestFixture]
public class VideoServiceTests
{
    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        var service = new VideoService();
        // declare the property
        service.FileReader = new FakeFileReader();

        var result = service.ReadVideoTitle();
        
        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
```
:::info
Initialize property for production, then **set property** before testing.
:::
* via Constructor
```csharp
public class VideoService
    {
        private IFileReader _fileReader;

        // constructor injection
        public VideoService(IFileReader fileReader = null)
        {
            _fileReader = fileReader ?? new FileReader();
        }

        public string ReadVideoTitle()
        {
            var str = _fileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";

            return video.Title;
        }
    }
```
```csharp
public class Program
{
    public static void Main()
    {
        var service = new VideoService();
        var title = service.ReadVideoTitle();
    }
}

[TestFixture]
public class VideoServiceTests
{
    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        // set fake by constructor
        var service = new VideoService(new FakeFileReader());

        var result = service.ReadVideoTitle();

        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
```
:::info
Set the using class by **constructor injection**.
:::
* Frameworks
    * NInject
    * StructureMap
    * Spring .NET
    * Autofac
    * Unity
> **DI**(Dependency Injection) framework is a registry that contain your interfaces and implementation, and it will automatically take care of creating object graphs based on the interfaces and types registered.

#### Mocking (Isolation) Frameworks
* Frameworks
    * Moq
    * NSubstitute
    * FakeItEasy
    * Rhino Mocks
> Help us **dynamically** create the objects of fake or mock data for testing.

#### Creating Mock Objects Using Moq
```csharp
[TestFixture]
public class VideoServiceTests
{
    private Mock<IFileReader> _fileReader;
    private VideoService _videoService;

    [SetUp]
    public void SetUp()
    {
        _fileReader = new Mock<IFileReader>();
        _videoService = new VideoService(_fileReader.Object);
    }

    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

        var result = _videoService.ReadVideoTitle();

        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
```
:::info
If you want to test another **mock data** by *Read* method, then you can change the *Returns* directly rather than using the *FakeFileReader*.
:::

#### State-based vs Interaction Testing
> Using *Interaction Testing* which test the **external behavior** not the implementation only when dealing with **external resources**, because that might refactor or restructure your code a lot or break other tests. So prefer *State-based Testing* to *Interaction Testing*.

#### Testing the Interaction between Two Objects
```csharp
public class OrderService
{
    private readonly IStorage _storage;

    public OrderService(IStorage storage)
    {
        _storage = storage;
    }

    public int PlaceOrder(Order order)
    {
        var orderId = _storage.Store(order);

        // some other work

        return orderId;
    }
}
```
```csharp
[TestFixture]
public class OrderServiceTests
{
    [Test]
    public void PlaceOrder_WhenCalled_StoreTheOrder()
    {
        var storage = new Mock<IStorage>();
        var orderService = new OrderService(storage.Object);

        var order = new Order();
        orderService.PlaceOrder(order);

        // verify Store had been called after implementation of PlaceOrder 
        storage.Verify(s => s.Store(order));
    }
}
```
:::info
Verify methods **called** inside execution of another method.
:::

#### Fake as Little as Possible
> Use **mocks** as little as possible, only when dealing with **external resources**, otherwise unit tests might turn out to be explosion of interfaces and constructor parameters, also fat and fragile tests.

