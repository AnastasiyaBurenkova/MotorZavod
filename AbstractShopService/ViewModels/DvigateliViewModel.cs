using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class DvigateliViewModel
    {
        public int Id { get; set; }

        public string DvigateliName { get; set; }

        public decimal Price { get; set; }

        public List<DvigateliDetaliViewModel> DvigateliDetalis { get; set; }
    }
}
