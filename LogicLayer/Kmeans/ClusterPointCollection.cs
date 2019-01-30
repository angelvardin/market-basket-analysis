using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Kmeans
{
    public class ClusterPointCollection : List<ClusterPoint>
    {

        public ClusterPoint Centroid { get; set; }



        public ClusterPointCollection()
            : base()
        {
            Centroid = new ClusterPoint();
        }



        public void AddPoint(ClusterPoint p)
        {
            this.Add(p);
            UpdateCentroid();
        }

        public void AddPointWithoutUpdate(ClusterPoint p)
        {
            this.Add(p);
        }

        public ClusterPoint RemovePoint(int index)
        {
            ClusterPoint removedPoint = new ClusterPoint(this[index].Id, this[index].X, this[index].Y, this[index].Z);
            this.RemoveAt(index);
            UpdateCentroid();

            return (removedPoint);
        }

        public ClusterPoint RemovePoint(ClusterPoint p)
        {
            ClusterPoint removedPoint = new ClusterPoint(p.Id, p.X, p.Y, p.Z);
            this.Remove(p);
            UpdateCentroid();

            return (removedPoint);
        }

        public ClusterPoint GetPointNearestToCentroid()
        {
            double minimumDistance = 0.0;
            int nearestPointIndex = -1;

            foreach (ClusterPoint p in this)
            {
                double distance = ClusterPoint.FindDistance(p, Centroid);

                if (this.IndexOf(p) == 0)
                {
                    minimumDistance = distance;
                    nearestPointIndex = this.IndexOf(p);
                }
                else
                {
                    if (minimumDistance > distance)
                    {
                        minimumDistance = distance;
                        nearestPointIndex = this.IndexOf(p);
                    }
                }
            }

            return (this[nearestPointIndex]);
        }

        public double GetDistanceToCentroid()
        {
            double distance = 0.0;
            foreach (var item in this)
            {
                distance += ClusterPoint.FindDistance(this.Centroid, item);
            }
            return distance / this.Count;
        
        }


        public void UpdateCentroid()
        {
            double xSum = (from p in this select p.X).Sum();
            double ySum = (from p in this select p.Y).Sum();
            double zSum = (from p in this select p.Z).Sum();
            Centroid.X = (xSum / (double)this.Count);
            Centroid.Y = (ySum / (double)this.Count);
            Centroid.Z = (zSum / (double)this.Count);
        }

        public double UpdateCentroidDistance()
        {
            ClusterPoint OldCentoroid = new ClusterPoint()
            {
                X = Centroid.X,
                Y = Centroid.Y,
                Z = Centroid.Z,
            };
            double xSum = (from p in this select p.X).Sum();
            double ySum = (from p in this select p.Y).Sum();
            double zSum = (from p in this select p.Z).Sum();
            Centroid.X = (xSum / (double)this.Count);
            Centroid.Y = (ySum / (double)this.Count);
            Centroid.Z = (zSum / (double)this.Count);

            double distance = ClusterPoint.FindDistance(Centroid, OldCentoroid);

            return distance;
        }
    }
}
