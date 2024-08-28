using System;
using Xunit;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Config;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Output;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Input;
using Brady.GeneratorReport.XMLFileProcessor.Exceptions;
using System.Linq;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class XMLParserTests : TestBase //passing tests here justify the use of the XMLParsingBase test class to load referencedata, input files for processing and output files for comparison 
    {
        private readonly XMLParser<ReferenceData> _referenceDataParser = new XMLParser<ReferenceData>();
        private readonly XMLParser<GenerationReport> _generationReportParser = new XMLParser<GenerationReport>();
        private readonly XMLParser<GenerationOutput> _generatorOutputParser = new XMLParser<GenerationOutput>();

        [Fact]
        public async void ParseReferenceDataFromXMLFile_Expect_HighValueMediumEmissionFactors()
        {
            const decimal expectedValueFactorHigh = 0.946m;
            const decimal expectedEmissionFactorMed = 0.562m;

            var referenceData = await _referenceDataParser.TryParseAsync(REFERENCE_DATA_FILE); 

            Assert.Equal(expectedValueFactorHigh, referenceData.Factors.ValueFactor.High);
            Assert.Equal(expectedEmissionFactorMed, referenceData.Factors.EmissionsFactor.Medium);
        }

        [Fact]
        public async void ParseGeneratorReportFromXMLFile_Expect_WindOffshoreJan1EnergyPriceLocation()
        {
            var path = MODEL_INPUT_FOLDER + "01-Basic.xml";
            var generationReport = await _generationReportParser.TryParseAsync(path);

            const string windGeneratorJan1Name = "Wind[Offshore]";
            const decimal expectedEnergy = 100.368m;
            const decimal expectedPrice = 20.148m;
            const string expectedLocation = "Offshore";
            var Jan1 = new DateTime(2017, 1, 1);

            var windGenerator = generationReport.Wind.Where(w => w.Name == windGeneratorJan1Name).First();
            var windGenerationJan1 = windGenerator.Generation.Where(d => d.Date == Jan1).First();

            Assert.Equal(expectedEnergy, windGenerationJan1.Energy);
            Assert.Equal(expectedPrice, windGenerationJan1.Price);
            Assert.Equal(expectedLocation, windGenerator.Location);
            Assert.Equal(Jan1, windGenerationJan1.Date);
        }

        [Fact]
        public async void ParseGeneratorOutputFromXMLFile_Expect_TotalEmissionActualHeatRate() //not a functional requirement, but allows for comparing output to known-good files
        {
            var path = MODEL_OUTPUT_FOLDER + "01-Basic-Result.xml";
            var generationOutput = await _generatorOutputParser.TryParseAsync(path);

            const string coalGeneratorTotalsName = "Coal[1]";
            const decimal expectedCoalTotal = 5341.716526632m;
            var actualTotal = generationOutput.Totals.Where(t => t.Name == coalGeneratorTotalsName).Select(t => t.Total).First();
            Assert.Equal(expectedCoalTotal, actualTotal);

            const string gasEmissionGeneratorJan3Name = "Gas[1]";
            const decimal expectedEmissionJan3 = 5.132380700m;
            var expecteEmissionJan3Date = new DateTime(2017, 1, 3);
            var actual = generationOutput.MaxEmissionGenerators.Where(g => g.Name == gasEmissionGeneratorJan3Name).Select(g => new { Emission = g.Emission, Date = g.Date }).First();
            Assert.Equal(expectedEmissionJan3, actual.Emission);
            Assert.Equal(expecteEmissionJan3Date, actual.Date);

            const string coalGeneratorHeatRateName = "Coal[1]";
            const decimal expectedActualHeatRate = 1m;
            var actualActualHeatRate = generationOutput.ActualHeatRates.Where(a => a.Name == coalGeneratorHeatRateName).Select(a => a.HeatRate).First();
            Assert.Equal(expectedActualHeatRate, actualActualHeatRate);
        }


        [Fact]
        public async void LoadBadXMLFile_Expect_XMLParserException()
        {
            var path = MODEL_INPUT_FOLDER + "01-Error.xml";
            await Assert.ThrowsAsync<XMLParserException>( async () =>
            {
                var generationReportXMLError = await _generatorOutputParser.TryParseAsync(path);
            });
        }

        [Fact]
        public async void LoadUnavailableFile_Expect_XMLParserException()
        {
            var path = MODEL_INPUT_FOLDER + "DoesNotExist.xml";
            await Assert.ThrowsAsync<XMLParserException>(async () =>
            {
                var generationReportXMLError = await _generatorOutputParser.TryParseAsync(path);
            });
        }

        //todo - consider testing malicious xml files / xdos

    }
}
