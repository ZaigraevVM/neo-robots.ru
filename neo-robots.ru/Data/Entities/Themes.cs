using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Theme
    {
        public Theme()
        {
            HashTagsThemes = new HashSet<HashTagsTheme>();
            NewsThemes = new HashSet<NewsTheme>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<HashTagsTheme> HashTagsThemes { get; set; }
        public virtual ICollection<NewsTheme> NewsThemes { get; set; }
    }
}
