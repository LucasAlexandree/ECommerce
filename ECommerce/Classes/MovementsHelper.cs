using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class MovementsHelper : IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public static Response NewOrder(NewOrderView view, string userName)
        {
            using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = new Orders
                    {
                        CompanyId = user.CompanyId,
                        CustomerId = view.CustomerId,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateId = DBHelper.GetState("Criado", db),
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();
                    var details = db.OrderDetailTmp.Where(odt => odt.UserName == userName).ToList();

                    foreach (var detail in details)
                    {
                        var orderDetail = new OrderDetails
                        {
                            Description = detail.Description,
                            OrderId = order.OrderId,
                            Price = detail.Price,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate
                        };

                        db.OrderDetail.Add(orderDetail);
                        db.OrderDetailTmp.Remove(detail);

                    }

                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response {

                        Message = ex.Message,
                        Succeeded = false

                    };
                    throw;
                }
            }
        }




        public static Response NewPurchase(NewPurchaseView view, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var purchase = new Purchase
                    {
                        CompanyId = user.CompanyId,
                        ProviderId = view.ProviderId,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateId = DBHelper.GetState("Criado", db),
                        WareHouseId = view.ProviderId
                    };

                    db.Purchase.Add(purchase);
                    db.SaveChanges();
                    var details = db.PurchaseDetailTmps.Where(odt => odt.UserName == userName).ToList();

                    foreach (var detail in details)
                    {
                        var purchaseDetail = new PurchaseDetails
                        {
                            Description = detail.Description,
                            PurchaseId = purchase.PurchaseId,
                            Price = detail.Price,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate
                        };

                        db.PurchaseDetails.Add(purchaseDetail);
                        db.PurchaseDetailTmps.Remove(detail);
                       

                    }


                    

                    
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {

                        Message = ex.Message,
                        Succeeded = false

                    };
                    throw;
                }
            }
        }




    }
}