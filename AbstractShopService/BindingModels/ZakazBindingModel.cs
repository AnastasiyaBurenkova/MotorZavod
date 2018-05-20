using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.BindingModels
{
    public class ZakazBindingModel
    {
        public int Id { get; set; }

        public int ZakazchikId { get; set; }

        public int DvigateliId { get; set; }

        public int? RabochiId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
