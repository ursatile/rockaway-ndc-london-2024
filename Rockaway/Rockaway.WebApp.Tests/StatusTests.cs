using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Rockaway.WebApp.Services;
using Shouldly;

namespace Rockaway.WebApp.Tests;

public class StatusTests {

	private static readonly ServerStatus testStatus = new() {
		Assembly = "TEST_ASSEMBLY",
		Modified = new DateTimeOffset(2021, 2, 3, 4, 5, 6, TimeSpan.Zero).ToString("O"),
		Hostname = "TEST_HOST",
		DateTime = new DateTimeOffset(2022, 3, 4, 5, 6, 7, TimeSpan.Zero).ToString("O")
	};

	private class TestStatusReporter : IStatusReporter {
		public ServerStatus GetStatus() => testStatus;
	}

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
		await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.ConfigureServices(services => {
			services.AddSingleton<IStatusReporter>(new TestStatusReporter());
		}));
		using var client = factory.CreateClient();
		var json = await client.GetStringAsync("/status");
		var status = JsonSerializer.Deserialize<ServerStatus>(json, jsonSerializerOptions);
		status.ShouldNotBeNull();
		status.ShouldBeEquivalentTo(testStatus);
	}
}