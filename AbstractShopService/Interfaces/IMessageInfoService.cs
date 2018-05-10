using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IMessageInfoService
    {
        List<MessageInfoViewModel> GetList();

        MessageInfoViewModel GetElement(int id);

        void AddElement(MessageInfoBindingModel model);
    }
}
