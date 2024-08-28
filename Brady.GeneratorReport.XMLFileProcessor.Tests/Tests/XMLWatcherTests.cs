using Brady.GeneratorReport.XMLFileProcessor.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Brady.GeneratorReport.XMLFileProcessor.Tests
{
    public class XMLWatcherTests : TestBase
    {
        [Fact]
        public async void Test1()
        {
            var mockLogger = Substitute.For<ILogger<IComponent>>();
            var mockAppSettings = Substitute.For<IOptions<AppSettings>>();
            mockAppSettings.Value.Returns(new AppSettings { ReferenceDataFile = "Any" });
            var watcher = new XMLWatcher(mockAppSettings, mockLogger);
            var notificationFilename = string.Empty;
            watcher.NewXMLFile += (o, e) => { notificationFilename = e.Filename; };
            watcher.Watch(TESTING_OUTPUT_FOLDER);

            var testFileName = $"{TESTING_OUTPUT_FOLDER}test.xml";
            File.Delete(testFileName);
            await File.WriteAllTextAsync(testFileName, "");

            for(var tries = 0; tries < 10; tries++)
            { 
                if (notificationFilename.Length > 0) break;
                await Task.Delay(TimeSpan.FromMilliseconds(100)); //todo hacky, find a better way
            }

            Assert.True(notificationFilename == "test.xml");

            //todo add negative tests, check no duplicates...
        }
    }
}
