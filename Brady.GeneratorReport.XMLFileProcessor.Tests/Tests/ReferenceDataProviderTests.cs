using Brady.GeneratorReport.XMLFileProcessor.DTOs.Config;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using System;
using Xunit;
using NSubstitute;
using Microsoft.Extensions.Options;
using Brady.GeneratorReport.XMLFileProcessor.Interfaces;
using Microsoft.Extensions.Logging;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class ReferenceDataProviderTests : XMLParsingBase
    {
        ReferenceData REFERENCE_DATA_LITERAL = new ReferenceData
        {
            Factors = new Factors
            {
                ValueFactor = new ValueFactor
                {
                    High = 0.946m,
                    Medium = 0.696m,
                    Low = 0.265m
                },
                EmissionsFactor = new EmissionsFactor
                {
                    High = 0.812m,
                    Medium = 0.562m,
                    Low = 0.312m
                }
            }
        };

        [Fact]
        public async void TestLiteralEqualsLoadFromFile()
        {
            var expected = REFERENCE_DATA_LITERAL;
            var actual = await GetReferenceDataAsync();
            Assert.True(expected.Equals(actual));
        }
        [Fact]
        public async void TestReferenceDataProviderEqualsLiteral()
        {
            var expected = REFERENCE_DATA_LITERAL;

            var mockAppSettings = Substitute.For<IOptions<AppSettings>>();
            mockAppSettings.Value.Returns(new AppSettings { ReferenceDataFile = "Any" });
            var mockLogger = Substitute.For<ILogger<IComponent>>();
            var mockParser = Substitute.For<IParser<ReferenceData>>();
            var referenceData = await GetReferenceDataAsync();
            mockParser.TryParseAsync(Arg.Any<string>()).Returns(referenceData);
            var actual = new ReferenceDataProvider(mockAppSettings, mockLogger, mockParser);
            Assert.True(expected.Equals(actual.ReferenceData));
        }
    }
}
