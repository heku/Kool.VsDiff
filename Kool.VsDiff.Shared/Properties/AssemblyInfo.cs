﻿using Kool.VsDiff;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(Vsix.PRODUCT + "." + Vsix.PACKAGE)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct(Vsix.PRODUCT)]
[assembly: AssemblyCopyright(Vsix.GITHUB)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Vsix.VERSION)]

// https://docs.microsoft.com/en-us/visualstudio/extensibility/localizing-menu-commands
// https://www.codeproject.com/Tips/648517/Error-Resolved-IOException-Cannot-locate-resource
// https://msdn.microsoft.com/en-us/library/dd997383(v=vs.100).aspx#New%20Specific%20Cultures
[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.Satellite)]