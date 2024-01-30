using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Rockaway.WebApp.TagHelpers;

public class FancyPanelTagHelper : TagHelper {
	public override void Process(TagHelperContext context, TagHelperOutput output) {
		base.Process(context, output);
		output.TagMode = TagMode.StartTagAndEndTag;
		output.TagName = "div";
		output.Attributes.Add("class", "fancy-panel");
	}
}