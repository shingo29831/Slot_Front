using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model;
public class Deserialize_Data
{
	[JsonPropertyName("result")]
	public string? Result { get; set; }

	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[JsonPropertyName("username")]
	public string? Username { get; set; }

	[JsonPropertyName("password")]
	public string? Password { get; set; }

	[JsonPropertyName("token")]
	public string? Token { get; set; }

	[JsonPropertyName("table")]
	public string? Table { get; set; }

	[JsonPropertyName("money")]
	public int? Money { get; set; }

	[JsonPropertyName("key")]
	public string? Key { get; set; }

	[JsonPropertyName("table_id")]
	public string? Table_id { get; set; }

}