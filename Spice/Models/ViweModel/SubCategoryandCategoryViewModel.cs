using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViweModel
{
    public class SubCategoryandCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public SubCategory SubCategory { get; set; }

        public string StutasMessage{ get; set; }
    }
}
