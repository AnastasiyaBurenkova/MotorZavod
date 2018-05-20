using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class DvigateliServiceList : IDvigateliService
    {
        private DataListSingleton source;

        public DvigateliServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<DvigateliViewModel> GetList()
        {
            List<DvigateliViewModel> result = source.Dvigatelis
                .Select(rec => new DvigateliViewModel
                {
                    Id = rec.Id,
                    DvigateliName = rec.DvigateliName,
                    Price = rec.Price,
                    DvigateliDetalis = source.DvigateliDetalis
                            .Where(recPC => recPC.DvigateliId == rec.Id)
                            .Select(recPC => new DvigateliDetaliViewModel
                            {
                                Id = recPC.Id,
                                DvigateliId = recPC.DvigateliId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = source.Detalis
                                    .FirstOrDefault(recC => recC.Id == recPC.DetaliId)?.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public DvigateliViewModel GetElement(int id)
        {
            Dvigateli element = source.Dvigatelis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new DvigateliViewModel
                {
                    Id = element.Id,
                    DvigateliName = element.DvigateliName,
                    Price = element.Price,
                    DvigateliDetalis = source.DvigateliDetalis
                            .Where(recPC => recPC.DvigateliId == element.Id)
                            .Select(recPC => new DvigateliDetaliViewModel
                            {
                                Id = recPC.Id,
                                DvigateliId = recPC.DvigateliId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = source.Detalis
                                        .FirstOrDefault(recC => recC.Id == recPC.DetaliId)?.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DvigateliBindingModel model)
        {
            Dvigateli element = source.Dvigatelis.FirstOrDefault(rec => rec.DvigateliName == model.DvigateliName);
            if (element != null)
            {
                throw new Exception("Уже есть двигатель с таким названием");
            }
            int maxId = source.Dvigatelis.Count > 0 ? source.Dvigatelis.Max(rec => rec.Id) : 0;
            source.Dvigatelis.Add(new Dvigateli
            {
                Id = maxId + 1,
                DvigateliName = model.DvigateliName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.DvigateliDetalis.Count > 0 ?
                                    source.DvigateliDetalis.Max(rec => rec.Id) : 0;
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
                source.DvigateliDetalis.Add(new DvigateliDetali
                {
                    Id = ++maxPCId,
                    DvigateliId = maxId + 1,
                    DetaliId = groupDetali.DetaliId,
                    Count = groupDetali.Count
                });
            }
        }

        public void UpdElement(DvigateliBindingModel model)
        {
            Dvigateli element = source.Dvigatelis.FirstOrDefault(rec =>
                                        rec.DvigateliName == model.DvigateliName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть двигатель с таким названием");
            }
            element = source.Dvigatelis.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.DvigateliName = model.DvigateliName;
            element.Price = model.Price;

            int maxPCId = source.DvigateliDetalis.Count > 0 ? source.DvigateliDetalis.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.DvigateliDetalis.Select(rec => rec.DetaliId).Distinct();
            var updateDetalis = source.DvigateliDetalis
                                            .Where(rec => rec.DvigateliId == model.Id &&
                                           compIds.Contains(rec.DetaliId));
            foreach (var updateDetali in updateDetalis)
            {
                updateDetali.Count = model.DvigateliDetalis
                                                .FirstOrDefault(rec => rec.Id == updateDetali.Id).Count;
            }
            source.DvigateliDetalis.RemoveAll(rec => rec.DvigateliId == model.Id &&
                                       !compIds.Contains(rec.DetaliId));
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
                DvigateliDetali elementPC = source.DvigateliDetalis
                                        .FirstOrDefault(rec => rec.DvigateliId == model.Id &&
                                                        rec.DetaliId == groupDetali.DetaliId);
                if (elementPC != null)
                {
                    elementPC.Count += groupDetali.Count;
                }
                else
                {
                    source.DvigateliDetalis.Add(new DvigateliDetali
                    {
                        Id = ++maxPCId,
                        DvigateliId = model.Id,
                        DetaliId = groupDetali.DetaliId,
                        Count = groupDetali.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Dvigateli element = source.Dvigatelis.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.DvigateliDetalis.RemoveAll(rec => rec.DvigateliId == id);
                source.Dvigatelis.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}