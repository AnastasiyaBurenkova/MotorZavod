using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class DetaliServiceList : IDetaliService
    {
        private DataListSingleton source;

        public DetaliServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<DetaliViewModel> GetList()
        {
            List<DetaliViewModel> result = new List<DetaliViewModel>();
            for (int i = 0; i < source.Detalis.Count; ++i)
            {
                result.Add(new DetaliViewModel
                {
                    Id = source.Detalis[i].Id,
                    DetaliName = source.Detalis[i].DetaliName
                });
            }
            return result;
        }

        public DetaliViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Detalis.Count; ++i)
            {
                if (source.Detalis[i].Id == id)
                {
                    return new DetaliViewModel
                    {
                        Id = source.Detalis[i].Id,
                        DetaliName = source.Detalis[i].DetaliName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DetaliBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Detalis.Count; ++i)
            {
                if (source.Detalis[i].Id > maxId)
                {
                    maxId = source.Detalis[i].Id;
                }
                if (source.Detalis[i].DetaliName == model.DetaliName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Detalis.Add(new Detali
            {
                Id = maxId + 1,
                DetaliName = model.DetaliName
            });
        }

        public void UpdElement(DetaliBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Detalis.Count; ++i)
            {
                if (source.Detalis[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Detalis[i].DetaliName == model.DetaliName && 
                    source.Detalis[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Detalis[index].DetaliName = model.DetaliName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Detalis.Count; ++i)
            {
                if (source.Detalis[i].Id == id)
                {
                    source.Detalis.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
