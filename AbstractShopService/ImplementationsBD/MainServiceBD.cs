using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace AbstractShopService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ZakazViewModel> GetList()
        {
            List<ZakazViewModel> result = context.Zakazs
                .Select(rec => new ZakazViewModel
                {
                    Id = rec.Id,
                    ZakazchikId = rec.ZakazchikId,
                    DvigateliId = rec.DvigateliId,
                    RabochiId = rec.RabochiId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    ZakazchikFIO = rec.Zakazchik.ZakazchikFIO,
                    DvigateliName = rec.Dvigateli.DvigateliName,
                    RabochiName = rec.Rabochi.RabochiFIO
                })
                .ToList();
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            var Zakaz = new Zakaz
            {
                ZakazchikId = model.ZakazchikId,
                DvigateliId = model.DvigateliId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ZakazStatus.Принят
            };
            context.Zakazs.Add(Zakaz);
            context.SaveChanges();

            var Zakazchik = context.Zakazchiks.FirstOrDefault(x => x.Id == model.ZakazchikId);
            SendEmail(Zakazchik.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} создан успешно", Zakaz.Id,
                Zakaz.DateCreate.ToShortDateString()));
        }

        public void TakeZakazInWork(ZakazBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Zakaz element = context.Zakazs.Include(rec => rec.Zakazchik).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var DvigateliDetalis = context.DvigateliDetalis
                                                .Include(rec => rec.Detali)
                                                .Where(rec => rec.DvigateliId == element.DvigateliId);
                    // списываем
                    foreach (var DvigateliDetali in DvigateliDetalis)
                    {
                        int countOnGarazhs = DvigateliDetali.Count * element.Count;
                        var GarazhDetalis = context.GarazhDetalis
                                                    .Where(rec => rec.DetaliId == DvigateliDetali.DetaliId);
                        foreach (var GarazhDetali in GarazhDetalis)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (GarazhDetali.Count >= countOnGarazhs)
                            {
                                GarazhDetali.Count -= countOnGarazhs;
                                countOnGarazhs = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnGarazhs -= GarazhDetali.Count;
                                GarazhDetali.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnGarazhs > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                DvigateliDetali.Detali.DetaliName + " требуется " +
                                DvigateliDetali.Count + ", не хватает " + countOnGarazhs);
                        }
                    }
                    element.RabochiId = model.RabochiId;
                    element.DateImplement = DateTime.Now;
                    element.Status = ZakazStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Zakazchik.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передеан в работу", element.Id, element.DateCreate.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public void FinishZakaz(int id)
        {
            Zakaz element = context.Zakazs.Include(rec => rec.Zakazchik).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Zakazchik.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
                element.DateCreate.ToShortDateString()));
        }

        public void PayZakaz(int id)
        {
            Zakaz element = context.Zakazs.Include(rec => rec.Zakazchik).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Zakazchik.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateCreate.ToShortDateString()));
        }

        public void PutDetaliOnGarazh(GarazhDetaliBindingModel model)
        {
            GarazhDetali element = context.GarazhDetalis
                                                .FirstOrDefault(rec => rec.GarazhId == model.GarazhId &&
                                                                    rec.DetaliId == model.DetaliId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.GarazhDetalis.Add(new GarazhDetali
                {
                    GarazhId = model.GarazhId,
                    DetaliId = model.DetaliId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpZakazchik = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpZakazchik = new SmtpClient("smtp.gmail.com", 587);
                objSmtpZakazchik.UseDefaultCredentials = false;
                objSmtpZakazchik.EnableSsl = true;
                objSmtpZakazchik.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpZakazchik.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpZakazchik.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpZakazchik = null;
            }
        }
    }
}