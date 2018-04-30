using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class GarazhBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string GarazhName { get; set; }
    }
}
