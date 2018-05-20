using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с письмами")]
    public interface IMessageInfoService
    {
        [CustomMethod("Метод получения списка писем")]
        List<MessageInfoViewModel> GetList();
        [CustomMethod("Метод получения письма по id")]
        MessageInfoViewModel GetElement(int id);
        [CustomMethod("Метод добавления письма")]
        void AddElement(MessageInfoBindingModel model);
    }
}
