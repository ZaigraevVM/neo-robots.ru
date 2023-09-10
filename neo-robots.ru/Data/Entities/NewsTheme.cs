using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class NewsTheme
    {
        public int NewsId { get; set; }
        public int ThemeId { get; set; }

        public virtual News News { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
