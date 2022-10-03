using System.ComponentModel.DataAnnotations;

namespace api.meespostma.nl.Models.Projects
{
    public class ProjectUpdateDto : BaseDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Description { get; set; }

        [Required, Url]
        public string Url { get; set; }

        [Required, MaxLength(50)]
        public string UrlPlaceholder { get; set; }

        public string? Logo { get; set; }
        public string? OriginalLogoName { get; set; }
    }
}
