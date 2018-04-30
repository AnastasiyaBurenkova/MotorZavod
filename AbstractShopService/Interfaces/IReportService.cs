using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IReportService
    {
        void SaveDvigateliPrice(ReportBindingModel model);

        List<GarazhsLoadViewModel> GetGarazhsLoad();

        void SaveGarazhsLoad(ReportBindingModel model);

        List<ZakazchikZakazsModel> GetZakazchikZakazs(ReportBindingModel model);

        void SaveZakazchikZakazs(ReportBindingModel model);
    }
}
