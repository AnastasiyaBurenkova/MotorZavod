using System.Runtime.Serialization;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class ZakazchikBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string ZakazchikFIO { get; set; }
    }
}
