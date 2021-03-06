﻿using Azure.Functions.Cli.Tests.E2E.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Azure.Functions.Cli.Tests.E2E
{
    public class CreateFunctionTests : BaseE2ETest
    {
        public CreateFunctionTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task create_template_function_success_message()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime dotnet",
                    "new --template HttpTrigger --name testfunc"
                },
                OutputContains = new[]
                {
                    "The function \"testfunc\" was created successfully from the \"HttpTrigger\" template."
                }
            }, _output);
        }

        [Fact]
        public async Task create_template_function_sanitization_dotnet()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init 12n.e.0w-file$ --worker-runtime dotnet",
                    "new --prefix 12n.e.0w-file$ --template HttpTrigger --name 12@n.other-file$"
                },
                CommandTimeout = new TimeSpan(0, 1, 0),
                CheckFiles =  new[]
                {
                    new FileResult
                    {
                        Name = "12n.e.0w-file$/_12_n_other_file_.cs",
                        ContentContains = new[]
                        {
                            "namespace _12n.e__w_file_",
                            "public static class _12_n_other_file_"
                        }
                    }
                }
            }, _output);
        }

        [Fact]
        public async Task create_template_function_using_alias()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime node",
                    "new --language js --template \"http trigger\" --name testfunc"
                },
                OutputContains = new[]
                {
                    "The function \"testfunc\" was created successfully from the \"http trigger\" template."
                }
            }, _output);
        }

        [Fact]
        public async Task create_template_function_js_no_space_name()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime node",
                    "new --language js --template httptrigger --name testfunc"
                },
                OutputContains = new[]
                {
                    "The function \"testfunc\" was created successfully from the \"httptrigger\" template."
                }
            }, _output);
        }

        [Fact]
        public async Task create_template_function_dotnet_space_name()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime dotnet",
                    "new --template \"http trigger\" --name testfunc2"
                },
                OutputContains = new[]
                {
                    "The function \"testfunc2\" was created successfully from the \"http trigger\" template."
                }
            }, _output);
        }

        [Fact]
        public async Task create_typescript_template()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime node --language typescript",
                    "new --template \"http trigger\" --name testfunc"
                },
                CheckFiles = new FileResult[]
                {
                    new FileResult
                    {
                        Name = Path.Combine("testfunc", "function.json"),
                        ContentContains = new []
                        {
                            "../dist/testfunc/index.js",
                            "authLevel",
                            "methods",
                            "httpTrigger"
                        }
                    }
                },
                OutputContains = new[]
                {
                    "The function \"testfunc\" was created successfully from the \"http trigger\" template."
                }
            }, _output);
        }

        [Fact]
        public async Task create_typescript_template_blob()
        {
            await CliTester.Run(new RunConfiguration
            {
                Commands = new[]
                {
                    "init . --worker-runtime node --language typescript",
                    "new --template \"azure Blob Storage trigger\" --name testfunc"
                },
                CheckFiles = new FileResult[]
                {
                    new FileResult
                    {
                        Name = Path.Combine("testfunc", "function.json"),
                        ContentContains = new []
                        {
                            "../dist/testfunc/index.js",
                            "blobTrigger"
                        },
                        ContentNotContains = new []
                        {
                            "authLevel",
                            "methods"
                        }
                    }
                },
                OutputContains = new[]
                {
                    "The function \"testfunc\" was created successfully from the \"azure Blob Storage trigger\" template."
                }
            }, _output);
        }
    }
}
