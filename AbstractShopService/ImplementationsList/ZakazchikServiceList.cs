using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<ZakazchikViewModel> result = source.Zakazchiks
                .Select(rec => new ZakazchikViewModel
                {
                    Id = rec.Id,
                    ZakazchikFIO = rec.ZakazchikFIO
                })
                .ToList();
            return result;

            List<ZakazchikViewModel> result1 = (from rec in source.Zakazchiks
                                                select new ZakazchikViewModel
                                                {
                                                    Id = rec.Id,
                                                    ZakazchikFIO = rec.ZakazchikFIO
                                                }).ToList();

             return result1;
        }

        public ZakazchikViewModel GetElement(int id)
        {
            Zakazchik element = source.Zakazchiks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ZakazchikViewModel
                {
                    Id = element.Id,
                    ZakazchikFIO = element.ZakazchikFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ZakazchikBindingModel model)
        {
            Zakazchik element = source.Zakazchiks.FirstOrDefault(rec => rec.ZakazchikFIO == model.ZakazchikFIO);
            if (element != null)
            {
                throw new Exception("Уже есть заказчик с таким ФИО");
            }
            int maxId = source.Zakazchiks.Count > 0 ? source.Zakazchiks.Max(rec => rec.Id) : 0;
            source.Zakazchiks.Add(new Zakazchik
            {
                Id = maxId + 1,
                ZakazchikFIO = model.ZakazchikFIO
            });
        }

        public void UpdElement(ZakazchikBindingModel model)
        {
            Zakazchik element = source.Zakazchiks.FirstOrDefault(rec =>
                                    rec.ZakazchikFIO == model.ZakazchikFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть заказчик с таким ФИО");
            }
            element = source.Zakazchiks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ZakazchikFIO = model.ZakazchikFIO;
        }

        public void DelElement(int id)
        {
            Zakazchik element = source.Zakazchiks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Zakazchiks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}