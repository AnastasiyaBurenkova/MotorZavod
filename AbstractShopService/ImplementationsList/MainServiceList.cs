using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<ZakazViewModel> result = new List<ZakazViewModel>();
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                string zakazchikFIO = string.Empty;
                for (int j = 0; j < source.Zakazchiks.Count; ++j)
                {
                    if (source.Zakazchiks[j].Id == source.Zakazs[i].ZakazchikId)
                    {
                        zakazchikFIO = source.Zakazchiks[j].ZakazchikFIO;
                        break;
                    }
                }
                string dvigateliTip = string.Empty;
                for (int j = 0; j < source.Dvigatelis.Count; ++j)
                {
                    if (source.Dvigatelis[j].Id == source.Zakazs[i].DvigateliId)
                    {
                        dvigateliTip = source.Dvigatelis[j].DvigateliName;
                        break;
                    }
                }
                string rabochiFIO = string.Empty;
                if (source.Zakazs[i].RabochiId.HasValue)
                {
                    for (int j = 0; j < source.Rabochis.Count; ++j)
                    {
                        if (source.Rabochis[j].Id == source.Zakazs[i].RabochiId.Value)
                        {
                            rabochiFIO = source.Rabochis[j].RabochiFIO;
                            break;
                        }
                    }
                }
                result.Add(new ZakazViewModel
                {
                    Id = source.Zakazs[i].Id,
                    ZakazchikId = source.Zakazs[i].ZakazchikId,
                    ZakazchikFIO = zakazchikFIO,
                    DvigateliId = source.Zakazs[i].DvigateliId,
                    DvigateliName = dvigateliTip,
                    RabochiId = source.Zakazs[i].RabochiId,
                    RabochiName = rabochiFIO,
                    Count = source.Zakazs[i].Count,
                    Sum = source.Zakazs[i].Sum,
                    DateCreate = source.Zakazs[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Zakazs[i].DateImplement?.ToLongDateString(),
                    Status = source.Zakazs[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazs[i].Id > maxId)
                {
                    maxId = source.Zakazchiks[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazs[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.DvigateliDetalis[i].DvigateliId == source.Zakazs[index].DvigateliId)
                {
                    int countOnGarazhs = 0;
                    for (int j = 0; j < source.GarazhDetalis.Count; ++j)
                    {
                        if (source.GarazhDetalis[j].DetaliId == source.DvigateliDetalis[i].DetaliId)
                        {
                            countOnGarazhs += source.GarazhDetalis[j].Count;
                        }
                    }
                    if (countOnGarazhs < source.DvigateliDetalis[i].Count * source.Zakazs[index].Count)
                    {
                        for (int j = 0; j < source.Detalis.Count; ++j)
                        {
                            if (source.Detalis[j].Id == source.DvigateliDetalis[i].DetaliId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Detalis[j].DetaliName +
                                    " требуется " + source.DvigateliDetalis[i].Count + ", в наличии " + countOnGarazhs);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.DvigateliDetalis[i].DvigateliId == source.Zakazs[index].DvigateliId)
                {
                    int countOnGarazhs = source.DvigateliDetalis[i].Count * source.Zakazs[index].Count;
                    for (int j = 0; j < source.GarazhDetalis.Count; ++j)
                    {
                        if (source.GarazhDetalis[j].DetaliId == source.DvigateliDetalis[i].DetaliId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.GarazhDetalis[j].Count >= countOnGarazhs)
                            {
                                source.GarazhDetalis[j].Count -= countOnGarazhs;
                                break;
                            }
                            else
                            {
                                countOnGarazhs -= source.GarazhDetalis[j].Count;
                                source.GarazhDetalis[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Zakazs[index].RabochiId = model.RabochiId;
            source.Zakazs[index].DateImplement = DateTime.Now;
            source.Zakazs[index].Status = ZakazStatus.Выполняется;
        }

        public void FinishZakaz(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazs[index].Status = ZakazStatus.Готов;
        }

        public void PayZakaz(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazs[index].Status = ZakazStatus.Оплачен;
        }

        public void PutDetaliOnGarazh(GarazhDetaliBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.GarazhDetalis.Count; ++i)
            {
                if (source.GarazhDetalis[i].GarazhId == model.GarazhId &&
                    source.GarazhDetalis[i].DetaliId == model.DetaliId)
                {
                    source.GarazhDetalis[i].Count += model.Count;
                    return;
                }
                if (source.GarazhDetalis[i].Id > maxId)
                {
                    maxId = source.GarazhDetalis[i].Id;
                }
            }
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
