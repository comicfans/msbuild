# MSBuild modified for YouCompleteMe
For precise semantic code complete, you need compile_commands.json for YouCompleteMe(also RTags) which records compile flags for every source file. [bear](https://github.com/rizsotto/Bear) or [btrace](https://github.com/rprichard/sourceweb) can generate compile_commands.json from existng none-CMake based project (mostly for UNIX-liked OS).  And this modified MSBuild is for existing MSVC based project(mostly for Windows).

# How it work:
msbuild project in memory is built up by manyu 'task', Microsof.Build.CPPTasks.CL is just for compiling, this components is not open sourced, but its most important logic is just in base class ToolTask (which is part of Opensourced msbuild).this mod just hook into ToolTask, ignore none-CL task(no help),ignoer skip-rebuild-check(you can still get flags even if project is built), skip actually build command, and write a compile_commands.json in msbuild current working directory. for use with clang ,this adds "--driver-mode=cl" to every commands

# This is not complete!
YouCompleteMe throws msvc style parameters incorrectly .you need YouCompleteMe with ycmd replaced [third_party/ycmd](https://github.com/comicfans/ycmd.git) . And you need to build newest clang, not the one YouCompleteMe auto downloaded (clang has bug that can not found include path, fixed in 277005 but not 3.9 binary release)



# How to build and run:
just build as original msbuild (following), some tips:
1. Choose x86 configuration. x64 will gives 'dll can not load'(I'm not familiar with dotnet and I don't know why).
2. Build whole soluation, not only msbuild project. some basic task defined in other projects is not built as msbuild's dependencies.
3. Now you can run this msbuild, but it will lookup tracker.exe in same directory (which built msbuild.exe in). you need to copy it (mostly under C:\program files(x86)\msbuild\14.0\bin  depends on your bitness or version) to directory which your built msbuild.exe in. you must copy correct bitness version (you build x86 version msbuild, so copy x86 version tracker.exe). 
4. tracker.exe will load FileTracker32/64.dll in same directory ,so you must also copy these files. I'm lasy, so I just copy everything none-conflict from C:\Program Files (x86)\MSBuild\14.0\Bin
5. now you can run this msbuild to build your project or soluation, it will gives compile_commads.json in current working dir.


# Microsoft.Build (MSBuild)
The Microsoft Build Engine is a platform for building applications. This engine, which is also known as MSBuild, provides an XML schema for a project file that controls how the build platform processes and builds software. Visual Studio uses MSBuild, but MSBuild *does not* depend on Visual Studio. By invoking msbuild.exe on your project or solution file, you can orchestrate and build products in environments where Visual Studio isn't installed.

For more information on MSBuild, see the [MSDN documentation](https://msdn.microsoft.com/en-us/library/dd393574%28v=vs.140%29.aspx).

### Build Status
Full framework build from `master` (stable, inserted into Visual Studio builds):
[![Build Status](http://dotnet-ci.cloudapp.net/job/Microsoft_msbuild/job/innerloop_master_Windows_NT_Desktop/badge/icon)](http://dotnet-ci.cloudapp.net/buildStatus/icon?job=Microsoft_msbuild/innerloop_master_Windows_NT_Desktop)

The `xplat` branch is soon to be merged back upstream. Follow the [The Great Merge](https://github.com/Microsoft/msbuild/milestone/6) milestone for progress.

| Runtime\OS | Windows | Ubuntu |Mac OS X|
|:------|:------:|:------:|:------:|
| **Full Framework** |[![Build Status](http://dotnet-ci.cloudapp.net/buildStatus/icon?job=Microsoft_msbuild/innerloop_xplat_Windows_NT_Desktop)](http://dotnet-ci.cloudapp.net/job/Microsoft_msbuild/job/innerloop_xplat_Windows_NT_Desktop/)| N/A | N/A |
|**.NET Core**|[![Build Status](http://dotnet-ci.cloudapp.net/buildStatus/icon?job=Microsoft_msbuild/innerloop_xplat_Windows_NT_CoreCLR)](http://dotnet-ci.cloudapp.net/job/Microsoft_msbuild/job/innerloop_xplat_Windows_NT_CoreCLR/)|[![Build Status](http://dotnet-ci.cloudapp.net/buildStatus/icon?job=Microsoft_msbuild/innerloop_xplat_Ubuntu_CoreCLR)](http://dotnet-ci.cloudapp.net/job/Microsoft_msbuild/job/innerloop_xplat_Ubuntu_CoreCLR/)|[![Build Status](http://dotnet-ci.cloudapp.net/buildStatus/icon?job=Microsoft_msbuild/innerloop_xplat_OSX_CoreCLR)](http://dotnet-ci.cloudapp.net/job/Microsoft_msbuild/job/innerloop_xplat_OSX_CoreCLR/)|

[![Join the chat at https://gitter.im/Microsoft/msbuild](https://badges.gitter.im/Microsoft/msbuild.svg)](https://gitter.im/Microsoft/msbuild?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Check out what we're working on using our Waffle board!](https://badge.waffle.io/Microsoft/msbuild.svg?label=In+Progress&title=waffle+board)](http://waffle.io/Microsoft/msbuild)

### Source code

* Clone the sources: `git clone https://github.com/Microsoft/msbuild.git`

### Building
## Building MSBuild in VS 2015
For the full supported experience, you will need to have Visual Studio 2015. You can open the solution in Visual Studio 2013, but you will encounter issues building with the provided scripts.

To get started on **Visual Studio 2015**:

1. [Install Visual Studio 2015](http://www.visualstudio.com/en-us/downloads/visual-studio-2015-downloads-vs).  Select the following optional components:
  - _Microsoft Web Developer Tools_
  - _Universal Windows App Development Tools_
    - _Tools and Windows SDK 10.0.10240_
2. Clone the source code (see above).
3. Restore NuGet packages: `msbuild /t:BulkRestoreNugetPackages build.proj`
4. Open src/MSBuild.sln solution in Visual Studio 2015.

## Building MSBuild in Unix (Mac & Linux)
MSBuild's xplat branch allows MSBuild to be run on Unix Systems. Set-up instructions can be viewed on the wiki:   [Building Testing and Debugging on .Net Core MSBuild](https://github.com/Microsoft/msbuild/wiki/Building-Testing-and-Debugging-on-.Net-Core-MSBuild)

## How to Engage, Contribute and Provide Feedback
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

Before you contribute, please read through the contributing and developer guides to get an idea of what kinds of pull requests we will or won't accept.

* [Contributing Guide](https://github.com/Microsoft/msbuild/wiki/Contributing-Code)
* [Developer Guide](https://github.com/Microsoft/msbuild/wiki/Building-Testing-and-Debugging)

Want to get more familiar with what's going on in the code?
* [Pull requests](https://github.com/Microsoft/msbuild/pulls): [Open](https://github.com/Microsoft/msbuild/pulls?q=is%3Aopen+is%3Apr)/[Closed](https://github.com/Microsoft/msbuild/pulls?q=is%3Apr+is%3Aclosed)
* [Issues](https://github.com/Microsoft/msbuild/issues)

You are also encouraged to start a discussion by filing an issue or creating a gist.

## MSBuild Components

* **MSBuild**. [Microsoft.Build.CommandLine](https://msdn.microsoft.com/en-us/library/dd393574(v=vs.120).aspx)  is the entrypoint for the Microsoft Build Engine (MSBuild.exe).

* **Microsoft.Build**. The [Microsoft.Build](https://msdn.microsoft.com/en-us/library/gg145008(v=vs.120).aspx) namespaces contain types that provide programmatic access to, and control of, the MSBuild engine.

* **Microsoft.Build.Framework**. The [Microsoft.Build.Framework](https://msdn.microsoft.com/en-us/library/microsoft.build.framework(v=vs.120).aspx) namespace contains the types that define how tasks and loggers interact with the MSBuild engine. For additional information on this component, see our [Microsoft.Build.Framework wiki page](https://github.com/Microsoft/msbuild/wiki/Microsoft.Build.Framework).

* **Microsoft.Build.Tasks**. The [Microsoft.Build.Tasks](https://msdn.microsoft.com/en-us/library/microsoft.build.tasks(v=vs.120).aspx) namespace contains the implementation of all tasks shipping with MSBuild.

* **Microsoft.Build.Utilities**. The [Microsoft.Build.Utilities](https://msdn.microsoft.com/en-us/library/microsoft.build.utilities(v=vs.120).aspx) namespace provides helper classes that you can use to create your own MSBuild loggers and tasks.

## License

MSBuild is licensed under the [MIT license](LICENSE).
