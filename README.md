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
:::success
*JsonConvert* is a class of **Newtonsoft.Json** namespace which is a package can be installed by **NuGet**.
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
:::success
**Moq** mocking framework also can be installed by **NuGet**.
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

#### An Example of a Mock Abuse
```csharp
public class Product
{
    public float ListPrice { get; set; }

    public float GetPrice(ICustomer customer)
    {
        if (customer.IsGold)
        {
            return ListPrice * 0.7f;
        }

        return ListPrice;
    }
}
```
```csharp
[TestFixture]
public class ProductTests
{
    [Test]
    public void GetPrice_GoldCustomer_Apply30PercentDiscount()
    {
        // this test is ideal
        var product = new Product {ListPrice = 100};

        var result = product.GetPrice(new Customer {IsGold = true});

        Assert.That(result, Is.EqualTo(70));
    }

    [Test]
    public void GetPrice_GoldCustomer_Apply30PercentDiscount2()
    {
        // this test is mock abuse
        var customer = new Mock<ICustomer>();
        customer.Setup(c => c.IsGold).Returns(true);
        
        var product = new Product {ListPrice = 100};

        var result = product.GetPrice(customer.Object);

        Assert.That(result, Is.EqualTo(70));
    }
}
```
:::info
Try to write small, maintainable, reliable tests, and use **mocks** only when dealing with external resources to avoid a recipe for disaster.
:::

#### Who Should Write Tests
> Writing *Unit* and *Integration tests* is the job of a **software developer**, but *End-to-End tests* don't care about internal implementation but focus on high level like an end user, so it probably written by **test engineer**.

## Day19

### Excercises

#### Source Codes
* [VideoService](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/VideoService.cs)
* [VideoRepository](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/VideoRepository.cs)
* [VideoServiceTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/Mocking/VideoServiceTests.cs)

```csharp
// step0. original code
public class VideoService
{
    public string GetUnprocessedVideoAsCsv()
    {
        var videoIds = new List<int>();

        using (var context = new VideoContext())
        {
            var videos = (from video in context.Videos
                where !video.IsProcessed
                select video).ToList();

            foreach (var v in videos) videoIds.Add(v.Id);

            return String.Join(",", videoIds);
        }
    }
}
```

```csharp
// step1. refactor class
public class VideoService
{
    public string GetUnprocessedVideoAsCsv()
    {
        var videoIds = new List<int>();

        var videos = new VideoRepository().GetUnprocessedVideos();
        foreach (var v in videos) videoIds.Add(v.Id);

        return String.Join(",", videoIds);
    }
}

public class VideoRepository
{
    public IEnumerable<Video> GetUnprocessedVideos()
    {
        using (var context = new VideoContext())
        {
            return (from video in context.Videos
                where !video.IsProcessed
                select video).ToList();
        }
    }
}
```


```csharp
// step2. create interface
public interface IVideoRepository
{
    IEnumerable<Video> GetUnprocessedVideos();
}

public class VideoRepository : IVideoRepository
{
    public IEnumerable<Video> GetUnprocessedVideos()
    {
        using (var context = new VideoContext())
        {
            return (from video in context.Videos
                where !video.IsProcessed
                select video).ToList();
        }
    }
}
```

```csharp
// step3. dependency injection
public class VideoService
{
    private IVideoRepository _videoRepository;

    public VideoService(IVideoRepository videoRepository = null)
    {
        _videoRepository = videoRepository ?? new VideoRepository();
    }
        
    public string GetUnprocessedVideoAsCsv()
    {
        var videoIds = new List<int>();

        var videos = _videoRepository.GetUnprocessedVideos();
        foreach (var v in videos) videoIds.Add(v.Id);

        return String.Join(",", videoIds);
    }
}
```

```csharp
// step4. writing tests
[TestFixture]
public class VideoServiceTests
{
    private Mock<IVideoRepository> _videoRepository;
    private VideoService _videoService;

    [SetUp]
    public void SetUp()
    {
        _videoRepository = new Mock<IVideoRepository>();
        _videoService = new VideoService(_videoRepository.Object);
    }

    [Test]
    public void GetUnprocessedVideoAsCsv_AllVideosAreProcessed_ReturnAnEmptyString()
    {
        _videoRepository.Setup(c => c.GetUnprocessedVideos()).Returns(new List<Video>());

        var videos = _videoService.GetUnprocessedVideoAsCsv();

        Assert.That(videos, Is.EqualTo(""));
    }

    [Test]
    public void GetUnprocessedVideoAsCsv_AFewUnprocessedVideos_ReturnAStringWithIdOfUnprocessedVideos()
    {
        _videoRepository.Setup(c => c.GetUnprocessedVideos()).Returns(new List<Video>
        {
            new Video {Id = 1},
            new Video {Id = 2},
            new Video {Id = 3}
        });

        var videos = _videoService.GetUnprocessedVideoAsCsv();

        Assert.That(videos, Is.EqualTo("1,2,3"));
    }
}
```

#### Source Codes
* [InstallerHelper](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/InstallerHelper.cs)
* [FileDownloader](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/FileDownloader.cs)
* [InstallerHelperTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/Mocking/InstallerHelperTests.cs)

```csharp
// step0. original code
public class InstallerHelper
{
    private string _setupDestinationFile;

    public bool DownloadInstaller(string customerName, string installerName)
    {
        var client = new WebClient();
        try
        {
            client.DownloadFile(string.Format("http://example.com/{0}/{1}", customerName, installerName),
                _setupDestinationFile);
            return true;
        }
        catch (WebException)
        {
            return false;
        }
    }
}
```

```csharp
// step1. refactor class
public class InstallerHelper
{
    private string _setupDestinationFile;

    public bool DownloadInstaller(string customerName, string installerName)
    {
        try
        {
            new FileDownloader.DownloadFile(string.Format("http://example.com/{0}/{1}", customerName, installerName),
                _setupDestinationFile);
            return true;
        }
        catch (WebException)
        {
            return false;
        }
    }
}

public class FileDownloader
{
    public void DownloadFile(string url, string path)
    {
        var client = new WebClient();
        client.DownloadFile(url, path);
    }
}
```
:::info
`WebException` is the what we concern, other exceptions might need to do **other operations**, so don't need to be hidden to return false or test it.
:::

```csharp
// step2. create interface
public interface IFileDownloader
{
    void DownloadFile(string url, string path);
}

public class FileDownloader : IFileDownloader
{
    public void DownloadFile(string url, string path)
    {
        var client = new WebClient();
        client.DownloadFile(url, path);
    }
}
```

```csharp
// step3. dependency injection
public class InstallerHelper
{
    private readonly IFileDownloader _fileDownloader;
    private string _setupDestinationFile;

    public InstallerHelper(IFileDownloader fileDownloader = null)
    {
        _fileDownloader = fileDownloader ?? new FileDownloader();
    }

    public bool DownloadInstaller(string customerName, string installerName)
    {
        try
        {
            _fileDownloader.DownloadFile(string.Format("http://example.com/{0}/{1}", customerName, installerName),
                _setupDestinationFile);
            return true;
        }
        catch (WebException)
        {
            return false;
        }
    }
}
```
:::info
if you use **DI** framework, then write the `_fileDownloader = fileDownloader;` is okay, don't need to use above *poor man's dependency injection approach*.
:::

```csharp
// step4. writing tests
[TestFixture]
public class InstallerHelperTests
{
    private Mock<IFileDownloader> _fileDownloader;
    private InstallerHelper _installerHelper;

    [SetUp]
    public void SetUp()
    {
        _fileDownloader = new Mock<IFileDownloader>();
        _installerHelper = new InstallerHelper(_fileDownloader.Object);
    }

    [Test]
    public void DownloadInstaller_DownloadFail_ReturnFalse()
    {
        _fileDownloader.Setup(f => f.DownloadFile(It.IsAny<string>(), It.IsAny<string>()))
            .Throws<WebException>();

        var result = _installerHelper.DownloadInstaller("customer", "installer");

        Assert.That(result, Is.False);
    }

    [Test]
    public void DownloadInstaller_DownloadCompletes_ReturnTrue()
    {
        var result = _installerHelper.DownloadInstaller("", "");

        Assert.That(result, Is.True);
    }
}
```
:::info
`It.IsAny` is used to ignore the arguments, otherwise you need set the arguments as the same as production code then it could test properly to throw the exception by `_fileDownloader.Setup(f => f.DownloadFile("http://example.com/customer/installer", null)).Throws<WebException>();`
:::

#### Source Codes
* [EmployeeController](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/EmployeeController.cs)
* [EmployeeStorage](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/EmployeeStorage.cs)
* [EmployeeControllerTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/Mocking/EmployeeControllerTests.cs)

```csharp
// step0. original code
public class EmployeeController
{
    private EmployeeContext _db;

    public EmployeeController()
    {
        _db = new EmployeeContext();
    }

    public ActionResult DeleteEmployee(int id)
    {
        var employee = _db.Employees.Find(id);
        _db.Employees.Remove(employee);
        _db.SaveChanges();
        return RedirectToAction("Employees");
    }

    private ActionResult RedirectToAction(string employees)
    {
        return new RedirectResult();
    }
}
```
:::info
We don't need to test `private` *RedirectToAction* method detail, it's purely responsible for implementation, so we can focus on what type it return.
:::
:::success
Remember to install the namespace *System.Data.Entity.Repository* by **NuGet**.
:::

```csharp
// step1. refactor class
public class EmployeeStorage
{
    private readonly EmployeeContext _db;

    public EmployeeStorage()
    {
        _db = new EmployeeContext();
    }

    public void DeleteEmployee(int id)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return;
        _db.Employees.Remove(employee);
        _db.SaveChanges();
    }
}
```
:::warning
I refactor the class with the each methods `Find`, `Remove`, and `SaveChanges`, but it's not necessary. The `EmployeeStorage` should hide the details to publish `DeleteEmployee` is enough.
:::

```csharp
// step2. create interface
public interface IEmployeeStorage
{
    void DeleteEmployee(int id);
}

public class EmployeeStorage : IEmployeeStorage
{
    private readonly EmployeeContext _db;

    public EmployeeStorage()
    {
        _db = new EmployeeContext();
    }

    public void DeleteEmployee(int id)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return;
        _db.Employees.Remove(employee);
        _db.SaveChanges();
    }
}
```
:::info
Use `interface` to mock our external resources in unit tests.
:::
```csharp
// step3. dependency injection
public class EmployeeController
{
    private readonly IEmployeeStorage _storage;

    public EmployeeController(IEmployeeStorage employeeStorage = null)
    {
        _storage = employeeStorage ?? new EmployeeStorage();
    }

    public ActionResult DeleteEmployee(int id)
    {
        _storage.DeleteEmployee(id);
        return RedirectToAction("Employees");
    }

    private ActionResult RedirectToAction(string employees)
    {
        return new RedirectResult();
    }
}
```
:::info
All the responsibility of external resources is encapsulated inside our `_storage`, the testing of details should belong to **End-to-End test**.
:::

```csharp
// step4. writing tests
[TestFixture]
public class EmployeeControllerTests
{
    private Mock<IEmployeeStorage> _storage;
    private EmployeeController _controller;

    [SetUp]
    public void SetUp()
    {
        _storage = new Mock<IEmployeeStorage>();
        _controller = new EmployeeController(_storage.Object);
    }

    [Test]
    public void DeleteEmployee_WhenCalled_DeleteTheEmployeeFromDb()
    {
        _controller.DeleteEmployee(1);

        _storage.Verify(s => s.DeleteEmployee(1));
    }

    [Test]
    public void DeleteEmployee_WhenCalled_ReturnRedirectResult()
    {
        var result = _controller.DeleteEmployee(1);

        Assert.That(result, Is.TypeOf<RedirectResult>());
    }
}
```

## Day20

### Project - Testing BookingHelper

#### Source Codes
* [BookingHelper](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/BookingHelper.cs)
* [BookingRepository](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/BookingRepository.cs)
* [BookingHelperTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/Mocking/BookingHelperTests.cs)

#### Introduction
```csharp
public static class BookingHelper
{
    public static string OverlappingBookingsExist(Booking booking)
    {
        if (booking.Status == "Cancelled")
            return string.Empty;

        var unitOfWork = new UnitOfWork();
        var bookings =
            unitOfWork.Query<Booking>()
                .Where(
                    b => b.Id != booking.Id && b.Status != "Cancelled");

        var overlappingBooking =
            bookings.FirstOrDefault(
                b =>
                    booking.ArrivalDate >= b.ArrivalDate
                    && booking.ArrivalDate < b.DepartureDate
                    || booking.DepartureDate > b.ArrivalDate
                    && booking.DepartureDate <= b.DepartureDate);

        return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
    }
}
```

#### Test Cases
* BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString
* BookingStartsAndFinishesAfterAnExistingBooking_ReturnEmptyString
* BookingStartsAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingReference
* BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingReference
* BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingReference
* BookingStartsInTheMiddleOfAnExistingBookingButFinishesAfter_ReturnExistingBookingReference
* BookingsOverlapButNewBookingIsCancelled_ReturnAnEmptyString

#### Extracting IBookingRepository
```csharp
public static class BookingHelper
{
    public static string OverlappingBookingsExist(Booking booking, IBookingRepository repository )
    {
        if (booking.Status == "Cancelled")
            return string.Empty;

        var bookings = repository.GetActiveBookings(booking.Id);
        
        var overlappingBooking = bookings.FirstOrDefault(
            b =>
                booking.ArrivalDate >= b.ArrivalDate
                && booking.ArrivalDate < b.DepartureDate
                || booking.DepartureDate > b.ArrivalDate
                && booking.DepartureDate <= b.DepartureDate);

        return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
    }
}

public interface IBookingRepository
{
    IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
}

public class BookingRepository : IBookingRepository
{
    public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null)
    {
        var unitOfWork = new UnitOfWork();

        var bookings = unitOfWork.Query<Booking>()
            .Where(b => b.Status != "Cancelled");
        if (excludedBookingId.HasValue)
            bookings = bookings.Where(b => b.Id != excludedBookingId.Value);

        return bookings;
    }
}
```

#### Writing the First Test
```csharp
[TestFixture]
public class BookingHelper_OverlappingBookingsExistTests
{
    [Test]
    public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
    {
        var repository = new Mock<IBookingRepository>();
        repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking>
        {
            new Booking
            {
                Id = 2,
                ArrivalDate = new DateTime(2020, 12, 12, 12, 0, 0),
                DepartureDate = new DateTime(2020, 12, 20, 12, 0, 0),
                Reference = "a"
            }
        }.AsQueryable());

        var result = BookingHelper.OverlappingBookingsExist(
            new Booking {
                Id = 1,
                ArrivalDate = new DateTime(2020, 12, 10, 12, 0, 0),
                DepartureDate = new DateTime(2020, 12, 11, 12, 0, 0),
            }
            , repository.Object);

        Assert.That(result, Is.Empty);
    }
}
```

#### Refactoring
```csharp
[TestFixture]
public class BookingHelper_OverlappingBookingsExistTests
{
    private Booking _existingBooking;
    private Mock<IBookingRepository> _repository;

    [SetUp]
    public void SetUp()
    {
        _existingBooking = new Booking
            {Id = 2, ArrivalDate = ArriveOn(2020, 12, 12), DepartureDate = DepartOn(2020, 12, 20), Reference = "a"};
        _repository = new Mock<IBookingRepository>();
        _repository.Setup(r => r.GetActiveBookings(1)).Returns(new List<Booking> {_existingBooking}.AsQueryable());
    }
    
    [Test]
    public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
    {
        var result = BookingHelper.OverlappingBookingsExist(new Booking
        {
            Id = 1,
            ArrivalDate = Before(_existingBooking.ArrivalDate, days: 2),
            DepartureDate = Before(_existingBooking.ArrivalDate)
        }, _repository.Object);

        Assert.That(result, Is.Empty);
    }

    private static DateTime Before(DateTime dateTime, int days = 1)
    {
        return dateTime.AddDays(-days);
    }


    private static DateTime After(DateTime dateTime, int days = 1)
    {
        return dateTime.AddDays(days);
    }

    private static DateTime DepartOn(int year, int month, int day)
    {
        return new DateTime(year, month, day, 12, 0, 0);
    }

    private static DateTime ArriveOn(int year, int month, int day)
    {
        return new DateTime(year, month, day, 12, 0, 0);
    }
}
```

#### Fixing a Bug
```csharp
public static string OverlappingBookingsExist(Booking booking, IBookingRepository repository)
{
    if (booking.Status == "Cancelled")
        return string.Empty;

    var bookings = repository.GetActiveBookings(booking.Id);
    
    // https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
    var overlappingBooking = bookings.FirstOrDefault(
        b =>
            booking.ArrivalDate < b.DepartureDate
            && b.ArrivalDate < booking.DepartureDate);
            
    return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
}

[Test]
public void BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingReference()
{
    var result = BookingHelper.OverlappingBookingsExist(new Booking
    {
        Id = 1,
        ArrivalDate = Before(_existingBooking.ArrivalDate),
        DepartureDate = After(_existingBooking.DepartureDate),
    }, _repository.Object);

    Assert.That(result, Is.EqualTo(_existingBooking.Reference));
}
```

#### Writing Additional Tests
```csharp
[Test]
public void BookingsOverlapButNewBookingIsCancelled_ReturnAnEmptyString()
{
    var result = BookingHelper.OverlappingBookingsExist(new Booking
    {
        Id = 1,
        ArrivalDate = Before(_existingBooking.ArrivalDate),
        DepartureDate = After(_existingBooking.DepartureDate),
        Status = "Cancelled"
    }, _repository.Object);

    Assert.That(result, Is.Empty);
}
```

## Day21

### Project - HouseKeeperHelper

#### Source Codes
* [HousekeeperService (HousekeeperHelper)](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/HousekeeperService.cs)
* [StatementGenerator](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/StatementGenerator.cs)
* [EmailSender](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1/Mocking/EmailSender.cs)
* [HousekeeperServiceTests](https://github.com/museMKUltra/c-sharp-unit-tests/blob/main/ClassLibrary1.UnitTests/Mocking/HousekeeperServiceTests.cs)

#### Introduction
```csharp
public static class HousekeeperHelper
{
    private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

    public static bool SendStatementEmails(DateTime statementDate)
    {
        var housekeepers = UnitOfWork.Query<Housekeeper>();

        foreach (var housekeeper in housekeepers)
        {
            if (housekeeper.Email == null)
                continue;

            var statementFilename = SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

            if (string.IsNullOrWhiteSpace(statementFilename))
                continue;

            var emailAddress = housekeeper.Email;
            var emailBody = housekeeper.StatementEmailBody;

            try
            {
                EmailFile(emailAddress, emailBody, statementFilename,
                    string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                    MessageBoxButtons.OK);
            }
        }

        return true;
    }

    private static string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate)
    {
        var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

        if (!report.HasData)
            return string.Empty;

        report.CreateDocument();

        var filename = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            string.Format("Sandpiper Statement {0:yyyy-MM} {1}.pdf", statementDate, housekeeperName));

        report.ExportToPdf(filename);

        return filename;
    }

    private static void EmailFile(string emailAddress, string emailBody, string filename, string subject)
    {
        var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
        {
            Port = SystemSettingsHelper.EmailPort,
            Credentials =
                new NetworkCredential(
                    SystemSettingsHelper.EmailUsername,
                    SystemSettingsHelper.EmailPassword)
        };

        var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName,
            Encoding.UTF8);
        var to = new MailAddress(emailAddress);

        var message = new MailMessage(from, to)
        {
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = emailBody,
            BodyEncoding = Encoding.UTF8
        };

        message.Attachments.Add(new Attachment(filename));
        client.Send(message);
        message.Dispose();

        File.Delete(filename);
    }
}
```

#### Refactoring for Testability
```csharp
public class HousekeeperHelper
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStatementGenerator _statementGenerator;
    private readonly IEmailSender _emailSender;
    private readonly IXtraMessageBox _messageBox;

    // isolate classes which depend on external resources with interface
    public HousekeeperHelper(
        IUnitOfWork unitOfWork,
        IStatementGenerator statementGenerator,
        IEmailSender emailSender,
        IXtraMessageBox messageBox)
    {
        _unitOfWork = unitOfWork;
        _statementGenerator = statementGenerator;
        _emailSender = emailSender;
        _messageBox = messageBox;
    }

    public bool SendStatementEmails(DateTime statementDate)
    {
        var housekeepers = _unitOfWork.Query<Housekeeper>();

        foreach (var housekeeper in housekeepers)
        {
            if (housekeeper.Email == null)
                continue;

            var statementFilename =
                _statementGenerator.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

            if (string.IsNullOrWhiteSpace(statementFilename))
                continue;

            var emailAddress = housekeeper.Email;
            var emailBody = housekeeper.StatementEmailBody;

            try
            {
                _emailSender.EmailFile(emailAddress, emailBody, statementFilename,
                    string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
            }
            catch (Exception e)
            {
                _messageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                    MessageBoxButtons.OK);
            }
        }

        return true;
    }
}

public interface IStatementGenerator
{
    string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate);
}

public class StatementGenerator : IStatementGenerator
{
    public string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate)
    {
        var report = new HousekeeperStatementReport(housekeeperOid, statementDate);

        if (!report.HasData)
            return string.Empty;

        report.CreateDocument();

        var filename = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            string.Format("Sandpiper Statement {0:yyyy-MM} {1}.pdf", statementDate, housekeeperName));

        report.ExportToPdf(filename);

        return filename;
    }
}

public interface IEmailSender
{
    void EmailFile(string emailAddress, string emailBody, string filename, string subject);
}

public class EmailSender : IEmailSender
{
    public void EmailFile(string emailAddress, string emailBody, string filename, string subject)
    {
        var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
        {
            Port = SystemSettingsHelper.EmailPort,
            Credentials =
                new NetworkCredential(
                    SystemSettingsHelper.EmailUsername,
                    SystemSettingsHelper.EmailPassword)
        };

        var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName,
            Encoding.UTF8);
        var to = new MailAddress(emailAddress);

        var message = new MailMessage(from, to)
        {
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = emailBody,
            BodyEncoding = Encoding.UTF8
        };

        message.Attachments.Add(new Attachment(filename));
        client.Send(message);
        message.Dispose();

        File.Delete(filename);
    }
}
```

#### Fixing a Design Issue
```csharp
// no need to always return boolean true, set a void function
public void SendStatementEmails(DateTime statementDate)
{
    var housekeepers = _unitOfWork.Query<Housekeeper>();

    foreach (var housekeeper in housekeepers)
    {
        if (housekeeper.Email == null)
            continue;

        var statementFilename =
            _statementGenerator.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);
        if (string.IsNullOrWhiteSpace(statementFilename))
            continue;

        var emailAddress = housekeeper.Email;
        var emailBody = housekeeper.StatementEmailBody;

        try
        {
            _emailSender.EmailFile(emailAddress, emailBody, statementFilename,
                string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
        }
        catch (Exception e)
        {
            _messageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                MessageBoxButtons.OK);
        }
    }
}
```

#### An Alternative Solution
> In the last lecture, I argued that the return type of this method should be void because it always returns true.
>
> Later, however, I realized that it would actually be better to keep the return type as boolean and write a unit test for the scenario where the download fails. In that test, we assert that the method under test should return false.
>
> Obviously, this test will fail because the method under test always returns true. This is an indication of a bug in the production code and our unit test helps us find it.
>
> This is another case where you should think of your methods as block boxes when unit testing them. Don’t write tests based on the existing implementation because the exiting implementation may be incomplete and/or have a bug. Treat the method under test as a black box, give it different inputs and verify that the outcome is correct. 
>
> by Mosh

#### Writing the First Interaction Test
```csharp
[TestFixture]
public class HousekeeperServiceTests
{
    var unitOfWork = new Mock<IUnitOfWork>();
    unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
    {
        new Housekeeper {Email = "a", Oid = 1, FullName = "b", StatementEmailBody = "c"}
    }.AsQueryable());

    var statementGenerator = new Mock<IStatementGenerator>();
    var emailSender = new Mock<IEmailSender>();
    var messageBox = new Mock<IXtraMessageBox>();

    var service = new HousekeeperService(
        unitOfWork.Object,
        statementGenerator.Object,
        emailSender.Object,
        messageBox.Object);

    service.SendStatementEmails(new DateTime(2020, 12, 12));

    statementGenerator.Verify(sg => sg.SaveStatement(1, "b", new DateTime(2020, 12, 12)));
}
```
:::warning
The class of project is more high level that contain multiple different implementations such as *statementGenerator*, *emailSender*, and *messageBox*, so rename the class **HousekeeperHelper** to **HousekeeperService**.
:::

#### Keeping Tests Clean
```csharp
[TestFixture]
public class HousekeeperServiceTests
{
    private HousekeeperService _service;
    private Mock<IStatementGenerator> _statementGenerator;
    private Mock<IEmailSender> _emailSender;
    private Mock<IXtraMessageBox> _messageBox;
    private Housekeeper _houseKeeper;
    private readonly DateTime _statementDate = new DateTime(2020, 12, 12);

    [SetUp]
    public void SetUp()
    {
        _houseKeeper = new Housekeeper {Email = "a", Oid = 1, FullName = "b", StatementEmailBody = "c"};

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {_houseKeeper}.AsQueryable());

        _statementGenerator = new Mock<IStatementGenerator>();
        _emailSender = new Mock<IEmailSender>();
        _messageBox = new Mock<IXtraMessageBox>();
        _service = new HousekeeperService(
            unitOfWork.Object,
            _statementGenerator.Object,
            _emailSender.Object,
            _messageBox.Object);
    }

    [Test]
    public void SendStatementEmails_WhenCalled_GenerateStatements()
    {
        _service.SendStatementEmails(_statementDate);

        _statementGenerator.Verify(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
    }
}
```

#### Testing that a Method is Not Called
```csharp
public void SendStatementEmails(DateTime statementDate)
{
    ...
    foreach (var housekeeper in housekeepers)
    {
        // fix condition not only for null
        if (string.IsNullOrWhiteSpace(housekeeper.Email))
            continue;
        ...
    }
    ...
}

[TestFixture]
public class HousekeeperServiceTests
{
    ...
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void SendStatementEmails_HouseKeepersEmailIsNullOrWhiteSpace_ShouldNotGenerateStatements(string email)
    {
        _houseKeeper.Email = email;

        _service.SendStatementEmails(_statementDate);

        _statementGenerator.Verify(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), Times.Never);
    }
}
```

#### Another Interaction Test
```csharp
[Test]
public void SendStatementEmails_WhenCalled_EmailTheStatement()
{
    _statementGenerator
        .Setup(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
        .Returns(_statementFileName);

    _service.SendStatementEmails(_statementDate);

    _emailSender.Verify(es => es.EmailFile(
        _houseKeeper.Email,
        _houseKeeper.StatementEmailBody,
        _statementFileName,
        It.IsAny<string>()));
}
        
[Test]
[TestCase(null)]
[TestCase("")]
[TestCase(" ")]
public void SendStatementEmails_StatementFileNameIsNullOrWhiteSpace_ShouldNotEmailTheStatement(string fileName)
{
    _statementGenerator
        .Setup(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
        .Returns(fileName);

    _service.SendStatementEmails(_statementDate);

    _emailSender.Verify(es => es.EmailFile(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>()),
        Times.Never);
}
```

#### Extracting Helper Methods
```csharp
[TestFixture]
public class HousekeeperServiceTests
{
    ...
    private string _statementFileName;

    [SetUp]
    public void SetUp()
    {
        ...
        _statementFileName = "fileName";
        _statementGenerator = new Mock<IStatementGenerator>();
        _statementGenerator
            .Setup(sg => sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
            .Returns(() => _statementFileName);
        // use lambda expression let you can set fileName first, then execute the returns function
        ...
    }

    private void VerifyEmailSent()
    {
        _emailSender.Verify(es => es.EmailFile(
            _houseKeeper.Email,
            _houseKeeper.StatementEmailBody,
            _statementFileName,
            It.IsAny<string>()));
    }

    private void VerifyEmailNotSent()
    {
        _emailSender.Verify(es => es.EmailFile(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Never);
    }

    [Test]
    public void SendStatementEmails_WhenCalled_EmailTheStatement()
    {
        _service.SendStatementEmails(_statementDate);

        VerifyEmailSent();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void SendStatementEmails_StatementFileNameIsNullOrWhiteSpace_ShouldNotEmailTheStatement(string fileName)
    {
        _statementFileName = fileName;

        _service.SendStatementEmails(_statementDate);

        VerifyEmailNotSent();
    }
}
```
:::info
This is what you call proper *Unit Tests* that is **short**, **clean**, and **fast**, if you maintain too many lines in unit test that will slow you down, break easily, and take you more time to figure out what is going on.
:::

#### Testing Exceptions
```csharp
private void VerifyMessageBoxDisplay()
{
    _messageBox.Verify(mb => mb.Show(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<MessageBoxButtons>()));
}

[Test]
public void SendStatementEmails_EmailSendingFails_DisplayAMessageBox()
{
    _emailSender.Setup(es => es.EmailFile(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>())
    ).Throws<Exception>();

    _service.SendStatementEmails(_statementDate);

    VerifyMessageBoxDisplay();
}
```