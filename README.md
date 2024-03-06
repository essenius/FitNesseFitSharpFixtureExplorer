# Fixture Explorer ![workflow badge](../../actions/workflows/fixture-explorer-ci.yml/badge.svg)

Fixture to identify API of fixture assemblies using reflection (and documentation convention in the assembly).

## Installation
The steps to install are very similar to that of installing the [FibonacciDemo](../../../FitNesseFitSharpFibonacciDemo).

Differences are:
* Download the repo code as a zip file and extract the contents of the folder `FitNesseFitSharpFixtureExplorer-master`. 
* Go to the solution folder: `cd /D %LOCALAPPDATA%\FitNesse\FixtureExplorer`
* If you have .NET 6 SDK installed:
    * Build solution: `dotnet build --configuration release FixtureExplorer.sln`
    * No need to publish since we don't need dependencies
* If you don't have .NET 6 SDK installed: download FixtureExplorer.zip from the latest [release](../../releases) and extract into `FixtureExplorer\FixtureExplorer`
* Go to the assembly folder folder: `cd FixtureExplorer\bin\Release\net6.0`
* Start FitNesse
* Run the suite: Open a browser and enter the URL http://localhost:8080/FitSharpDemos.FixtureExplorerTest?test

## Tutorial and Reference
See the [Wiki](../../wiki)

## Contribute
Enter an issue or provide a pull request. 
