using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractShopRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetGarazhsLoad()
        {
            var list = _service.GetGarazhsLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetZakazchikZakazs(ReportBindingModel model)
        {
            var list = _service.GetZakazchikZakazs(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveDvigateliPrice(ReportBindingModel model)
        {
            _service.SaveDvigateliPrice(model);
        }

        [HttpPost]
        public void SaveGarazhsLoad(ReportBindingModel model)
        {
            _service.SaveGarazhsLoad(model);
        }

        [HttpPost]
        public void SaveZakazchikZakazs(ReportBindingModel model)
        {
            _service.SaveZakazchikZakazs(model);
        }
    }
}