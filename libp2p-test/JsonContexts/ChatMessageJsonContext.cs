using System.Text.Json;
using System.Text.Json.Serialization;

namespace libp2p_test;

[JsonSerializable(typeof(ChatMessage))]
internal partial class ChatMessageJsonContext : JsonSerializerContext
{
}

