using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using System;
using System.Web.Http;
using AbstractShopRestApi.Services;

namespace AbstractShopRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateZakaz(ZakazBindingModel model)
        {
            _service.CreateZakaz(model);
        }

        [HttpPost]
        public void TakeZakazInWork(ZakazBindingModel model)
        {
            _service.TakeZakazInWork(model);
        }

        [HttpPost]
        public void FinishZakaz(ZakazBindingModel model)
        {
            _service.FinishZakaz(model.Id);
        }

        [HttpPost]
        public void PayZakaz(ZakazBindingModel model)
        {
            _service.PayZakaz(model.Id);
        }

        [HttpPost]
        public void PutDetaliOnGarazh(GarazhDetaliBindingModel model)
        {
            _service.PutDetaliOnGarazh(model);
        }
        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}