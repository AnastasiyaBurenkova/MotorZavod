using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IZakazchikService
    {
        List<ZakazchikViewModel> GetList();

        ZakazchikViewModel GetElement(int id);

        void AddElement(ZakazchikBindingModel model);

        void UpdElement(ZakazchikBindingModel model);

        void DelElement(int id);
    }
}
