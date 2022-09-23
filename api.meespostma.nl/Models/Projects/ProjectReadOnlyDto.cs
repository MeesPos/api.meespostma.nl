namespace api.meespostma.nl.Models.Projects
{
    public class ProjectReadOnlyDto : BaseDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string UrlPlaceholder { get; set; }

        public string Logo { get; set; }
    }
}
