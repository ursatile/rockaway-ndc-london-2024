using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using AngleSharp;

namespace Rockaway.WebApp.Tests;

public class SecurityTests {
	[Fact]
	public async Task Admin_Returns_Redirect_When_Not_Signed_In() {
		await using var factory = new WebApplicationFactory<Program>();
		var doNotFollowRedirects = new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false };
		using var client = factory.CreateClient(doNotFollowRedirects);
		using var response = await client.GetAsync("/admin");
		response.StatusCode.ShouldBe(HttpStatusCode.Found);
		response.Headers.Location?.ToString().ShouldContain("/identity/account/login");
	}

	[Fact]
	public async Task Admin_Has_Personalised_Nav() {
		var fakeUsername = $"{Guid.NewGuid()}@rockaway.dev";
		var browsingContext = BrowsingContext.New(Configuration.Default);
		await using var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.AddFakeAuthentication(fakeUsername));
		var client = factory.CreateClient();
		var html = await client.GetStringAsync("/admin");
		var dom = await browsingContext.OpenAsync(req => req.Content(html));
		var title = dom.QuerySelector("a#manage");
		title.ShouldNotBeNull();
		title.InnerHtml.ShouldBe($"Hello {fakeUsername}!");
	}


}

class FakeAuthenticationFilter(string emailAddress) : IStartupFilter {
	public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
		builder => {
			var options = FakeAuthenticationOptions.Create(emailAddress);
			builder.UseMiddleware<FakeAuthenticationMiddleware>(options);
			next(builder);
		};

	private class FakeAuthenticationOptions(string emailAddress) {

		public string EmailAddress { get; } = emailAddress;

		internal static IOptions<FakeAuthenticationOptions> Create(string emailAddress)
			=> Options.Create(new FakeAuthenticationOptions(emailAddress));
	}

	private class FakeAuthenticationMiddleware(RequestDelegate next, IOptions<FakeAuthenticationOptions> options) {
		private readonly string authenticationType = IdentityConstants.ApplicationScheme;
		private readonly string emailAddress = options.Value.EmailAddress;
		public async Task InvokeAsync(HttpContext context) {
			var claims = new Claim[] {
				new(ClaimTypes.Name, emailAddress),
				new(ClaimTypes.Email, emailAddress)
			};
			var identity = new ClaimsIdentity(claims, authenticationType);
			context.User = new(identity);
			await next(context);
		}
	}

}

public static class FakeAuthenticationMiddlewareExtensions {
	public static IWebHostBuilder AddFakeAuthentication(this IWebHostBuilder builder, string email) {
		builder.ConfigureServices(services => {
			services.AddTransient<IStartupFilter>(_ => new FakeAuthenticationFilter(email));
		});
		return builder;
	}
}

