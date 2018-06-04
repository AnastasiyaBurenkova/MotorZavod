using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class GarazhViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string GarazhName { get; set; }
        [DataMember]
        public List<GarazhDetaliViewModel> GarazhDetalis { get; set; }
    }
}
