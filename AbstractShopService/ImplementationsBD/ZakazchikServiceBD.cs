using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class ZakazchikServiceBD : IZakazchikService
    {
        private AbstractDbContext context;

        public ZakazchikServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ZakazchikViewModel> GetList()
        {
            List<ZakazchikViewModel> result = context.Zakazchiks
                .Select(rec => new ZakazchikViewModel
                {
                    Id = rec.Id,
                    ZakazchikFIO = rec.ZakazchikFIO,
                    Mail = rec.Mail
                })
                .ToList();
            return result;
        }

        public ZakazchikViewModel GetElement(int id)
        {
            Zakazchik element = context.Zakazchiks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ZakazchikViewModel
                {
                    Id = element.Id,
                    ZakazchikFIO = element.ZakazchikFIO,
                    Mail = element.Mail,
                    Messages = context.MessageInfos
                            .Where(recM => recM.ZakazchikId == element.Id)
                            .Select(recM => new MessageInfoViewModel
                            {
                                MessageId = recM.MessageId,
                                DateDelivery = recM.DateDelivery,
                                Subject = recM.Subject,
                                Body = recM.Body
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ZakazchikBindingModel model)
        {
            Zakazchik element = context.Zakazchiks.FirstOrDefault(rec => rec.ZakazchikFIO == model.ZakazchikFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Zakazchiks.Add(new Zakazchik
            {
                ZakazchikFIO = model.ZakazchikFIO,
                Mail = model.Mail
            });
            context.SaveChanges();
        }

        public void UpdElement(ZakazchikBindingModel model)
        {
            Zakazchik element = context.Zakazchiks.FirstOrDefault(rec =>
                                    rec.ZakazchikFIO == model.ZakazchikFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Zakazchiks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ZakazchikFIO = model.ZakazchikFIO;
            element.Mail = model.Mail;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Zakazchik element = context.Zakazchiks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Zakazchiks.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}