using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Kmeans
{
    public class KMeans
    {
         

        public static List<ClusterPointCollection> DoKMeans(ClusterPointCollection points, int clusterCount)
        {
            //initialize all clusters
            List<ClusterPointCollection> allClusters = InitializeClusters(points, clusterCount);

            //start k-means clustering
            int movements = 1;
            while (movements > 0)
            {
                movements = 0;

                foreach (ClusterPointCollection cluster in allClusters) //for all clusters
                {
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++) //for all points in each cluster
                    {
                        ClusterPoint point = cluster[pointIndex];

                        int nearestCluster = FindNearestCluster(allClusters, point);
                        if (nearestCluster != allClusters.IndexOf(cluster)) //if point has moved
                        {
                            if (cluster.Count > 1) //cluster shall have minimum one point
                            {
                                ClusterPoint removedPoint = cluster.RemovePoint(point);
                                allClusters[nearestCluster].AddPoint(removedPoint);
                                movements += 1;
                            }
                        }
                    }
                }
            }

            return (allClusters);
        }


        public static List<ClusterPointCollection> InitializeClusters(ClusterPointCollection points,int clusterCount)
        {
            List<ClusterPointCollection> allClusters = new List<ClusterPointCollection>();
            HashSet<ClusterPoint> used = new HashSet<ClusterPoint>();
            Random r = new Random();
            int i = 0;
            while (i < clusterCount)
            {
                int index = r.Next() % clusterCount;
                if (used.Contains(points[index])) continue;

                used.Add(points[index]);
                i++;
                ClusterPointCollection cluster = new ClusterPointCollection();
                cluster.Centroid = points[index];
                cluster.AddPoint(points[index]);
                allClusters.Add(cluster);
            }

            foreach (var point in points)
            {
                if (used.Contains(point)) continue;

                int nearestCluster = FindNearestCluster(allClusters, point);
                allClusters[nearestCluster].AddPoint(point);

            }

            return allClusters;
        }

        public static int FindNearestCluster(List<ClusterPointCollection> allClusters, ClusterPoint point)
        {
            double minimumDistance = 0.0;
            int nearestClusterIndex = -1;

            for (int k = 0; k < allClusters.Count; k++) //find nearest cluster
            {
                double distance = ClusterPoint.FindDistance(point, allClusters[k].Centroid);
                if (k == 0)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = 0;
                }
                else if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = k;
                }
            }

            return (nearestClusterIndex);
        }

        public static List<ClusterPointCollection> DoKMeansSecondVariant(ClusterPointCollection points, int clusterCount)
        {
            double MIN_DISPLACEMENT = 0.001;

            List<ClusterPointCollection> allClusters = new List<ClusterPointCollection>();
            HashSet<ClusterPoint> used = new HashSet<ClusterPoint>();
            Random r = new Random();
            int i = 0;
            while (i < clusterCount)
            {
                int index = r.Next() % clusterCount;
                if (used.Contains(points[index])) continue;

                used.Add(points[index]);
                i++;
                ClusterPointCollection cluster = new ClusterPointCollection();
                cluster.Centroid = points[index];
                allClusters.Add(cluster);
            }

            double displacement = Double.MaxValue;

            do
            {
                foreach (var cluster in allClusters)
                {
                    cluster.Clear();
                }
                foreach (var point in points)
                {
                    int nearestCluster = FindNearestCluster(allClusters, point);
                    allClusters[nearestCluster].AddPointWithoutUpdate(point);
                }
                displacement = 0.0;
                foreach (var cluster in allClusters)
                {
                    displacement += cluster.UpdateCentroidDistance();
                }
                
            } while (displacement > MIN_DISPLACEMENT);

            return (allClusters);
        }
    }
}
