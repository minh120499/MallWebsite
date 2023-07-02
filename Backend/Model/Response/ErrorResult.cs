using System.Collections;
using Newtonsoft.Json;

namespace Backend.Model.Response;

public class ErrorResult
{
    [JsonProperty("errors")]
    public List<Dictionary<string, string>>? Errors { get; set; }

    [JsonProperty("error")]
    public string? Messages { get; set; }
}