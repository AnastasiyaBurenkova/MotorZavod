using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class GarazhDetaliViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int GarazhId { get; set; }
        [DataMember]
        public int DetaliId { get; set; }
        [DataMember]
        public string DetaliName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
