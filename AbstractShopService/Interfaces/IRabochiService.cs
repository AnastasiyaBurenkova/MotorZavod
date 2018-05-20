using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с работниками")]
    public interface IRabochiService
    {
        [CustomMethod("Метод получения списка работников")]
        List<RabochiViewModel> GetList();
        [CustomMethod("Метод получения работника по id")]
        RabochiViewModel GetElement(int id);
        [CustomMethod("Метод добавления работника")]
        void AddElement(RabochiBindingModel model);
        [CustomMethod("Метод изменения данных по работнику")]
        void UpdElement(RabochiBindingModel model);
        [CustomMethod("Метод удаления работника")]
        void DelElement(int id);
    }
}
