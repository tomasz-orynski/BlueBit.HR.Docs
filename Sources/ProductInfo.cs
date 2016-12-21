using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//Plik wymieniany w całości podczas builda.

//Ustawiane w buildzie na konrektne wartości.
[assembly: AssemblyProduct("VERSION_PRODUCT")]
[assembly: AssemblyTitle("VERSION_DESCRIPTION")]
[assembly: AssemblyDescription("VERSION_DESCRIPTION")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.1.0")]
[assembly: AssemblyTrademark("")]

//Ustawiane w buildzie, ale na te same wartości.
[assembly: AssemblyCompany("BlueBit")]
[assembly: AssemblyCopyright("Copyright ©BlueBit")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyConfiguration("")]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "ProductLog.config", Watch = true)]
