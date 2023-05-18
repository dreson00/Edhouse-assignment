using Microsoft.Extensions.Logging;
using Moq;
using RegularForestLibrary;

namespace RegularForestLibraryTests
{
    public class IntegrationTests
    {
        [Fact]
        public void CountVisibleTreesInFile_ShouldReturnCorrectCount()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RegularForest>>();
            var forest = new RegularForest(loggerMock.Object);

            // Act
            var result = forest.CountVisibleTreesInFile("TestData\\ExampleData1.txt");

            // Assert
            loggerMock.Verify(
                mock => mock.Log(LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    null,
                    ((Func<object, Exception, string>)It.IsAny<object>())!),
                Times.Exactly(3));
            Assert.Equal(21, result);
        }

        [Fact]
        public void CountVisibleTreesInFile_ShouldThrowException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RegularForest>>();
            var forest = new RegularForest(loggerMock.Object);

            // Act and Assert
            Assert.Throws<FormatException>(() => forest.CountVisibleTreesInFile("TestData\\ExampleData2.txt"));
        }
    }
}
