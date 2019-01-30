using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Kmeans
{
    public class ClusterPoint
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public ClusterPoint()
        {
            Id = -1;
            X = -1;
            Y = -1;
            Z = -1;
        }

        public ClusterPoint(double x, double y)
        {
            this.Id = -1;
            this.X = x;
            this.Y = y;
            this.Z = -1;
        }

        public ClusterPoint(int id, double x, double y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }

        public ClusterPoint(int id, double x, double y, double z)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool IsBetween(ClusterPoint p1, ClusterPoint p2)
        {
            if (this.X >= p1.X && this.X <= p2.X)
            {
                if (this.Y >= p1.Y && this.Y <= p2.Y)
                {
                    if (this.Z >= p1.Z && this.Z <= p2.Z)
                    {
                        return (true);
                    }
                    
                }
            }

            return (false);
        }

        public override bool Equals(object obj)
        {
            if (obj is ClusterPoint)
            {
                ClusterPoint point = (ClusterPoint)obj;
                if (this.X == point.X && this.Y == point.Y && this.Z==point.Z)
                {
                    return (true);
                }
            }

            return (false);
        }

        public override string ToString()
        {
            return ("{" + this.X + "," + this.Y + "," + this.Z + "}");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static double FindDistance(ClusterPoint pt1, ClusterPoint pt2)
        {
            double x1 = pt1.X, y1 = pt1.Y, z1 = pt1.Z;
            double x2 = pt2.X, y2 = pt2.Y, z2 = pt1.Z;


            //find euclidean distance
            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0)+Math.Pow(z2 - z1, 2.0));
            return (distance);
        }

    }
}
