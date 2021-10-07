
Dell Client Framework SDK

----------------
Getting Started:
----------------
The solution contains sample code for building plugins for the Dell Client Framework (DCF).  If you received
this code via the DellClientFrameworkSDK.zip file, then it contains all files necessary to build and test
these samples.  Simply build the solution.

If you checked this code out from the Git/Stash repository, then the binary directory will be empty 
and you will not be able to build this solution until you copy the appropriate DCF artifacts into the
"bin" directory.  This is because these samples have references to DCF binaries in the bin\$(Config) directory.

The 'bin' directory should contain the following DCF artifacts:
	bin\Debug\DCF*.*
	bin\Debug\TestApp*.*
	bin\Debug\Util*.*
	bin\Release\DCF*.*

----------------
Source Examples:
----------------

This SDK contains various projects to show how to write plugins for DCF Agents and Consoles.

	Dell.Client.Samples.Agent.ConsoleApp - shows how to run the Agent in a console application instead of
		a Windows service.  By default the Agent requires elevated privileges.  If it is desired that the
		Agent run without elevated privileges, it will execute with slightly reduced functionality provided
		that the AgentConfig.AllowUnelevatedExecution is set to true.  This project demonstrates this ability.

	Dell.Client.Samples.Agent.Plugin*.* - contains source for sample Agent plugins that demonstate various
		abilities and features of the framework.  These may be tested by running bin\debug\TestApp.ClientFramework.exe.

	Dell.Client.Samples.Console - builds a sample user console (system tray) application.  This application is
		used to test the Console plugins.

	Dell.Client.Samples.Console.Plugins - builds a single class library that contains multiple types of console
		plugins (IConsolePlugin) for configuring a home screen, pages, panels, and a wizard.
		
	Dell.POC.* - various libraries for use for DCF Proof-of-Concept projects that are shown at Dell World.
	
----------
Debugging:
----------
This SDK contains several test and utility applications for debugging your plugins.  Some include external configuration
files for customizing your application.
	
	TestApp.ClientFramework.exe - runs the Agent in a WinForms application and includes a realtime logger. This
		is useful for debugging IAgentPlugins without having to run them in a Windows service.  This is the most
		common application for debugging and testing an Agent plugin.

	TestApp.Agent.exe - runs the Agent in a Cmd prompt.

	TestApp.UserProcess.exe - runs the UserProcess in a Cmd prompt (useful for debugging IUserProcess plugins)

	Utils.LogListener.exe - attaches to the Agent logging service and shows log messages in realtime.

	Utils.StopFramework.exe - stops all Agent processes including any UserProcess instances.  This is useful if the
		User process is started by the Agent and the Agent is not stopped before it terminates.  In this case the
		User process will be left running and your IUserProcess plugins and all dependent assemblies will be locked.
		If try to build your UserProcess plugins and get "can access XXX file because it is in use by another process", 
		you have a Agent process running and need to run this utility application.

-------------
Code Signing:
-------------
The DEBUG version of DCF does not validate plugins to insure they are correctly signed with a known certificate.
This allows you to develop plugins without having to sign your binaries.  Note that the RELEASE version of DCF does
require your plugins to be signed with a valid Dell certificate in order to be loaded in the the Framework.

--------
Contact:
--------
The Dell Client Framework and the SDK are released via Confluence at https://confluence.cpgswtools.com/pages/viewpage.action?pageId=83103988.
If you have any questions, please contact doug_gillespie@dell.com.


