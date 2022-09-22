using System;
using System.Collections.Generic;

namespace api.meespostma.nl.Data
{
    public partial class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string UrlPlaceholder { get; set; } = null!;
        public byte[]? Logo { get; set; }
    }
}
