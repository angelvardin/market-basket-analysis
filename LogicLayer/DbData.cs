using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using LogicLayer.Kmeans;

namespace LogicLayer
{
    public class DbData
    {
        private NorthwindEntities context = new NorthwindEntities();
        
        public Dictionary<int, Tuple<string, decimal>> AllProducts 
        { 
            get
            {
                NorthwindEntities context = new NorthwindEntities();
                Dictionary<int, Tuple<string, decimal>> products = new Dictionary<int, Tuple<string, decimal>>();
                foreach (var item in context.Products.ToList())
                {
                    if (!products.ContainsKey(item.ProductID))
                    {
                        decimal prize = item.UnitPrice!= null? item.UnitPrice.Value: 0.0m;                        
                        products.Add(item.ProductID, new Tuple<string, decimal>(item.ProductName,prize));
                    }
                    else
                    {
                        continue;
                    }
                }
                return products;
            }
        }

        public Dictionary<int,int> AllProductCount
        {
            get
            {
                Dictionary<int, int> points = new Dictionary<int, int>();
                foreach (var item in context.Order_Details)
                {
                    if (points.ContainsKey(item.ProductID))
                    {
                        points[item.ProductID] += item.Quantity;
                        
                    }
                    else
                    {
                        
                        points.Add(item.ProductID, item.Quantity);

                    }
                }

                return (points);

            }
        }
    }
}
