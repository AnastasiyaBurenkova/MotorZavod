using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class GarazhServiceBD : IGarazhService
    {
        private AbstractDbContext context;

        public GarazhServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<GarazhViewModel> GetList()
        {
            List<GarazhViewModel> result = context.Garazhs
                .Select(rec => new GarazhViewModel
                {
                    Id = rec.Id,
                    GarazhName = rec.GarazhName,
                    GarazhDetalis = context.GarazhDetalis
                            .Where(recPC => recPC.GarazhId == rec.Id)
                            .Select(recPC => new GarazhDetaliViewModel
                            {
                                Id = recPC.Id,
                                GarazhId = recPC.GarazhId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public GarazhViewModel GetElement(int id)
        {
            Garazh element = context.Garazhs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new GarazhViewModel
                {
                    Id = element.Id,
                    GarazhName = element.GarazhName,
                    GarazhDetalis = context.GarazhDetalis
                            .Where(recPC => recPC.GarazhId == element.Id)
                            .Select(recPC => new GarazhDetaliViewModel
                            {
                                Id = recPC.Id,
                                GarazhId = recPC.GarazhId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(GarazhBindingModel model)
        {
            Garazh element = context.Garazhs.FirstOrDefault(rec => rec.GarazhName == model.GarazhName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Garazhs.Add(new Garazh
            {
                GarazhName = model.GarazhName
            });
            context.SaveChanges();
        }

        public void UpdElement(GarazhBindingModel model)
        {
            Garazh element = context.Garazhs.FirstOrDefault(rec =>
                                        rec.GarazhName == model.GarazhName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Garazhs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.GarazhName = model.GarazhName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Garazh element = context.Garazhs.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context.GarazhDetalis.RemoveRange(
                                            context.GarazhDetalis.Where(rec => rec.GarazhId == id));
                        context.Garazhs.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}