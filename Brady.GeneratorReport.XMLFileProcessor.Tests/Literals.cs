using Brady.GeneratorReport.XMLFileProcessor.DTOs.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    static class Literals
    {
        public static readonly ReferenceData REFERENCE_DATA_LITERAL = new ReferenceData
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
    }
}
