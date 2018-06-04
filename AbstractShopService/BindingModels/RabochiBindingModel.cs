using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class RabochiBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string RabochiFIO { get; set; }
    }
}
