using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface IDvigateliService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<DvigateliViewModel> GetList();
        [CustomMethod("Метод получения изделия по id")]
        DvigateliViewModel GetElement(int id);
        [CustomMethod("Метод добавления изделия")]
        void AddElement(DvigateliBindingModel model);
        [CustomMethod("Метод изменения данных по изделию")]
        void UpdElement(DvigateliBindingModel model);
        [CustomMethod("Метод удаления изделия")]
        void DelElement(int id);
    }
}
