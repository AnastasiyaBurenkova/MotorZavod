using System;
using System.Collections.Generic;

namespace AbstractShopService.ViewModels
{
    public class GarazhsLoadViewModel
    {
        public string GarazhName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Components { get; set; }
    }
}
