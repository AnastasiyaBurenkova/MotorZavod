using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractShopModel
{
    /// <summary>
    /// Изделие, изготавливаемое в магазине
    /// </summary>
    public class Dvigateli
    {
        public int Id { get; set; }

        [Required]
        public string DvigateliName { get; set; }

        [Required]
        public decimal Price { get; set; }
        [ForeignKey("DvigateliId")]
        public virtual List<Zakaz> Zakazs { get; set; }

        [ForeignKey("DvigateliId")]
        public virtual List<DvigateliDetali> DvigateliDetalis { get; set; }
    }
}