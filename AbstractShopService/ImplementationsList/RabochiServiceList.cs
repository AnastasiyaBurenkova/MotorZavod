using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class RabochiServiceList : IRabochiService
    {
        private DataListSingleton source;

        public RabochiServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<RabochiViewModel> GetList()
        {
            List<RabochiViewModel> result = source.Rabochis
                .Select(rec => new RabochiViewModel
                {
                    Id = rec.Id,
                    RabochiFIO = rec.RabochiFIO
                })
                .ToList();
            return result;
        }

        public RabochiViewModel GetElement(int id)
        {
            Rabochi element = source.Rabochis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new RabochiViewModel
                {
                    Id = element.Id,
                    RabochiFIO = element.RabochiFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RabochiBindingModel model)
        {
            Rabochi element = source.Rabochis.FirstOrDefault(rec => rec.RabochiFIO == model.RabochiFIO);
            if (element != null)
            {
                throw new Exception("Уже есть рабочий с таким ФИО");
            }
            int maxId = source.Rabochis.Count > 0 ? source.Rabochis.Max(rec => rec.Id) : 0;
            source.Rabochis.Add(new Rabochi
            {
                Id = maxId + 1,
                RabochiFIO = model.RabochiFIO
            });
        }

        public void UpdElement(RabochiBindingModel model)
        {
            Rabochi element = source.Rabochis.FirstOrDefault(rec =>
                                        rec.RabochiFIO == model.RabochiFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть рабочий с таким ФИО");
            }
            element = source.Rabochis.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.RabochiFIO = model.RabochiFIO;
        }

        public void DelElement(int id)
        {
            Rabochi element = source.Rabochis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Rabochis.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}