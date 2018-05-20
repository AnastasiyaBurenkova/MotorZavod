using System.Runtime.Serialization;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class DetaliBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DetaliName { get; set; }
    }
}
