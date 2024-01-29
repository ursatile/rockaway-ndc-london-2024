using Microsoft.AspNetCore.Mvc.Testing;
using Rockaway.WebApp.Services;
using System.Text.Json;
using Shouldly;

namespace Rockaway.WebApp.Tests;

public class StatusTests {

	private static readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);

	[Fact]
	public async Task Status_Returns_Success() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		using var response = await client.GetAsync("/status");
		response.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task Status_Returns_Correct_Hostname() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		var json = await client.GetStringAsync("/status");
		var status = JsonSerializer.Deserialize<ServerStatus>(json, jsonSerializerOptions);
		status.ShouldNotBeNull();
		status.Hostname.ShouldBe("DYLAN-FRAMEWORK");
	}
}
