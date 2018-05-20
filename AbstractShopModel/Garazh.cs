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
    /// Хранилиище компонентов в магазине
    /// </summary>
    public class Garazh
    {
        public int Id { get; set; }

        [Required]
        public string GarazhName { get; set; }
        [ForeignKey("GarazhId")]
        public virtual List<GarazhDetali> GarazhDetalis { get; set; }
    }
}