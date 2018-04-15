using AbstractShopModel;
using System.Collections.Generic;

namespace AbstractShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Zakazchik> Zakazchiks { get; set; }

        public List<Detali> Detalis { get; set; }

        public List<Rabochi> Rabochis { get; set; }

        public List<Zakaz> Zakazs { get; set; }

        public List<Dvigateli> Dvigatelis { get; set; }

        public List<DvigateliDetali> DvigateliDetalis { get; set; }

        public List<Garazh> Garazhs { get; set; }

        public List<GarazhDetali> GarazhDetalis { get; set; }

        private DataListSingleton()
        {
            Zakazchiks = new List<Zakazchik>();
            Detalis = new List<Detali>();
            Rabochis = new List<Rabochi>();
            Zakazs = new List<Zakaz>();
            Dvigatelis = new List<Dvigateli>();
            DvigateliDetalis = new List<DvigateliDetali>();
            Garazhs = new List<Garazh>();
            GarazhDetalis = new List<GarazhDetali>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}

