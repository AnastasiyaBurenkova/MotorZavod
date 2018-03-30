using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<GarazhViewModel> result = new List<GarazhViewModel>();
            for (int i = 0; i < source.Garazhs.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<GarazhDetaliViewModel> GarazhDetalis = new List<GarazhDetaliViewModel>();
                for (int j = 0; j < source.GarazhDetalis.Count; ++j)
                {
                    if (source.GarazhDetalis[j].GarazhId == source.Garazhs[i].Id)
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
                        GarazhDetalis.Add(new GarazhDetaliViewModel
                        {
                            Id = source.GarazhDetalis[j].Id,
                            GarazhId = source.GarazhDetalis[j].GarazhId,
                            DetaliId = source.GarazhDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.GarazhDetalis[j].Count
                        });
                    }
                }
                result.Add(new GarazhViewModel
                {
                    Id = source.Garazhs[i].Id,
                    GarazhName = source.Garazhs[i].GarazhName,
                    GarazhDetalis = GarazhDetalis
                });
            }
            return result;
        }

        public GarazhViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Garazhs.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<GarazhDetaliViewModel> GarazhDetalis = new List<GarazhDetaliViewModel>();
                for (int j = 0; j < source.GarazhDetalis.Count; ++j)
                {
                    if (source.GarazhDetalis[j].GarazhId == source.Garazhs[i].Id)
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
                        GarazhDetalis.Add(new GarazhDetaliViewModel
                        {
                            Id = source.GarazhDetalis[j].Id,
                            GarazhId = source.GarazhDetalis[j].GarazhId,
                            DetaliId = source.GarazhDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.GarazhDetalis[j].Count
                        });
                    }
                }
                if (source.Garazhs[i].Id == id)
                {
                    return new GarazhViewModel
                    {
                        Id = source.Garazhs[i].Id,
                        GarazhName = source.Garazhs[i].GarazhName,
                        GarazhDetalis = GarazhDetalis
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(GarazhBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Garazhs.Count; ++i)
            {
                if (source.Garazhs[i].Id > maxId)
                {
                    maxId = source.Garazhs[i].Id;
                }
                if (source.Garazhs[i].GarazhName == model.GarazhName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Garazhs.Add(new Garazh
            {
                Id = maxId + 1,
                GarazhName = model.GarazhName
            });
        }

        public void UpdElement(GarazhBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Garazhs.Count; ++i)
            {
                if (source.Garazhs[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Garazhs[i].GarazhName == model.GarazhName &&
                    source.Garazhs[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Garazhs[index].GarazhName = model.GarazhName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.GarazhDetalis.Count; ++i)
            {
                if (source.GarazhDetalis[i].GarazhId == id)
                {
                    source.GarazhDetalis.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Garazhs.Count; ++i)
            {
                if (source.Garazhs[i].Id == id)
                {
                    source.Garazhs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
