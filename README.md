# FitNesseFitSharpFixtureExplorer
Fixture to identify API of fixture assemblies using reflection (and documentation convention in the assembly).

# Getting Started
1. Ensure you have Java installed (1.8 or higher)
1. Download FitNesse (http://fitnesse.org) and install it to ```C:\Apps\FitNesse```
1. Download FitSharp 2.8.2.1 or higher from https://www.nuget.org/packages/fitsharp/
1. Rename the pkg file to a zip file and extract the contents of the lib folder to ```C:\Apps\FitSharp\2.8.2.1``` (use the version number that you downloaded). <br/>Alternatively, use ```dotnet nuget install fitsharp``` and change the FITSHARP_HOME variable in ```plugins.properties``` to ```${USERPROFILE}\.nuget\packages\fitsharp\2.8.2.1\lib\net5.0```, or copy the contents of that folder to ```C:\Apps\FitSharp\2.8.2.1\net5.0```
1. Clone the repo to a local folder (```C:\Data\FitNesseDemo```)
1. If you took a different folder name, update ```plugins.properties``` to point to the right folder
1. Build all projects in the solution FixtureExplorer
1. Start FitNesse with the root repo folder  as the data folder, and the assembly folder as the current directory:
```
    cd /D C:\Data\FitNesseDemo\FixtureExplorer\FixtureExplorer\bin\debug\net5.0
    java -jar C:\Apps\FitNesse\fitnesse-standalone.jar -d C:\Data\FitNesseDemo -e 0
```

8. Open a browser and enter the URL http://localhost:8080/FitSharpDemos.FixtureExplorerTest?test

# Contribute
Enter an issue or provide a pull request.
