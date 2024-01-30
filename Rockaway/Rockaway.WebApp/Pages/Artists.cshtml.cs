using Rockaway.WebApp.Data;
using Rockaway.WebApp.Models;

namespace Rockaway.WebApp.Pages;

public class ArtistsModel(RockawayDbContext db, ILogger<IndexModel> logger)
	: PageModel {
	public IEnumerable<ArtistViewData> Artists { get; set; } = default!;

	public void OnGet() {
		Artists = db.Artists.Select(a => new ArtistViewData(a));
	}
}