using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class DetaliServiceBD : IDetaliService
    {
        private AbstractDbContext context;

        public DetaliServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<DetaliViewModel> GetList()
        {
            List<DetaliViewModel> result = context.Detalis
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
            Detali element = context.Detalis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new DetaliViewModel
                {
                    Id = element.Id,
                    DetaliName = element.DetaliName
                };
            }
            throw new Exception("Деталь не найдена");
        }

        public void AddElement(DetaliBindingModel model)
        {
            Detali element = context.Detalis.FirstOrDefault(rec => rec.DetaliName == model.DetaliName);
            if (element != null)
            {
                throw new Exception("Уже есть детель с таким названием");
            }
            context.Detalis.Add(new Detali
            {
                DetaliName = model.DetaliName
            });
            context.SaveChanges();
        }

        public void UpdElement(DetaliBindingModel model)
        {
            Detali element = context.Detalis.FirstOrDefault(rec =>
                                        rec.DetaliName == model.DetaliName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть деталь с таким названием");
            }
            element = context.Detalis.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Деталь не найдена");
            }
            element.DetaliName = model.DetaliName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Detali element = context.Detalis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Detalis.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Деталь не найдена");
            }
        }
    }
}
