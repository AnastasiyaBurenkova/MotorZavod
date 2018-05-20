using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IDetaliService
    {
        List<DetaliViewModel> GetList();

        DetaliViewModel GetElement(int id);

        void AddElement(DetaliBindingModel model);

        void UpdElement(DetaliBindingModel model);

        void DelElement(int id);
    }
}
