using System;

namespace AbstractShopModel
{
    public class MessageInfo
    {
        public int Id { get; set; }

        public string MessageId { get; set; }

        public string FromMailAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime DateDelivery { get; set; }

        public int? ZakazchikId { get; set; }

        public virtual Zakazchik Zakazchik { get; set; }
    }
}
