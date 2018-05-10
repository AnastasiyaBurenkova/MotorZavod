using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace AbstractShopModel
{
    /// <summary>
    /// Клиент магазина
    /// </summary>
    public class Zakazchik
    {
        public int Id { get; set; }

        [Required]
        public string ZakazchikFIO { get; set; }
        [ForeignKey("ZakazchikId")]
       public virtual List<Zakaz> Zakazs { get; set; }
    }
}
