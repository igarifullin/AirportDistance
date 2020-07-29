using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Services.ValidationRules;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.ValidationRules
{
    [TestFixture]
    public class StringIsNotNullRuleHandlerTests
    {
        private StringIsNotNullRuleHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new StringIsNotNullRuleHandler();
        }

        private static IEnumerable<TestCaseData> SuccessCases()
        {
            yield return new TestCaseData("LED");
            yield return new TestCaseData("NotEmptyString");
        }

        private static IEnumerable<TestCaseData> FailCases()
        {
            yield return new TestCaseData((string)null);
            yield return new TestCaseData(string.Empty);
            yield return new TestCaseData("");
        }

        [TestCaseSource(nameof(SuccessCases))]
        public async Task ExecuteAsync_RegexSuccess(string value)
        {
            // arrange
            var rule = new StringIsNotNullRule(value);

            // act
            var result = await _sut.ExecuteAsync(rule);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [TestCaseSource(nameof(FailCases))]
        public async Task ExecuteAsync_RegexFails(string value)
        {
            // arrange
            var rule = new StringIsNotNullRule(value);

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