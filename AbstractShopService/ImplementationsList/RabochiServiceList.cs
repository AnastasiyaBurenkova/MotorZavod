using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class RabochiServiceList : IRabochiService
    {
        private DataListSingleton source;

        public RabochiServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<RabochiViewModel> GetList()
        {
            List<RabochiViewModel> result = new List<RabochiViewModel>();
            for (int i = 0; i < source.Rabochis.Count; ++i)
            {
                result.Add(new RabochiViewModel
                {
                    Id = source.Rabochis[i].Id,
                    RabochiFIO = source.Rabochis[i].RabochiFIO
                });
            }
            return result;
        }

        public RabochiViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Rabochis.Count; ++i)
            {
                if (source.Rabochis[i].Id == id)
                {
                    return new RabochiViewModel
                    {
                        Id = source.Rabochis[i].Id,
                        RabochiFIO = source.Rabochis[i].RabochiFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(RabochiBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Rabochis.Count; ++i)
            {
                if (source.Rabochis[i].Id > maxId)
                {
                    maxId = source.Rabochis[i].Id;
                }
                if (source.Rabochis[i].RabochiFIO == model.RabochiFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Rabochis.Add(new Rabochi
            {
                Id = maxId + 1,
                RabochiFIO = model.RabochiFIO
            });
        }

        public void UpdElement(RabochiBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Rabochis.Count; ++i)
            {
                if (source.Rabochis[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Rabochis[i].RabochiFIO == model.RabochiFIO &&
                    source.Rabochis[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Rabochis[index].RabochiFIO = model.RabochiFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Rabochis.Count; ++i)
            {
                if (source.Rabochis[i].Id == id)
                {
                    source.Rabochis.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
