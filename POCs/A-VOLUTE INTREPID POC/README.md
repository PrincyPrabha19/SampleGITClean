Hi All,

We are sharing our work on "How to deploy a UI Plugin using an Optional Package."
You can download the attached zip archive that contains the source code.

Source Code Description
=======================

Requirements
    - Visual Studio 2017
    - Redstone2 SDK


The source code is split into 3 Visual Studio Solutions :
    - MyMainApp
    - OptionalPackageNativePlugin
    - OptionalPackageManagedPlugin

MyMainApp
-----------------
The main application that will look for plugins within its optional package dependencies


OptionalPackageNativePlugin
---------------------------------------
An optional package that will deploy a WindowsRuntimeComponenent (NativeUIPlugin.dll)
This component contains a C++/XAML UserControl and is able to insert it within the main app.


OptionalPackageManagedPlugin
-------------------------------------------
An optional package that will deploy two WindowsRuntimeComponenent :
    - NativeUIPlugin
    - ManagedUIPlugin
The idea was to try to do the same thing as in the OptionalPackageNativePlugin solution
but instead of creating the C++/XAML UserControl, try to create the C#/XAML one that is referenced.


 
How to use the source code
==========================

1. First you need to have your Main Application deployed. Then if you start it, you will see a similar ui than the one from the microsoft sample.
2. Then you can deploy one or both of the optional package.
3. And restart your MainApplication, you will see tabs that will contain the ui plugins.

### Remarks :
When you uninstall the Main Application, it will uninstall its optional package automatically.



Issue faced developing these solutions
======================================

MyMainApp
----------------

### 1. How to load our plugin ?

Classical UWP App use libraries that are known at build time using References or the `DllImport` attribute.
In our case we need to load libraries discovered at runtime.
In the C++ Optional Package sample, it uses LoadLibrary to load the dll from the optional package. 
It is straight forward with the C++ Windows API, but in C# it is less obvious how to do it. 
For that I have adapted the class UnmanagedLibrary 
from [Microsoft blog](https://blogs.msdn.microsoft.com/jmstall/2007/01/06/type-safe-managed-wrappers-for-kernel32getprocaddress/)
in order to make it work with the Windows Runtime API.
See UnmanagedLibrary.cs file
Finally, the DLL that belongs to the optional package must export C Function using the stdcall calling convention in order to be callable. 


### 2. How to pass my Grid ( that will be the plugin host ) to the plugin ?

Now we are able to call our optional Package DLL from the MainApp. But we face the restriction of the exported C function, 
they cannot use C++/CX references. That means I cannot easily use this function to pass or retrieve UI Element.
To solve this, I used the `IInspectable` interface. It allows you to convert your C++/CX reference into a raw pointer and convert it back to a reference.

### 3). How to get an IInspectable Pointer from a C# Grid reference ?

The `IInspectqble` interface is part of the "Windows Runtime Library" and is not available in C#. 
I had to create a C++ Windows Runtime Component that is responsible to do the conversion of any object to a `IInspectable` pointer and return it as a `IntPtr`.





OptionalPackageNativePlugin
---------------------------------------

### 1. How to make the UserControl find its XAML ? As it is not embedded into the native DLL nor into the MainApp Appx folder

Now I am able to pass my Grid from the Main App to the Optional Package DLL. 
But during the UserControl construction, it calls `InitializeComponents()` this function is responsible to initialize the components from the XAML. 
Unfortunately it fails because it is not able to find the XAML in the MainApp Appx.
The solution is to write a custom `InitializeComponentFromOptionalPackage()` that will load the XAML from the optional package Appx. 
It also means that the optional package needs to embed (as content) all the XBF files (Compiled XAML).



OptionalPackageManagedPlugin
-------------------------------------------

### 2. How to create an UserControl that is not referenced by the Package
With this solution when it tries to create the `ManagedUIPlugin::UIPlugin` UserControl
it fails with the `REGDB_E_CLASSNOTREG` error. Indeed the .NET Core factory is not aware of this object because its assembly ManagedUIPlugin is not loaded.

### 3. How to load our ManagedUIPlugin assembly without reference it ?
In .NETFramework there is a function called `Assembly.LoadFrom(path)` but there is no such thing in .NET Core that is used for UWP.


POC Results
===========

OptionalPackageNativePlugin : WORKS
OptionalPackageManagedPlugin :  DOES NOT WORK
