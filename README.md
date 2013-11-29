#Root-VSIX
Installs local VSIX extensions to custom Visual Studio RootSuffixes.

##Usage

    Root-VSIX [<VS version>] <RootSuffix> <Path to VSIX>

The Visual Studio version must be specified as the internal version number (12.0 is 2013).  If omitted, the extension will be installed to the latest version of Visual Studio installed on the computer.

#FAQ

 - What if I specify a RootSuffix that doesn't exist?
  - The extension installer will create the RootSuffix automatically; launching Visual Studio with that RootSuffix will still ask for default settings
