using Brady.GeneratorReport.XMLFileProcessor.DTOs.Input;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class e2eTests : XMLParsingBase
    {
        //todo - test further items, e.g. bad inputs<->exceptions, stdout, logfile, locking, disk space, throughput etc.. 

        System.Diagnostics.Process process = null;
        string outputFilename = null;
        string inputFolderFilename = null;

        [Fact]
        public async void Test1() //todo - hack - very brittle test that needs rework. included so there is at least one e2e test.
        {
            try
            {
                var current = Assembly.GetExecutingAssembly().Location;
                var cutPoint = current.IndexOf(@"\Brady.GeneratorReport.XMLFileProcessor.Tests");
                const string exeName = "Brady.GeneratorReport.XMLFileProcessor.exe";
                const string exePartialPath = @"\Brady.GeneratorReport.XMLFileProcessor\bin\Debug\net5.0\";

                //start exe
                var root = current.Substring(0, cutPoint);
                var exe = root + exePartialPath + exeName;
                StartExe(root, exePartialPath, exe);
                await Task.Delay(TimeSpan.FromSeconds(5)); //could take a while for the filesystemwatcher to register..

                //write valid generator report to input directory
                string config = root + @"\Brady.GeneratorReport.XMLFileProcessor\appSettings.json";
                var json = File.ReadAllText(config);
                var settings = JsonConvert.DeserializeObject<Facade.AppSettings>(json).App;
                inputFolderFilename = settings.InputDirectory + "test.xml";
                var generationReport = await GetGenerationReportAsync($"{MODEL_INPUT_FOLDER}01-Basic.xml");
                await (new XMLWriter<GenerationReport>()).TryWriteAsync(inputFolderFilename, generationReport);

                //check expected output file exists
                await Task.Delay(TimeSpan.FromSeconds(5)); //too big, but safe
                outputFilename = settings.OutputDirectory + "test-Result.xml";
                var success = File.Exists(outputFilename);
                Assert.True(success);
            }
            finally {
                CleanUp();
            }
        }

        void StartExe(string root, string exePartialPath, string exe) {
            process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = root + exePartialPath;
            process.StartInfo.FileName = exe;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        void CleanUp() {
            try {
                process?.Kill();
            }
            finally {
                try
                {
                    File.Delete(inputFolderFilename);
                    File.Delete(outputFilename);
                }
                finally { }
            }
        }
    }

    namespace Facade
    {
        class AppSettings //hack to ease deserialization without help from config services derefencing the "App" section
        {
            public App App { get; set; }
        }

        class App
        {
            public string InputDirectory { get; set; }
            public string OutputDirectory { get; set; }
            public string ReferenceDataFile { get; set; }
        }
    }
}
