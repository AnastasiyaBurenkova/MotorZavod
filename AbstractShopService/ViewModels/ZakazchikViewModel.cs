using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class ZakazchikViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string ZakazchikFIO { get; set; }
        [DataMember]
        public List<MessageInfoViewModel> Messages { get; set; }
    }
}
