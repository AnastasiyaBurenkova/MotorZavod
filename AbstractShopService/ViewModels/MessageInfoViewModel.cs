using System;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class MessageInfoViewModel
    {
        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        public string ZakazchikName { get; set; }

        [DataMember]
        public DateTime DateDelivery { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }
    }
}