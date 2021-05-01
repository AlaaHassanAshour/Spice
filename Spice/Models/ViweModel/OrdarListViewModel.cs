using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViweModel
{
    public class OrdarListViewModel
    {
        public List<OrdarDetailsViewModel> ordars{ get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

