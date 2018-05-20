using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с клиентами")]
    public interface IZakazchikService
    {
        [CustomMethod("Метод получения списка клиентов")]
        List<ZakazchikViewModel> GetList();
        [CustomMethod("Метод получения клиента по id")]
        ZakazchikViewModel GetElement(int id);
        [CustomMethod("Метод добавления клиента")]
        void AddElement(ZakazchikBindingModel model);
        [CustomMethod("Метод изменения данных по клиенту")]
        void UpdElement(ZakazchikBindingModel model);
        [CustomMethod("Метод удаления клиента")]
        void DelElement(int id);
    }
}
