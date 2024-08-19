# SmiteUnit <img src="logo.svg" align="right" width="100">

The ***S***ubprocess ***M***ethod ***I***njection ***Te***st Framework 


## What is SmiteUnit?
SmiteUnit is a testing framework for use in environments where a traditional unit testing framework cannot be used.

Common examples of using SmiteUnit include:
* Testing a plugin for an application
* Testing a mod for a video game
* Running automated integration tests

## Packages
| Name | Description | Latest Version | 
|--------------|---------|:--------------:|
| Linkoid.SmiteUnit | A bundle that includes all important packages for using SmiteUnit *except `SmiteUnit.Injection`*. | [![NuGet Version](https://img.shields.io/nuget/v/Linkoid.SmiteUnit)](https://www.nuget.org/packages/Linkoid.SmiteUnit/) | 
| Linkoid.SmiteUnit.Framework | Includes the framework for writing tests using SmiteUnit. Tests using the framework cannot be run by IDEs or other tools without the `SmiteUnit.TestAdapter`. | [![NuGet Version](https://img.shields.io/nuget/v/Linkoid.SmiteUnit.Framework)](http://www.nuget.org/packages/Linkoid.SmiteUnit.Framework/) |
| Linkoid.SmiteUnit.TestAdapter | An adapter for `SmiteUnit.Framework` that allows running tests from an IDE or from the commandline with `dotnet test`. | [![NuGet Version](https://img.shields.io/nuget/v/Linkoid.SmiteUnit.TestAdapter)](http://www.nuget.org/packages/Linkoid.SmiteUnit.TestAdapter/) |
| Linkoid.SmiteUnit.Injection | Includes the assembly required for injecting SmiteUnit tests into application. | [![NuGet Version](https://img.shields.io/nuget/v/Linkoid.SmiteUnit.Injection)](http://www.nuget.org/packages/Linkoid.SmiteUnit.Injection/) |



## Usage
To start using SmiteUnit, injection points must be added to the target program, and a seperate test project should be made to hold the tests.

### Adding Injection Points
Injection points need to be added to the program in which the tests must be run. To do this, the `SmiteUnit.Injection` package will be used.
Injection points control when the tests are started and when the tests close the program.
Where these injection points go will be different depending on the program, but will have a similar idea to this example:
```cs --region InjectionExample --source-file ./Docs/Program.cs --project ./Docs/Snippets.csproj
using SmiteUnit.Injection;

public static class Program
{
    public static void Main()
    {
        // Near startup a SmiteInjection object should be created and it's EntryPoint() method called.
        // Create it with the name of the assembly that holds the tests.
        var smiteInjection = new SmiteInjection("MyTestAssembly");

        // Call the entry point methods. Tests will start running here. 
        smiteInjection.EntryPoint();

        // If the program is interactive, there is likely some sort of update loop.
        bool updateLoop = true;
        while (updateLoop) 
        {
            // Inside of this update loop, UpdatePoint should be periodically called.
            smiteInjection.UpdatePoint();
        }

        // Finally, before the program exits, ExitPoint() should be called.
        smiteInjection.ExitPoint();

        System.Environment.Exit(0);
    }
}
```

### Creating a Test Project

Creating a test project follows similar steps to other C# testing frameworks.
It is recomended to follow the steps to set up tests for a popular framework like [NUnit](https://docs.nunit.org/articles/nunit/getting-started/installation.html).
After setting up a test project for your IDE, open up the `.csproj` file,
and remove references to the popular testing framework (e.g. if NUnit's instructions were followed, delete PackageReferences to NUnit) and replace them with package references to SmiteUnit.
The package references in the .csproj will probably end up looking like this:
```xml --source-file ./Docs/ExampleTestProject.csproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- This could be any supported framework -->
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- These package references can stay -->
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.11.0" />
    <PackageReference Include="coverlet.collector" Version="3.1.0" GeneratePathProperty="true" />

    <!-- These package references should be removed -->
    <!-- <PackageReference Include="NUnit3TestAdapter" Version="4.0.0" /> -->
    <!-- <PackageReference Include="NUnit" Version="3.14.*" /> -->

    <!-- This package reference should be added -->
    <PackageReference Include="Linkoid.SmiteUnit" Version="0.3.0-alpha" />
    
  </ItemGroup>
</Project>
```

### Writing Tests with SmiteUnit
To start writing tests with SmiteUnit, create a new class and add a `[SmiteProcess]` attribue to it,
then mark test methods with the `[SmiteTest]` attribute.

```cs --region FrameworkExample --source-file ./Docs/ReadmeExamples.cs --project ./Docs/Snippets.csproj
// The SmiteProcessAttribute tells the test adapter which program to start
[SmiteProcess("MyExecutable.exe", "arguments")]
public static class MySmiteTests
{
    // The SmiteSetUpAttribute marks methods that should run before any test methods
    [SmiteSetUp]
    public static void MySetUpMethod()
    {
        Console.WriteLine("Running my set up method!");
    }

    // The SmiteTestAttribute marks methods that can be run as tests
    [SmiteTest]
    public static void MyHelloWorldTest()
    {
        Console.WriteLine("Hello World!");
    }
}
```

### Running SmiteUnit Tests
SmiteUnit does not provide any tools for running tests, but it is compatible with `Microsoft.NET.Test.Sdk` via the `SmiteUnit.TestAdapter` which is automatically included in the `SmiteUnit` package.
Tests should be runnable from an IDE that supports `Microsoft.NET.Test.Sdk`, or can be run from the commandline with `dotnet test`.

## How SmiteUnit Works
SmiteUnit works by running a test in a sub process and capturing its input and output.
Somewhere in this process there is a hook that checks for a specific test that the parent process is attempting to invoke.
At the injection point if a valid test is found, the test is executed and the result is reported back to the parent process.
What sets this library apart from other testing frameworks is the test writer has complete control over where the injection point is.
This means that if the only way your code can possibly execute properly is 
as an injected dependency inside of another application that perhaps doesn't even have a proper debug mode,
you will still be able to run these tests in an automated fashion.

SmiteUnit is also designed to function well with other testing frameworks 
and it is even possible to run SmiteUnit inside of unit test Written in a different framework.
This would even be the ideal use case in situations where specific input and output of the application 
needs to be tested for instantce standard input and standard output.

## Key Design Requirements
* SmiteUnit should be usable in any program where the SmiteUnit assembly can be loaded and executed.
* The program in which the test is executed must be viewed as a black box.

