using LogicLayer;
using LogicLayer.Kmeans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrezentationLayer
{
    /// <summary>
    /// Interaction logic for ClustersWindow.xaml
    /// </summary>
    public partial class ClustersWindow : Window
    {
        private List<ClusterPointCollection> allClusters;
        private List<double> clusterPrize;

        public ClustersWindow()
        {
            InitializeComponent();
            this.allClusters = new List<ClusterPointCollection>();
            this.clusterPrize = new List<double>();
            SetData();
        }

        public ClustersWindow(List<ClusterPointCollection> allClusters, List<double> clusterPrize)
        {
            InitializeComponent();
            if (allClusters != null && clusterPrize != null)
            {
                this.allClusters = allClusters;
                this.clusterPrize = clusterPrize;
                SetData();
            }
            else
            {
                this.allClusters = new List<ClusterPointCollection>();
                this.clusterPrize = new List<double>();
            } 
        }

        private void SetData()
        {
            int i = 1;
            DbData products = new DbData();
            foreach (var clusterCollection in allClusters)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = "Cluster" + i + "  (Income: "+ String.Format("{0:0.##}", clusterPrize[i-1]) + " $)";
                treeItem.FontWeight = FontWeights.Bold;
                i++;
                int totalCount = 0;
                foreach (var item in clusterCollection)
                {
                    int count = products.AllProductCount[item.Id];
                    totalCount += count;
                    string line = products.AllProducts[item.Id].Item1 +"   (Prize:" 
                        + String.Format("{0:0.##}", products.AllProducts[item.Id].Item2) + " $" +
                        " Count "+ count + ")";

                    treeItem.Items.Add(new TreeViewItem() { Header =  line });
                }
                treeItem.Items.Add(new TreeViewItem() { Header = "Total Count " + totalCount });
                ClusterTreeWiev.Items.Add(treeItem);
            }
        }

        private void Closebtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
