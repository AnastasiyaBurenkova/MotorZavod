using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class DvigateliServiceBD : IDvigateliService
    {
        private AbstractDbContext context;

        public DvigateliServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<DvigateliViewModel> GetList()
        {
            List<DvigateliViewModel> result = context.Dvigatelis
                .Select(rec => new DvigateliViewModel
                {
                    Id = rec.Id,
                    DvigateliName = rec.DvigateliName,
                    Price = rec.Price,
                    DvigateliDetalis = context.DvigateliDetalis
                            .Where(recPC => recPC.DvigateliId == rec.Id)
                            .Select(recPC => new DvigateliDetaliViewModel
                            {
                                Id = recPC.Id,
                                DvigateliId = recPC.DvigateliId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public DvigateliViewModel GetElement(int id)
        {
            Dvigateli element = context.Dvigatelis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new DvigateliViewModel
                {
                    Id = element.Id,
                    DvigateliName = element.DvigateliName,
                    Price = element.Price,
                    DvigateliDetalis = context.DvigateliDetalis
                            .Where(recPC => recPC.DvigateliId == element.Id)
                            .Select(recPC => new DvigateliDetaliViewModel
                            {
                                Id = recPC.Id,
                                DvigateliId = recPC.DvigateliId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DvigateliBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Dvigateli element = context.Dvigatelis.FirstOrDefault(rec => rec.DvigateliName == model.DvigateliName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Dvigateli
                    {
                        DvigateliName = model.DvigateliName,
                        Price = model.Price
                    };
                    context.Dvigatelis.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupDetalis = model.DvigateliDetalis
                                                .GroupBy(rec => rec.DetaliId)
                                                .Select(rec => new
                                                {
                                                    DetaliId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    // добавляем компоненты
                    foreach (var groupDetali in groupDetalis)
                    {
                        context.DvigateliDetalis.Add(new DvigateliDetali
                        {
                            DvigateliId = element.Id,
                            DetaliId = groupDetali.DetaliId,
                            Count = groupDetali.Count
                        });
                        context.SaveChanges();
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

        public void UpdElement(DvigateliBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Dvigateli element = context.Dvigatelis.FirstOrDefault(rec =>
                                        rec.DvigateliName == model.DvigateliName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Dvigatelis.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.DvigateliName = model.DvigateliName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.DvigateliDetalis.Select(rec => rec.DetaliId).Distinct();
                    var updateDetalis = context.DvigateliDetalis
                                                    .Where(rec => rec.DvigateliId == model.Id &&
                                                        compIds.Contains(rec.DetaliId));
                    foreach (var updateDetali in updateDetalis)
                    {
                        updateDetali.Count = model.DvigateliDetalis
                                                        .FirstOrDefault(rec => rec.Id == updateDetali.Id).Count;
                    }
                    context.SaveChanges();
                    context.DvigateliDetalis.RemoveRange(
                                        context.DvigateliDetalis.Where(rec => rec.DvigateliId == model.Id &&
                                                                            !compIds.Contains(rec.DetaliId)));
                    context.SaveChanges();
                    // новые записи
                    var groupDetalis = model.DvigateliDetalis
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.DetaliId)
                                                .Select(rec => new
                                                {
                                                    DetaliId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupDetali in groupDetalis)
                    {
                        DvigateliDetali elementPC = context.DvigateliDetalis
                                                .FirstOrDefault(rec => rec.DvigateliId == model.Id &&
                                                                rec.DetaliId == groupDetali.DetaliId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupDetali.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.DvigateliDetalis.Add(new DvigateliDetali
                            {
                                DvigateliId = model.Id,
                                DetaliId = groupDetali.DetaliId,
                                Count = groupDetali.Count
                            });
                            context.SaveChanges();
                        }
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

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Dvigateli element = context.Dvigatelis.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.DvigateliDetalis.RemoveRange(
                                            context.DvigateliDetalis.Where(rec => rec.DvigateliId == id));
                        context.Dvigatelis.Remove(element);
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
