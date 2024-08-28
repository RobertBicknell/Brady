using Brady.GeneratorReport.XMLFileProcessor.DTOs.Input;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Output;
using Brady.GeneratorReport.XMLFileProcessor.Interfaces;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using Xunit;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class AppTests : XMLParsingBase
    {
        //todo lots more test cases needed
        //  exercise exceptions, logging etc
        //  add test for when a lockfile already exists but is not locked...

        [Fact]
        public async void ComponentOrchestration_HappyPath()
        {
            var mockLogger = Substitute.For<ILogger<IComponent>>();

            var mockAppSettings = Substitute.For<IOptions<AppSettings>>();
            mockAppSettings.Value.Returns(new AppSettings { });

            var mockWatcher = Substitute.For<IWatcher>();

            var mockReportParser = Substitute.For<IParser<GenerationReport>>();
            var mockParseResult = await GetGenerationReportAsync(MODEL_INPUT_FOLDER + "01-Basic.xml");
            mockReportParser.TryParseAsync(Arg.Any<string>()).Returns(mockParseResult);

            var mockCalculator = Substitute.For<ICalculator>();
            var mockCalculatorResult = await GetGenerationOutputAsync(MODEL_OUTPUT_FOLDER + "01-Basic-Result.xml");
            mockCalculator.Calculate(Arg.Any<GenerationReport>()).Returns(mockCalculatorResult); //todo use/check arg instead of Any..

            var mockWriter = Substitute.For<IWriter<GenerationOutput>>();
            var mockLocker = Substitute.For<ILocker>();

            var app = new App(mockLocker, mockAppSettings, mockLogger, mockWatcher, mockReportParser, mockCalculator, mockWriter);
            await app.Run(null);
            mockWatcher.NewXMLFile += Raise.EventWith<XMLWatchEventArgs>(this, new XMLWatchEventArgs(this, "test.xml"));

            //check the parser was called 
            await mockReportParser.Received(1).TryParseAsync(Arg.Any<string>());

            //check the calculator was called 
            mockCalculator.Received(1).Calculate(Arg.Any<GenerationReport>());

            //check the writer was called 
            await mockWriter.Received(1).TryWriteAsync(Arg.Any<string>(), Arg.Any<GenerationOutput>());
        }
    }
}
