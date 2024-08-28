using Brady.GeneratorReport.XMLFileProcessor.DTOs.Output;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using System;
using Xunit;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class XMLWriterTests : XMLParsingBase
    {
        [Theory]
        [InlineData("01-Basic-Result.xml")]
        public async void CheckCanWriteGenerationOutput(string filename)
        {
            //todo have Dispose empty directories used, delete log file...
            var expectedGenerationOutput = await GetGenerationOutputAsync($"{MODEL_OUTPUT_FOLDER}{filename}");
            var writer = new XMLWriter<GenerationOutput>();
            await writer.TryWriteAsync($"{TESTING_OUTPUT_FOLDER}{filename}", expectedGenerationOutput);

            var actualGenerationOutput = await GetGenerationOutputAsync($"{TESTING_OUTPUT_FOLDER}{filename}");

            Assert.True(expectedGenerationOutput.Equals(actualGenerationOutput));
        }
        //todo add tests to exerise exceptions
    }
}
