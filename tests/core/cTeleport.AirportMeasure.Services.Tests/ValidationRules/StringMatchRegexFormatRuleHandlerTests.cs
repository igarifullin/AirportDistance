using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Services.ValidationRules;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.ValidationRules
{
    [TestFixture]
    public class StringMatchRegexFormatRuleHandlerTests
    {
        private StringMatchRegexFormatRuleHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new StringMatchRegexFormatRuleHandler();
        }

        private static IEnumerable<TestCaseData> SuccessCases()
        {
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "LED");
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "AMS");
            
            yield return new TestCaseData(new Regex("^[0-9]{1,2}$"), "11");
            yield return new TestCaseData(new Regex("^[0-9]{1,2}$"), "0");
        }

        private static IEnumerable<TestCaseData> FailCases()
        {
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "Saint-Petersburg");
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "_");
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "AMSTERDAM");
            yield return new TestCaseData(new Regex("^[a-zA-Z]{3}$"), "123");
            
            yield return new TestCaseData(new Regex("^[0-9]{1,2}$"), "000");
            yield return new TestCaseData(new Regex("^[0-9]{1,2}$"), "RED");
        }

        [TestCaseSource(nameof(SuccessCases))]
        public async Task ExecuteAsync_RegexSuccess(Regex regex, string value)
        {
            // arrange
            var rule = new StringMatchRegexFormatRule(value, regex);

            // act
            var result = await _sut.ExecuteAsync(rule);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [TestCaseSource(nameof(FailCases))]
        public async Task ExecuteAsync_RegexFails(Regex regex, string value)
        {
            // arrange
            var rule = new StringMatchRegexFormatRule(value, regex);

            // act
            var result = await _sut.ExecuteAsync(rule);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.First().Code, Is.EqualTo((int)SystemErrorCodes.InvalidRequest));
        }
    }
}