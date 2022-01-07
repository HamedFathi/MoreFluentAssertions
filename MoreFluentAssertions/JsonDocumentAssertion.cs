using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MoreFluentAssertions
{
    public class JsonDocumentAssertion : ReferenceTypeAssertions<JsonDocument, JsonDocumentAssertion>
    {
        private JsonDocument _actualJDoc;
        public JsonDocumentAssertion(JsonDocument actual) : base(actual)
        {
            _actualJDoc = actual;
        }

        protected override string Identifier => nameof(JsonDocument);

        private (IEnumerable<string>, IEnumerable<string>) RemoveUnknownFromDifferences(IEnumerable<string> actual, IEnumerable<string> expected)
        {
            var actualResult = new List<string>();
            var expectedResult = new List<string>();
            var actualUnknownPaths = actual
                .Where(x => x.EndsWith("-undefined") || x.EndsWith("-null"))
                .Select(x => x.Substring(0, x.LastIndexOfAny(new[] { '-' })));

            var expectedUnknownPaths = expected
                .Where(x => x.EndsWith("-undefined") || x.EndsWith("-null"))
                .Select(x => x.Substring(0, x.LastIndexOfAny(new[] { '-' })));

            var allUnknownKeys = actualUnknownPaths.Concat(expectedUnknownPaths).Distinct();
            foreach (var item in actual)
            {
                var lastDash = item.LastIndexOfAny(new[] { '-' });
                var path = item.Substring(0, lastDash);
                if (allUnknownKeys.Any(x => x == path) && expected.Any(x => x.StartsWith($"{path}-")))
                {
                    actualResult.Add(path);
                }
                else
                {
                    actualResult.Add(item);
                }
            }

            foreach (var item in expected)
            {
                var lastDash = item.LastIndexOfAny(new[] { '-' });
                var path = item.Substring(0, lastDash);
                if (allUnknownKeys.Any(x => x == path) && actual.Any(x => x.StartsWith($"{path}-")))
                {
                    expectedResult.Add(path);
                }
                else
                {
                    expectedResult.Add(item);
                }
            }
            return (actualResult, expectedResult);
        }
        public AndConstraint<JsonDocumentAssertion> HaveSameSchemaAs(JsonDocument expected, string because = "", params object[] becauseArgs)
        {
            var actualKeys = _actualJDoc.RootElement.GetKeys();
            var expectedKeys = expected.RootElement.GetKeys();

            var (actualResult, expectedResult) = RemoveUnknownFromDifferences(actualKeys, expectedKeys);

            var existsInActual = actualResult.Except(expectedResult).ToList();
            var existsInExpected = expectedResult.Except(actualResult).ToList();

            var status = !existsInActual.Any() && !existsInExpected.Any();

            var sb = new StringBuilder();
            if (existsInActual.Any())
            {
                sb.AppendLine("The inputs do not match, the differences are as follows:");
                sb.AppendLine();
                sb.AppendLine("Actual:");

                foreach (var item in existsInActual)
                {
                    var lastDash = item.LastIndexOfAny(new[] { '-' });
                    var len = item.Length;
                    if (!string.IsNullOrEmpty(item))
                    {
                        var path = item.Substring(0, lastDash);
                        var type = item.Substring(lastDash + 1);
                        sb.AppendLine($"Path: {path}, Type:{type}");
                    }
                }
            }
            if (existsInExpected.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Expected:");

                foreach (var item in existsInExpected)
                {
                    var lastDash = item.LastIndexOfAny(new[] { '-' });
                    var len = item.Length;
                    if (!string.IsNullOrEmpty(item))
                    {
                        var path = item.Substring(0, lastDash);
                        var type = item.Substring(lastDash + 1);
                        sb.AppendLine($"Path: {path}, Type:{type}");
                    }
                }
            }

            var message = sb.ToString();

            Execute.Assertion
                .ForCondition(status)
                .BecauseOf(because, becauseArgs)
                .FailWith(message);

            return new AndConstraint<JsonDocumentAssertion>(this);
        }
        public AndConstraint<JsonDocumentAssertion> ContainSchemaOf(JsonDocument expected, bool ignoreAdditionalProps = false, string because = "", params object[] becauseArgs)
        {
            var actualKeys = _actualJDoc.RootElement
                .GetKeys()
                .WhereIf(ignoreAdditionalProps, x => !x.Contains("additionalProp"))
                ;
            var expectedKeys = expected.RootElement
                .GetKeys()
                .WhereIf(ignoreAdditionalProps, x => !x.Contains("additionalProp"))
                ;

            var (actualResult, expectedResult) = RemoveUnknownFromDifferences(actualKeys, expectedKeys);

            actualResult = actualResult.Select(x => Regex.Replace(x, @"\[[0-9]+\]", ".[item]")).Distinct();
            expectedResult = expectedResult.Select(x => Regex.Replace(x, @"\[[0-9]+\]", ".[item]")).Distinct();

            var existsInActual = actualResult.Except(expectedResult).ToList();
            var existsInExpected = expectedResult.Except(actualResult).ToList();

            var status = !existsInActual.Any() && !existsInExpected.Any();

            var sb = new StringBuilder();
            if (existsInActual.Any())
            {
                sb.AppendLine("The inputs do not match, the differences are as follows:");
                sb.AppendLine();
                sb.AppendLine("Actual:");

                foreach (var item in existsInActual)
                {
                    var lastDash = item.LastIndexOfAny(new[] { '-' });
                    var len = item.Length;
                    if (!string.IsNullOrEmpty(item))
                    {
                        var path = item.Substring(0, lastDash);
                        var type = item.Substring(lastDash + 1);
                        sb.AppendLine($"Path: {path}, Type:{type}");
                    }
                }
            }
            if (existsInExpected.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Expected:");

                foreach (var item in existsInExpected)
                {
                    var lastDash = item.LastIndexOfAny(new[] { '-' });
                    var len = item.Length;
                    if (!string.IsNullOrEmpty(item))
                    {
                        var path = item.Substring(0, lastDash);
                        var type = item.Substring(lastDash + 1);
                        sb.AppendLine($"Path: {path}, Type:{type}");
                    }
                }
            }

            var message = sb.ToString();

            Execute.Assertion
                .ForCondition(status)
                .BecauseOf(because, becauseArgs)
                .FailWith(message);

            return new AndConstraint<JsonDocumentAssertion>(this);

        }
    }
}
