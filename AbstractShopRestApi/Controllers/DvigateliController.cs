using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractShopRestApi.Controllers
{
    public class DvigateliController : ApiController
    {
        private readonly IDvigateliService _service;

        public DvigateliController(IDvigateliService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(DvigateliBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(DvigateliBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(DvigateliBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
