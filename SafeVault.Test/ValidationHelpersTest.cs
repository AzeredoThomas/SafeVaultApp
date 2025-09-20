using SafeVault.Helpers;
using Xunit;

namespace SafeVault.Tests
{
    public class ValidationHelpersTests
    {
        [Theory]
        [InlineData("<script>alert('XSS')</script>", false)]
        [InlineData("<iframe src='evil.com'></iframe>", false)]
        [InlineData("normalText123", true)]
        [InlineData("Hello <b>world</b>", true)] // HTML som inte är skadlig
        [InlineData("", true)]
        [InlineData(null, true)]
        public void IsValidXssInput_ShouldDetectMaliciousContent(string input, bool expected)
        {
            // Act
            var result = ValidationHelpers.IsValidXSSInput(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
