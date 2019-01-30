using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Apriory
{
    public class GenerateFromDb
    {
        private NorthwindEntities context = new NorthwindEntities();
        private Itemset items = new Itemset();
        private ItemsetCollection db = new ItemsetCollection();

        public Itemset Products
        { 
            get 
            {
                foreach (var item in context.Products)
                {
                    items.Add(item.ProductName);
                }
                return items;
            } 
        }


        public ItemsetCollection Transaction
        { 
            get
            {
                Dictionary<int, Itemset> transactions = new Dictionary<int, Itemset>();
     
                foreach (var item in context.Order_Details.Include("Product"))
                {
                    if (transactions.ContainsKey(item.OrderID))
                    {
                        transactions[item.OrderID].Add(item.Product.ProductName);
                    }
                    else
                    {
                        transactions.Add(item.OrderID, new Itemset());
                        transactions[item.OrderID].Add(item.Product.ProductName);
                    }
                }
                foreach (var item in transactions)
                {
                    db.Add(item.Value);
                }
                return db;
            }
        }
    }
}
