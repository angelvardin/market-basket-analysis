using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogicLayer;
using LogicLayer.Kmeans;
using System.Threading.Tasks;

namespace PrezentationLayer
{
    /// <summary>
    /// Interaction logic for KmeanWindow.xaml
    /// </summary>
    
    public partial class KmeanWindow : Window
    {
        
        public ClusterPointCollection AllPoints { get; set; }
        private List<ClusterPointCollection> allClusters;
        private List<double> clusterPrize;
        private bool Variant;

        public KmeanWindow()
        {
            InitializeComponent();
            Variant = true;
            this.DataContext = this;
            this.clusterPrize = new List<double>();
            FirstVariant.IsChecked = true;
            SecondVariant.IsChecked = false;
            SetData();

            this.ClusterCountNumericUpDown.ValueChanged += ClusterCountNumericUpDown_ValueChanged;
        }

        private void SetData()
        {
            LogicLayer.Kmeans.GenerateFromDb context = new GenerateFromDb();
            this.AllPoints = context.GetAllPoints;

            //DoKMeans();
        }

        private void DoKMeans()
        {
            List<ClusterPointCollection> best;
            if (Variant == true)
            {
                best = KMeans.DoKMeans(this.AllPoints, (int)ClusterCountNumericUpDown.Value);
                double bestDistance = FindDistance(best);

                for (int i = 0; i < RandomRestarts.Value - 1; i++)
                {
                    List<ClusterPointCollection> current = KMeans.DoKMeans(this.AllPoints, (int)ClusterCountNumericUpDown.Value);
                    double currentDistance = FindDistance(current);
                    if (currentDistance < bestDistance)
                    {
                        best = current;
                        bestDistance = currentDistance;
                    }
                }
            }
            else
            {
                best = KMeans.DoKMeansSecondVariant(this.AllPoints, (int)ClusterCountNumericUpDown.Value);
                double bestDistance = FindDistance(best);

                for (int i = 0; i < RandomRestarts.Value - 1; i++)
                {
                    List<ClusterPointCollection> current = KMeans.DoKMeansSecondVariant(this.AllPoints, (int)ClusterCountNumericUpDown.Value);
                    double currentDistance = FindDistance(current);
                    if (currentDistance < bestDistance)
                    {
                        best = current;
                        bestDistance = currentDistance;
                    }
                }
            }
            

            allClusters = best;
            //render kmeans-graph
            PointsChart.Series.Clear();
            PointsChart.LegendTitle = allClusters.Count + " Clusters";
            foreach (ClusterPointCollection cluster in allClusters)
            {
                Dictionary<double, double> chartSeriesData = new Dictionary<double, double>();
                foreach (ClusterPoint point in cluster)
                {
                    while (chartSeriesData.ContainsKey(point.X)) point.X += 0.01; //for duplicate points 
                    chartSeriesData.Add(point.X, point.Y);
                }

                PointsChart.Series.Add(CreateScatterSeries("Cluster" + (allClusters.IndexOf(cluster) + 1), chartSeriesData));
            }
            PopulatePrizeChart();
        }

        private ScatterSeries CreateScatterSeries(string title, Dictionary<double, double> data)
        {
            ScatterSeries scatterSeries = new ScatterSeries();
            scatterSeries.Title = title;
            scatterSeries.ItemsSource = data;
            scatterSeries.IndependentValueBinding = new Binding("Key");
            scatterSeries.DependentValueBinding = new Binding("Value");
            return (scatterSeries);
        }

        private ColumnSeries CreateColumnSeries(string title, Dictionary<string, double> data)
        {
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Title = title;
            columnSeries.ItemsSource = data;
            columnSeries.IndependentValueBinding = new Binding("Key");
            columnSeries.DependentValueBinding = new Binding("Value");
            return columnSeries;
        }

        private void ClusterCountNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.AllPoints.Count < ClusterCountNumericUpDown.Value)
            {
                MessageBox.Show(ClusterCountNumericUpDown.Value + " clusters cannot be formed using " + this.AllPoints.Count + " points");
                ClusterCountNumericUpDown.Value = this.AllPoints.Count;
                return;
            }
        }

        private void PopulatePrizeChart()
        {
            PrizeChart.Series.Clear();
            clusterPrize.Clear();

            Dictionary<string, double> data = new Dictionary<string, double>();
            int i = 0;
            
            foreach (var cluster in allClusters)
            {
                double prize = 0.0;
                foreach (var item in cluster)
                {
                    prize += item.X * item.Y - item.Z;
                }
                i++;
                data.Add("Cluster " + i, prize);
                clusterPrize.Add(prize);
            }
            PrizeChart.Series.Add(CreateColumnSeries("Prize", data));

        }

        private void DoKmean_Click(object sender, RoutedEventArgs e)
        {
            DoKMeans();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClustersWindow window = new ClustersWindow(allClusters, clusterPrize);
            window.Show();
        }

        private double FindDistance(List<ClusterPointCollection> allClusters)
        {
            double distance = 0.0;
            foreach (var item in allClusters)
            {
                distance += item.GetDistanceToCentroid();
            }
            return distance;
        }

        private void FirstVariant_Checked(object sender, RoutedEventArgs e)
        {
            Variant = true;
        }

        private void SecondVariant_Checked(object sender, RoutedEventArgs e)
        {
            Variant = false;
        }
    }
}
