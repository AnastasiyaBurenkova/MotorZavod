using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class GarazhServiceList : IGarazhService
    {
        private DataListSingleton source;

        public GarazhServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<GarazhViewModel> GetList()
        {
            List<GarazhViewModel> result = source.Garazhs
                .Select(rec => new GarazhViewModel
                {
                    Id = rec.Id,
                    GarazhName = rec.GarazhName,
                    GarazhDetalis = source.GarazhDetalis
                            .Where(recPC => recPC.GarazhId == rec.Id)
                            .Select(recPC => new GarazhDetaliViewModel
                            {
                                Id = recPC.Id,
                                GarazhId = recPC.GarazhId,
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

        public GarazhViewModel GetElement(int id)
        {
            Garazh element = source.Garazhs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new GarazhViewModel
                {
                    Id = element.Id,
                    GarazhName = element.GarazhName,
                    GarazhDetalis = source.GarazhDetalis
                            .Where(recPC => recPC.GarazhId == element.Id)
                            .Select(recPC => new GarazhDetaliViewModel
                            {
                                Id = recPC.Id,
                                GarazhId = recPC.GarazhId,
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

        public void AddElement(GarazhBindingModel model)
        {
            Garazh element = source.Garazhs.FirstOrDefault(rec => rec.GarazhName == model.GarazhName);
            if (element != null)
            {
                throw new Exception("Уже есть гараж с таким названием");
            }
            int maxId = source.Garazhs.Count > 0 ? source.Garazhs.Max(rec => rec.Id) : 0;
            source.Garazhs.Add(new Garazh
            {
                Id = maxId + 1,
                GarazhName = model.GarazhName
            });
        }

        public void UpdElement(GarazhBindingModel model)
        {
            Garazh element = source.Garazhs.FirstOrDefault(rec =>
                                        rec.GarazhName == model.GarazhName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть гараж с таким названием");
            }
            element = source.Garazhs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.GarazhName = model.GarazhName;
        }

        public void DelElement(int id)
        {
            Garazh element = source.Garazhs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.GarazhDetalis.RemoveAll(rec => rec.GarazhId == id);
                source.Garazhs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}