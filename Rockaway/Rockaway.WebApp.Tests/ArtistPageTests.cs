using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Rockaway.WebApp.Data;
using Shouldly;

namespace Rockaway.WebApp.Tests;

public class ArtistPageTests {

	public static IEnumerable<object[]> Artists =>
		new WebApplicationFactory<Program>().Services.CreateScope()
			.ServiceProvider.GetService<RockawayDbContext>()!
			.Artists.Select(a => new[] { a.Name });

	[Theory]
	[MemberData(nameof(Artists))]
	public async Task Artist_Page_Contains_All_Artists(string expectedName) {
		await using var factory = new WebApplicationFactory<Program>();
		var client = factory.CreateClient();
		var html = await client.GetStringAsync("/artists");
		html = WebUtility.HtmlDecode(html);
		html.ShouldContain(expectedName);
	}
}
