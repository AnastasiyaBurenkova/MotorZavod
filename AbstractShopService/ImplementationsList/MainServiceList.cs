using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ZakazViewModel> GetList()
        {
            List<ZakazViewModel> result = source.Zakazs
                .Select(rec => new ZakazViewModel
                {
                    Id = rec.Id,
                    ZakazchikId = rec.ZakazchikId,
                    DvigateliId = rec.DvigateliId,
                    RabochiId = rec.RabochiId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    ZakazchikFIO = source.Zakazchiks
                                    .FirstOrDefault(recC => recC.Id == rec.ZakazchikId)?.ZakazchikFIO,
                    DvigateliName = source.Dvigatelis
                                    .FirstOrDefault(recP => recP.Id == rec.DvigateliId)?.DvigateliName,
                    RabochiName = source.Rabochis
                                    .FirstOrDefault(recI => recI.Id == rec.RabochiId)?.RabochiFIO
                })
                .ToList();
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            int maxId = source.Zakazs.Count > 0 ? source.Zakazs.Max(rec => rec.Id) : 0;
            source.Zakazs.Add(new Zakaz
            {
                Id = maxId + 1,
                ZakazchikId = model.ZakazchikId,
                DvigateliId = model.DvigateliId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ZakazStatus.Принят
            });
        }

        public void TakeZakazInWork(ZakazBindingModel model)
        {
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var DvigateliDetalis = source.DvigateliDetalis.Where(rec => rec.DvigateliId == element.DvigateliId);
            foreach (var DvigateliDetali in DvigateliDetalis)
            {
                int countOnGarazhs = source.GarazhDetalis
                                            .Where(rec => rec.DetaliId == DvigateliDetali.DetaliId)
                                            .Sum(rec => rec.Count);
                if (countOnGarazhs < DvigateliDetali.Count * element.Count)
                {
                    var DetaliName = source.Detalis
                                    .FirstOrDefault(rec => rec.Id == DvigateliDetali.DetaliId);
                    throw new Exception("Не достаточно компонента " + DetaliName?.DetaliName +
                        " требуется " + DvigateliDetali.Count + ", в наличии " + countOnGarazhs);
                }
            }
            // списываем
            foreach (var DvigateliDetali in DvigateliDetalis)
            {
                int countOnGarazhs = DvigateliDetali.Count * element.Count;
                var GarazhDetalis = source.GarazhDetalis
                                            .Where(rec => rec.DetaliId == DvigateliDetali.DetaliId);
                foreach (var GarazhDetali in GarazhDetalis)
                {
                    // компонентов на одном слкаде может не хватать
                    if (GarazhDetali.Count >= countOnGarazhs)
                    {
                        GarazhDetali.Count -= countOnGarazhs;
                        break;
                    }
                    else
                    {
                        countOnGarazhs -= GarazhDetali.Count;
                        GarazhDetali.Count = 0;
                    }
                }
            }
            element.RabochiId = model.RabochiId;
            element.DateImplement = DateTime.Now;
            element.Status = ZakazStatus.Выполняется;
        }

        public void FinishZakaz(int id)
        {
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Готов;
        }

        public void PayZakaz(int id)
        {
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Оплачен;
        }

        public void PutDetaliOnGarazh(GarazhDetaliBindingModel model)
        {
            GarazhDetali element = source.GarazhDetalis
                                                .FirstOrDefault(rec => rec.GarazhId == model.GarazhId &&
                                                                    rec.DetaliId == model.DetaliId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.GarazhDetalis.Count > 0 ? source.GarazhDetalis.Max(rec => rec.Id) : 0;
                source.GarazhDetalis.Add(new GarazhDetali
                {
                    Id = ++maxId,
                    GarazhId = model.GarazhId,
                    DetaliId = model.DetaliId,
                    Count = model.Count
                });
            }
        }
    }
}