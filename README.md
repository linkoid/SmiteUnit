# SmiteUnit
***S***ubprocess ***M***ethod ***I***nvocation ***Te***st Library

## What is SmiteUnit?
SmiteUnit is a testing library for use in environments where a traditional unit testing framework cannot be used.

Common examples of using SmiteUnit include:
* Testing a plugin for an application
* Testing a mod for a video game
* Running automated integration tests

SmiteUnit works by running a test in a sub process and capturing its input and output.
Somewhere in this process there is a hook that checks for a specific test that the parent process is attempting to invoke.
At the injection point if a valid test is found, the test is executed and the result is reported back to the parent process.
What sets this library apart from other testing frameworks is the test writer has complete control over where the injection point is.
This means that if the only way your code can possibly execute properly is 
as an injected dependency inside of another application that perhaps doesn't even have a proper debug mode,
you will still be able to run these tests in an automated fashion.

Smite Lib is also designed to function well with other testing frameworks 
and it is even possible to run SmiteUnit inside of unit test Written in a different framework.
This would even be the ideal use case in situations where specific input and output of the application 
needs to be tested for instantce standard input and standard output.

## Key Design Requirements
* SmiteUnit should be usable in any program where the SmiteUnit assembly can be loaded and executed.
* The program in which the test is executed must be viewed as a black box.


