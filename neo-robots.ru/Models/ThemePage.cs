using SMI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Models
{
    public class ThemePage
    {
        public int Id { get; set; }
        public News News { get; set; }
        public List<News> NewsSection1News { get; set; }
    }
}