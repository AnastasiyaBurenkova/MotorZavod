using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IRabochiService
    {
        List<RabochiViewModel> GetList();

        RabochiViewModel GetElement(int id);

        void AddElement(RabochiBindingModel model);

        void UpdElement(RabochiBindingModel model);

        void DelElement(int id);
    }
}
