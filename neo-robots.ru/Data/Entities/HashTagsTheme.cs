using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class HashTagsTheme
    {
        public int HashTagId { get; set; }
        public int ThemeId { get; set; }

        public virtual HashTag HashTag { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
