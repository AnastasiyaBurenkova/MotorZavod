using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<ZakazViewModel> GetList();
        [CustomMethod("Метод создания заказа")]
        void CreateZakaz(ZakazBindingModel model);
        [CustomMethod("Метод передачи заказа в работу")]
        void TakeZakazInWork(ZakazBindingModel model);
        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishZakaz(int id);
        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayZakaz(int id);
        [CustomMethod("Метод пополнения компонент на складе")]
        void PutDetaliOnGarazh(GarazhDetaliBindingModel model);
    }
}
