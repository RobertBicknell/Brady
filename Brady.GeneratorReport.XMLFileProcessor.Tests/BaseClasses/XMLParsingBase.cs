using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Brady.GeneratorReport.XMLFileProcessor.Services;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Config;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Output;
using Brady.GeneratorReport.XMLFileProcessor.DTOs.Input;
using Brady.GeneratorReport.XMLFileProcessor.Exceptions;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public abstract class XMLParsingBase : TestBase
    {
        private readonly XMLParser<ReferenceData> _referenceDataParser = new XMLParser<ReferenceData>();
        private readonly XMLParser<GenerationReport> _generationReportParser = new XMLParser<GenerationReport>();
        private readonly XMLParser<GenerationOutput> _generatorOutputParser = new XMLParser<GenerationOutput>();

        public async Task<ReferenceData> GetReferenceDataAsync() 
        { 
            return await _referenceDataParser.TryParseAsync(REFERENCE_DATA_FILE); 
        }
        public async Task<GenerationReport> GetGenerationReportAsync(string path)
        {
            return await _generationReportParser.TryParseAsync(path);
        }
        public async Task<GenerationOutput> GetGenerationOutputAsync(string path)
        {
            return await _generatorOutputParser.TryParseAsync(path);
        }

    }
}
