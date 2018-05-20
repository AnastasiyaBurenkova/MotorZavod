using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class DetaliServiceList : IDetaliService
    {
        private DataListSingleton source;

        public DetaliServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<DetaliViewModel> GetList()
        {
            List<DetaliViewModel> result = source.Detalis
                .Select(rec => new DetaliViewModel
                {
                    Id = rec.Id,
                    DetaliName = rec.DetaliName
                })
                .ToList();
            return result;
        }

        public DetaliViewModel GetElement(int id)
        {
            Detali element = source.Detalis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new DetaliViewModel
                {
                    Id = element.Id,
                    DetaliName = element.DetaliName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DetaliBindingModel model)
        {
            Detali element = source.Detalis.FirstOrDefault(rec => rec.DetaliName == model.DetaliName);
            if (element != null)
            {
                throw new Exception("Уже есть деталь с таким названием");
            }
            int maxId = source.Detalis.Count > 0 ? source.Detalis.Max(rec => rec.Id) : 0;
            source.Detalis.Add(new Detali
            {
                Id = maxId + 1,
                DetaliName = model.DetaliName
            });
        }

        public void UpdElement(DetaliBindingModel model)
        {
            Detali element = source.Detalis.FirstOrDefault(rec =>
                                        rec.DetaliName == model.DetaliName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть деталь с таким названием");
            }
            element = source.Detalis.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.DetaliName = model.DetaliName;
        }

        public void DelElement(int id)
        {
            Detali element = source.Detalis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Detalis.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}