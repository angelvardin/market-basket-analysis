using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LogicLayer.Kmeans
{
    public class GenerateFromDb
    {
        private NorthwindEntities context = new NorthwindEntities();
        public ClusterPointCollection GetAllPoints 
        {
            get 
            {
                ClusterPointCollection allPoints = new ClusterPointCollection();
                Dictionary<int, ClusterPoint> points = new Dictionary<int, ClusterPoint>();
                foreach (var item in context.Order_Details)
                {
                    if (points.ContainsKey(item.ProductID))
                    {
                        points[item.ProductID].Y += item.Quantity;
                        points[item.ProductID].Z += item.Quantity * (double)item.UnitPrice*item.Discount;
                    }
                    else
                    {
                        double prize = 0.0;
                        if (item.Product.UnitPrice != null)
                        {
                            prize = (double)item.Product.UnitPrice.Value;
                        }
                        
                        points.Add(item.ProductID,
                            new ClusterPoint(item.ProductID, prize, item.Quantity,
                                item.Discount * (double)item.UnitPrice*item.Discount));
             
                    }
                }
                foreach (var item in points)
                {
                    allPoints.Add(item.Value);
                }
                return allPoints;
            }
        }
    }
}
