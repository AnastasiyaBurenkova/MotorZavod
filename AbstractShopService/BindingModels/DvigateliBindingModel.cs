using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.BindingModels
{
    public class DvigateliBindingModel
    {
        public int Id { get; set; }

        public string DvigateliName { get; set; }

        public decimal Price { get; set; }

        public List<DvigateliDetaliBindingModel> DvigateliDetalis { get; set; }
    }
}
