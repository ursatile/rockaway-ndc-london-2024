using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Data.Entities;

namespace Rockaway.WebApp.Pages;

public class ArtistsModel : PageModel {
	public List<Artist> artists { get; } = new List<Artist> {};
	private readonly ILogger<ArtistsModel> _logger;

	public ArtistsModel(RockawayDbContext db, ILogger<ArtistsModel> logger) {
		_logger = logger;
		artists = db.Artists.ToList();
	}

	public void OnGet() {
	}
}