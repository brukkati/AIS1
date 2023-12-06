using System.Collections.Generic;
using System.Text.Json;

namespace ServerClientLinkingPart
{
    public class Connection
    {
        public string Act { get; set; }
        public List<string> Content { get; set; }
        [System.Text.Json.Serialization.JsonConstructor]
        public Connection(string act, List<string> content)
        {
            Act = act;
            Content = content;
        }

        public Connection(string act, string content) : this(act, new List<string> { content }) { }

        public string GetJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public static Connection GetRequest(string json)
        {
            return JsonSerializer.Deserialize<Connection>(json);
        }
    }
}