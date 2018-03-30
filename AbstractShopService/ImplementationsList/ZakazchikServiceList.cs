using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class ZakazchikServiceList : IZakazchikService
    {
        private DataListSingleton source;

        public ZakazchikServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ZakazchikViewModel> GetList()
        {
            List<ZakazchikViewModel> result = new List<ZakazchikViewModel>();
            for (int i = 0; i < source.Zakazchiks.Count; ++i)
            {
                result.Add(new ZakazchikViewModel
                {
                    Id = source.Zakazchiks[i].Id,
                    ZakazchikFIO = source.Zakazchiks[i].ZakazchikFIO
                });
            }
            return result;
        }

        public ZakazchikViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Zakazchiks.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    return new ZakazchikViewModel
                    {
                        Id = source.Zakazchiks[i].Id,
                        ZakazchikFIO = source.Zakazchiks[i].ZakazchikFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ZakazchikBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Zakazchiks.Count; ++i)
            {
                if (source.Zakazchiks[i].Id > maxId)
                {
                    maxId = source.Zakazchiks[i].Id;
                }
                if (source.Zakazchiks[i].ZakazchikFIO == model.ZakazchikFIO)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Zakazchiks.Add(new Zakazchik
            {
                Id = maxId + 1,
                ZakazchikFIO = model.ZakazchikFIO
            });
        }

        public void UpdElement(ZakazchikBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazchiks.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Zakazchiks[i].ZakazchikFIO == model.ZakazchikFIO &&
                    source.Zakazchiks[i].Id != model.Id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazchiks[index].ZakazchikFIO = model.ZakazchikFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Zakazchiks.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    source.Zakazchiks.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
