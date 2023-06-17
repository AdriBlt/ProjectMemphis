using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ProjectMemphis.Data.Tests
{
    [TestClass]
    public class EventConverterTests
    {

        private class ClassWithEventProperty
        {
            [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
            public Event TheEvent { get; set; }
        }
        [TestMethod]
        public void VerifyThatAnEventPropertyIsDeserialisable()
        {
            // Given
            const string TheTitle = "this is an title";
            const string TheText = "this is a text";
            var myObjectWithEventProterty = new ClassWithEventProperty
            {
                TheEvent = new OptionsEvent
                {
                    Title = TheTitle,
                    Options = new List<Option> { new StateOption { Text = TheText } }
                }
            };

            var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            var myObjectWithEventProtertyAsJson = JsonConvert.SerializeObject(myObjectWithEventProterty, jsonSettings);

            // When
            var result = JsonConvert.DeserializeObject<ClassWithEventProperty>(myObjectWithEventProtertyAsJson, jsonSettings);

            // Then
            Assert.IsNotNull(result.TheEvent);
            Assert.IsInstanceOfType(result.TheEvent, typeof(OptionsEvent));
            var theEvent = (OptionsEvent)result.TheEvent;
            Assert.AreEqual(TheTitle, theEvent.Title);
            Assert.IsNotNull(theEvent.Options);
            Assert.IsInstanceOfType(theEvent.Options, typeof(IEnumerable<Option>));
            var options = ((IEnumerable<Option>)theEvent.Options);
            Assert.AreEqual(1, options.Count());
            Assert.AreEqual(TheText, options.First().Text);
        }
    }
}
