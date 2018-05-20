using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class GarazhViewModel
    {
        public int Id { get; set; }

        public string GarazhName { get; set; }

        public List<GarazhDetaliViewModel> GarazhDetalis { get; set; }
    }
}
