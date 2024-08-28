using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class TestBase : IDisposable
    {
        public string MODEL_INPUT_FOLDER { get; init; }
        public string MODEL_OUTPUT_FOLDER { get; init; }

        public string TESTING_INPUT_FOLDER { get; init; }
        public string TESTING_OUTPUT_FOLDER { get; init; }
        public string REFERENCE_DATA_FILE { get; init; }

        public TestBase() {
            var root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var xmlroot = root + @"\XML\";
            var testingroot = root + @"\Testing\";

            MODEL_INPUT_FOLDER = xmlroot + @"Input\";
            MODEL_OUTPUT_FOLDER = xmlroot + @"Output\";
            REFERENCE_DATA_FILE = xmlroot + @"ReferenceData\ReferenceData.xml";

            TESTING_INPUT_FOLDER = testingroot + @"Input\";
            TESTING_OUTPUT_FOLDER = testingroot + @"Output\";
        }

        public void Dispose()
        {
            //todo
            //  clean Testing input and output folders
            //  delete log files
        }
    }
}
