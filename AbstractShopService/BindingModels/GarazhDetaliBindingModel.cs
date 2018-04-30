using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class GarazhDetaliBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int GarazhId { get; set; }
        [DataMember]
        public int DetaliId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
