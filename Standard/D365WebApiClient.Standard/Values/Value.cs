using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Values
{
    public class Value : JObject
    {
        public Value()
        {
        }

        public Value(JObject other) : base(other)
        {
        }

        public Value(params object[] content) : base(content)
        {
        }

        public Value(object content) : base(content)
        {
        }

        public static Value Read(string json)
        {
            var jobject = JObject.Parse(json);
            var value = new Value();
            value.Merge(jobject);
            return value;
        }
    }
}
