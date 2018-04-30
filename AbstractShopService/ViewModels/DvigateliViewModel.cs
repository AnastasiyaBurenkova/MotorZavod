using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class DvigateliViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DvigateliName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<DvigateliDetaliViewModel> DvigateliDetalis { get; set; }
    }
}
