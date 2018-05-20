using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IGarazhService
    {
        List<GarazhViewModel> GetList();

        GarazhViewModel GetElement(int id);

        void AddElement(GarazhBindingModel model);

        void UpdElement(GarazhBindingModel model);

        void DelElement(int id);
    }
}
