# FitNesseFitSharpFixtureExplorer
Fixture to identify API of fixture assemblies using reflection (and documentation convention in the assembly).

# Installation
The steps to install are very similar to that of installing the [FibonacciDemo](../../../FitNesseFitSharpFibonacciDemo).

Differences are:
* Download the repo code as a zip file and extract the contents of the folder `FitNesseFitSharpFixtureExplorer-master`. 
* Go to the solution folder: `cd /D %LOCALAPPDATA%\FitNesse\FixtureExplorer`
* Build solution: `dotnet build --configuration release FixtureExplorer.sln`
* No need to publish since we don't need dependencies
* Go to folder: `cd FixtureExplorer\bin\Release\net5.0`
* Start FitNesse
* Run the suite: Open a browser and enter the URL http://localhost:8080/FitSharpDemos.FixtureExplorTest?test

# Tutorial and Reference
See the [Wiki](../../wiki)

# Contribute
Enter an issue or provide a pull request. 
