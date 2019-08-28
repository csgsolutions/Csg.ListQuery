# Introduction 
This repository provides the basic folder structure for a .NET Core project.

## Folders & Files

### build

[![Build status](https://ci.appveyor.com/api/projects/status/hx4so74ja1y42lfw/branch/master?svg=true)](https://ci.appveyor.com/project/jusbuc2k/csg-data/branch/master)

This folder contains repository-wide settings, targets, properties related to building the project. ```repo.props``` contains MSBuild properties that will apply to all projects in the repository. ```sources.props``` contains NuGet 
repositories that will be used when restoring packages to projects in the repository.

### src
This folder should contain a sub-folder for each project. For example, MyProject.Web for a website, or MyProject.Console for a console app, etc. Short names could also be used, such as Web, or Console. The contents of the ```Directory.Build.props``` file automatically gets included in any project in this folder.

### tests
This folder should contain a sub-folder for each test project. Project names should end with .UnitTests and .IntegrationTests as appropriate. Naming should be consistent with the projects in /src/, such as MyProject.Web.UnitTests or Web.UnitTests.

### boostrap.ps1
Do not modify this file. It is used by the build.ps1

### build.cmd
Batch file used to invoke build.ps1 in Windows environments

### build.ps1
The build script for this project. Running this script runs a restore, build, test and pack/publish cycle. Modify this script as appropriate for your project.

### version.props
Contains MSBuild properties to define the version number of assemblies built in the /src/ folder.
