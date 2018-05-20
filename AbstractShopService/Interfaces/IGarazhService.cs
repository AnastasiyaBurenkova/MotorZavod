using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы со складами")]
    public interface IGarazhService
    {
        [CustomMethod("Метод получения списка складов")]
        List<GarazhViewModel> GetList();
        [CustomMethod("Метод получения склада по id")]
        GarazhViewModel GetElement(int id);
        [CustomMethod("Метод добавления склада")]
        void AddElement(GarazhBindingModel model);
        [CustomMethod("Метод изменения данных по складу")]
        void UpdElement(GarazhBindingModel model);
        [CustomMethod("Метод удаления склада")]
        void DelElement(int id);
    }
}
