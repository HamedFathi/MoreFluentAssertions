using System.Text.Json;

namespace MoreFluentAssertions
{
    public static class FluentAssertionsExtensions
    {        
        public static JsonDocumentAssertion Should(this JsonDocument jsonDocument) => new JsonDocumentAssertion(jsonDocument);
    }
}
