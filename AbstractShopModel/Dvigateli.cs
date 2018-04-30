using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    /// <summary>
    /// Изделие, изготавливаемое в магазине
    /// </summary>
    public class Dvigateli
    {
        public int Id { get; set; }

        public string DvigateliName { get; set; }

        public decimal Price { get; set; }
    }
}