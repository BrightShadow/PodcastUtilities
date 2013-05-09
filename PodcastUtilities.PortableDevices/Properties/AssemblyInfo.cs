﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PodcastUtilities.PortableDevices")]
[assembly: AssemblyDescription("Windows Portable Device adapter")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c1ab5326-50d3-4042-9b00-8db5846f03ff")]

[assembly: CLSCompliant(true)]

[assembly: InternalsVisibleTo("PodcastUtilities.PortableDevices.Tests")]

// we cannot have a strong name as we depend on LinFu Core and it does not have a string name
[module: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

// Version information is in the shared AssemblyInfo