using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class RabochiServiceBD : IRabochiService
    {
        private AbstractDbContext context;

        public RabochiServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<RabochiViewModel> GetList()
        {
            List<RabochiViewModel> result = context.Rabochis
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
            Rabochi element = context.Rabochis.FirstOrDefault(rec => rec.Id == id);
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
            Rabochi element = context.Rabochis.FirstOrDefault(rec => rec.RabochiFIO == model.RabochiFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Rabochis.Add(new Rabochi
            {
                RabochiFIO = model.RabochiFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(RabochiBindingModel model)
        {
            Rabochi element = context.Rabochis.FirstOrDefault(rec =>
                                        rec.RabochiFIO == model.RabochiFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Rabochis.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.RabochiFIO = model.RabochiFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Rabochi element = context.Rabochis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Rabochis.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
