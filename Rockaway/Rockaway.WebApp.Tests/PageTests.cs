using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Rockaway.WebApp.Tests;

public class PageTests {
	[Fact]
	public async Task Index_Page_Returns_Success() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		using var response = await client.GetAsync("/");
		response.EnsureSuccessStatusCode();
	}

	[Theory]
	[InlineData("/")]
	[InlineData("/privacy")]
	[InlineData("/contact")]
	[InlineData("/about")]
	public async Task Page_Returns_Success(string path) {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		using var response = await client.GetAsync(path);
		response.EnsureSuccessStatusCode();
	}

	[Theory]
	[InlineData("/", "Home Page")]
	public async Task Page_Has_Correct_Title(string path, string title) {
		var browsingContext = BrowsingContext.New(Configuration.Default);
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		var html = await client.GetStringAsync(path);
		var dom = await browsingContext.OpenAsync(req => req.Content(html));
		var titleTag = dom.QuerySelector("title");
		titleTag.ShouldNotBeNull();
		titleTag.InnerHtml.ShouldBe(title);
	}
}