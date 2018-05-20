using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с компонентами")]
    
    public interface IDetaliService
    {
        [CustomMethod("Метод получения списка компонент")]
        List<DetaliViewModel> GetList();
        [CustomMethod("Метод получения компонента по id")]
        DetaliViewModel GetElement(int id);
        [CustomMethod("Метод добавления компонента")]
        void AddElement(DetaliBindingModel model);
        [CustomMethod("Метод изменения данных по компоненту")]
        void UpdElement(DetaliBindingModel model);
        [CustomMethod("Метод удаления компонента")]
        void DelElement(int id);
    }
}
