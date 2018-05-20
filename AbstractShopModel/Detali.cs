using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbstractShopModel
{
    /// <summary>
    /// Компонент, требуемый для изготовления изделия
    /// </summary>
    public class Detali
    {
        public int Id { get; set; }

        [Required]
        public string DetaliName { get; set; }
        [ForeignKey("DetaliId")]
       public virtual List<DvigateliDetali> DvigateliDetalis { get; set; }

       [ForeignKey("DetaliId")]
        public virtual List<GarazhDetali> GarazhDetalis { get; set; }
    }
}
