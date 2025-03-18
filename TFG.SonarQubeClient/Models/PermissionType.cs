using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PermissionType
	{
		// Global Permissions
		[JsonPropertyName("admin")]
		Admin,

		[JsonPropertyName("gateadmin")]
		GateAdmin,

		[JsonPropertyName("profileadmin")]
		ProfileAdmin,

		[JsonPropertyName("provisioning")]
		Provisioning,

		[JsonPropertyName("scan")]
		Scan,

		[JsonPropertyName("applicationcreator")]
		ApplicationCreator,

		[JsonPropertyName("portfoliocreator")]
		PortfolioCreator,

		// Project Permissions
		[JsonPropertyName("codeviewer")]
		CodeViewer,

		[JsonPropertyName("issueadmin")]
		IssueAdmin,

		[JsonPropertyName("securityhotspotadmin")]
		SecurityHotspotAdmin,

		[JsonPropertyName("user")]
		User
	}
}
