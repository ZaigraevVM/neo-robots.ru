using SMI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Models
{
    public class HomePage
    {
        public List<News> NewsSection1News { get; set; }
        public List<News> NewsSection2News { get; set; }
        public List<News> NewsSection3News { get; set; }
        public List<News> NewsSection4News { get; set; }
        public List<News> NewsSection5News { get; set; }
        public List<News> NewsSection6News { get; set; }
        public List<News> NewsSection7News { get; set; }
        public List<News> NewsSection8News { get; set; }


        public List<News> NewsSection1Slider { get; set; }
        public List<News> NewsSection2Slider { get; set; }
        public List<News> NewsSection3Slider { get; set; }
        public List<News> NewsSection4Slider { get; set; }




        public List<News> NewsSectionRightNews { get; set; }
        public List<News> NewsSectionRightSlider { get; set; }
        public List<News> NewsSectionRightTags { get; set; }



        public News News { get; set; }
    }
}
