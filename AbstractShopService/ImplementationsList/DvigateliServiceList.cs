using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<DvigateliViewModel> result = new List<DvigateliViewModel>();
            for (int i = 0; i < source.Dvigatelis.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<DvigateliDetaliViewModel> dvigateliDetalis = new List<DvigateliDetaliViewModel>();
                for (int j = 0; j < source.DvigateliDetalis.Count; ++j)
                {
                    if (source.DvigateliDetalis[j].DvigateliId == source.Dvigatelis[i].Id)
                    {
                        string detaliDlyaDvigatelya = string.Empty;
                        for (int k = 0; k < source.Detalis.Count; ++k)
                        {
                            if (source.DvigateliDetalis[j].DetaliId == source.Detalis[k].Id)
                            {
                                detaliDlyaDvigatelya = source.Detalis[k].DetaliName;
                                break;
                            }
                        }
                        dvigateliDetalis.Add(new DvigateliDetaliViewModel
                        {
                            Id = source.DvigateliDetalis[j].Id,
                            DvigateliId = source.DvigateliDetalis[j].DvigateliId,
                            DetaliId = source.DvigateliDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.DvigateliDetalis[j].Count
                        });
                    }
                }
                result.Add(new DvigateliViewModel
                {
                    Id = source.Dvigatelis[i].Id,
                    DvigateliName = source.Dvigatelis[i].DvigateliName,
                    Price = source.Dvigatelis[i].Price,
                    DvigateliDetalis = dvigateliDetalis
                });
            }
            return result;
        }

        public DvigateliViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Dvigatelis.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<DvigateliDetaliViewModel> dvigateliDetalis = new List<DvigateliDetaliViewModel>();
                for (int j = 0; j < source.DvigateliDetalis.Count; ++j)
                {
                    if (source.DvigateliDetalis[j].DvigateliId == source.Dvigatelis[i].Id)
                    {
                        string detaliDlyaDvigatelya = string.Empty;
                        for (int k = 0; k < source.Detalis.Count; ++k)
                        {
                            if (source.DvigateliDetalis[j].DetaliId == source.Detalis[k].Id)
                            {
                                detaliDlyaDvigatelya = source.Detalis[k].DetaliName;
                                break;
                            }
                        }
                        dvigateliDetalis.Add(new DvigateliDetaliViewModel
                        {
                            Id = source.DvigateliDetalis[j].Id,
                            DvigateliId = source.DvigateliDetalis[j].DvigateliId,
                            DetaliId = source.DvigateliDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.DvigateliDetalis[j].Count
                        });
                    }
                }
                if (source.Dvigatelis[i].Id == id)
                {
                    return new DvigateliViewModel
                    {
                        Id = source.Dvigatelis[i].Id,
                        DvigateliName = source.Dvigatelis[i].DvigateliName,
                        Price = source.Dvigatelis[i].Price,
                        DvigateliDetalis = dvigateliDetalis
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(DvigateliBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Dvigatelis.Count; ++i)
            {
                if (source.Dvigatelis[i].Id > maxId)
                {
                    maxId = source.Dvigatelis[i].Id;
                }
                if (source.Dvigatelis[i].DvigateliName == model.DvigateliName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Dvigatelis.Add(new Dvigateli
            {
                Id = maxId + 1,
                DvigateliName = model.DvigateliName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.Dvigatelis[i].Id > maxPCId)
                {
                    maxPCId = source.DvigateliDetalis[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.DvigateliDetalis.Count; ++i)
            {
                for (int j = 1; j < model.DvigateliDetalis.Count; ++j)
                {
                    if (model.DvigateliDetalis[i].DetaliId ==
                        model.DvigateliDetalis[j].DetaliId)
                    {
                        model.DvigateliDetalis[i].Count +=
                            model.DvigateliDetalis[j].Count;
                        model.DvigateliDetalis.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.DvigateliDetalis.Count; ++i)
            {
                source.DvigateliDetalis.Add(new DvigateliDetali
                {
                    Id = ++maxPCId,
                    DvigateliId = maxId + 1,
                    DetaliId = model.DvigateliDetalis[i].DetaliId,
                    Count = model.DvigateliDetalis[i].Count
                });
            }
        }

        public void UpdElement(DvigateliBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Dvigatelis.Count; ++i)
            {
                if (source.Dvigatelis[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Dvigatelis[i].DvigateliName == model.DvigateliName &&
                    source.Dvigatelis[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Dvigatelis[index].DvigateliName = model.DvigateliName;
            source.Dvigatelis[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.DvigateliDetalis[i].Id > maxPCId)
                {
                    maxPCId = source.DvigateliDetalis[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.DvigateliDetalis[i].DvigateliId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.DvigateliDetalis.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.DvigateliDetalis[i].Id == model.DvigateliDetalis[j].Id)
                        {
                            source.DvigateliDetalis[i].Count = model.DvigateliDetalis[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.DvigateliDetalis.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.DvigateliDetalis.Count; ++i)
            {
                if (model.DvigateliDetalis[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.DvigateliDetalis.Count; ++j)
                    {
                        if (source.DvigateliDetalis[j].DvigateliId == model.Id &&
                            source.DvigateliDetalis[j].DetaliId == model.DvigateliDetalis[i].DetaliId)
                        {
                            source.DvigateliDetalis[j].Count += model.DvigateliDetalis[i].Count;
                            model.DvigateliDetalis[i].Id = source.DvigateliDetalis[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.DvigateliDetalis[i].Id == 0)
                    {
                        source.DvigateliDetalis.Add(new DvigateliDetali
                        {
                            Id = ++maxPCId,
                            DvigateliId = model.Id,
                            DetaliId = model.DvigateliDetalis[i].DetaliId,
                            Count = model.DvigateliDetalis[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.DvigateliDetalis.Count; ++i)
            {
                if (source.DvigateliDetalis[i].DvigateliId == id)
                {
                    source.DvigateliDetalis.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Dvigatelis.Count; ++i)
            {
                if (source.Dvigatelis[i].Id == id)
                {
                    source.Dvigatelis.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
