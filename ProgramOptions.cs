using CommandLine;
using CommandLine.Text;
using System;

namespace Root_VSIX
{
    class ProgramOptions
    {
        [Option("visualStudioVersion", Required = false, HelpText = "The Visual Studio version (12.0 for VS2013, 14.0 for VS2015). If omitted, the extension will be installed to the latest installed version of Visual Studio.")]
        public string VisualStudioVersion { get; set; }

        [Option("rootSuffix", Required = false, HelpText = "The root suffix of Visual Studio to install into.")]
        public string RootSuffix { get; set; }

        [Option("vsixPath", Required = true, HelpText = "The path to the VSIX file to install.")]
        public string VSIXPath { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading                      = new HeadingInfo("Root-VSIX", "1.0.0.0"),
                Copyright                    = new CopyrightInfo("SLaks", 2016),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption            = true
            };
            
            help.AddPreOptionsLine("Installs local VSIX extensions to default or custom Visual Studio RootSuffixes" + Environment.NewLine);
            help.AddPreOptionsLine("Usage:");
            help.AddOptions(this);

            HelpText.DefaultParsingErrorsHandler(this, help);

            return help;
        }
    }
}
