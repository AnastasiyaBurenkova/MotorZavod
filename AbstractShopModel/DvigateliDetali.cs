using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    /// <summary>
    /// Сколько компонентов, требуется при изготовлении изделия
    /// </summary>
    public class DvigateliDetali
    {
        public int Id { get; set; }

        public int DvigateliId { get; set; }

        public int DetaliId { get; set; }

        public int Count { get; set; }
        public virtual Dvigateli Dvigateli { get; set; }

       public virtual Detali Detali { get; set; }
    }
}