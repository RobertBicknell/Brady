using Brady.GeneratorReport.XMLFileProcessor.Interfaces;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using NSubstitute;
using System;
using Xunit;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class CalculatorTests : XMLParsingBase
    {
        [Theory]
        [InlineData("01-Basic")]
        [InlineData("01-Multi")]
        public async void CompareActualToExpectedGenerationOutput(string filename)
        {
            var generationReport = await GetGenerationReportAsync($"{MODEL_INPUT_FOLDER}{filename}.xml");
            var expectedGenerationOutput = await GetGenerationOutputAsync($"{MODEL_OUTPUT_FOLDER}{filename}-Result.xml");
            var mockReferenceDataProvider = Substitute.For<IReferenceDataProvider>();
            mockReferenceDataProvider.ReferenceData.Returns(Literals.REFERENCE_DATA_LITERAL);
            var calculator = new Calculator(mockReferenceDataProvider);

            var actualGenerationOutput = calculator.Calculate(generationReport);

            Assert.True(expectedGenerationOutput.Equals(actualGenerationOutput));
        }
        //todo - port extension method tests from original project... 
    }
}
