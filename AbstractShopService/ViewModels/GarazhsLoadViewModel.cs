using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class GarazhsLoadViewModel
    {
        [DataMember]
        public string GarazhName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<GarazhsDetaliLoadViewModel> Components { get; set; }
    }

    [DataContract]
    public class GarazhsDetaliLoadViewModel
    {
        [DataMember]
        public string DetaliName { get; set; }

        [DataMember]
        public int Count { get; set; }
}
}
