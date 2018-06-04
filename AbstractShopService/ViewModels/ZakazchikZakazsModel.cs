using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class ZakazchikZakazsModel
    {
        [DataMember]
        public string ZakazchikName { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DvigateliName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
