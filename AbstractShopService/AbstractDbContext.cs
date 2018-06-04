using AbstractShopModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractShopService
{
    
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext() : base("AbstractDatabase19")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Zakazchik> Zakazchiks { get; set; }

        public virtual DbSet<Detali> Detalis { get; set; }

        public virtual DbSet<Rabochi> Rabochis { get; set; }

        public virtual DbSet<Zakaz> Zakazs { get; set; }

        public virtual DbSet<Dvigateli> Dvigatelis { get; set; }

        public virtual DbSet<DvigateliDetali> DvigateliDetalis { get; set; }

        public virtual DbSet<Garazh> Garazhs { get; set; }

        public virtual DbSet<GarazhDetali> GarazhDetalis { get; set; }
    }
}
