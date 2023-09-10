using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class HashTagsNews
    {
        public int HashTagId { get; set; }
        public int NewsId { get; set; }

        public virtual HashTag HashTag { get; set; }
        public virtual News News { get; set; }
    }
}
