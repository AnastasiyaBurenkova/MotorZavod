using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IDvigateliService
    {
        List<DvigateliViewModel> GetList();

        DvigateliViewModel GetElement(int id);

        void AddElement(DvigateliBindingModel model);

        void UpdElement(DvigateliBindingModel model);

        void DelElement(int id);
    }
}
