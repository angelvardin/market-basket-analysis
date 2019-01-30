using LogicLayer.Apriory;
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
    /// Interaction logic for AprioryWindow.xaml
    /// </summary>
    public partial class AprioryWindow : Window
    {
        private Itemset _items;
        private ItemsetCollection _db;
        private Paragraph para = new Paragraph();

        public AprioryWindow()
        {
            InitializeComponent();
            GenerateSimpleData();
        }
        private void GenerateSimpleData()
        {
            _items = new Itemset();
            
            _items.Add("кроасан");
            _items.Add("бира");
            _items.Add("чипс");
            _items.Add("хляб");
            _items.Add("чадър");
            _items.Add("торта");
            _items.Add("сирене");
            _items.Add("coca-cola");
    
            ItemsTxtbx.Text = string.Join(",", _items.ToArray());

            _db = new ItemsetCollection();
            _db.Add(new Itemset() { _items[0], _items[1], _items[2], _items[3], _items[4] });
            _db.Add(new Itemset() { _items[1], _items[2] });
            _db.Add(new Itemset() { _items[0], _items[1], _items[5] });
            _db.Add(new Itemset() { _items[1], _items[0], _items[6] });
            _db.Add(new Itemset() { _items[0], _items[5], _items[7] });


            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run(_db.ToString())));
            TransactionTxtbx.Document = myFlowDoc;


            TransactionNumberNTxtbx.Text = _db.Count.ToString();
            ItemsTxtbx.IsEnabled = false;

            SupportThresholdTxtbx.Text = "40.00";
            ConfidenceThresholdTxtbx.Text = "70.00";

        }

        private void Generatebtn_Click(object sender, RoutedEventArgs e)
        {
            //get items 
            Itemset items = new Itemset();
            items.AddRange(ItemsTxtbx.Text.Split(','));

            int transactionCount = 5;
            int.TryParse(TransactionNumberNTxtbx.Text, out transactionCount);
            Random rnd = new Random();

            //create random transactions
            _db = new ItemsetCollection();
            for (int transactionIndex = 0; transactionIndex < transactionCount; )
            {
                int itemCount = rnd.Next(items.Count);

                Itemset transaction = new Itemset();
                for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                {
                    int randomItemIndex = rnd.Next(itemCount);
                    if (!transaction.Contains(items[randomItemIndex]))
                    {
                        transaction.Add(items[randomItemIndex]);
                    }
                }

                if (transaction.Count > 0)
                {
                    _db.Add(transaction);
                    transactionIndex += 1;
                }
            }

            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run(_db.ToString())));
            ResultRTxtAr.Document.Blocks.Clear();

            TransactionTxtbx.Document = myFlowDoc;

            ResultRTxtAr.IsEnabled = false;
            TransactionTxtbx.IsEnabled = true;
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultRTxtAr.Document.Blocks.Clear();
            para.Inlines.Clear();

            //get items
            Itemset uniqueItems = _db.GetUniqueItems();

            //do apriori
            double supportThreshold = double.Parse(SupportThresholdTxtbx.Text);

            ItemsetCollection L = AprioriMining.DoApriori(_db, supportThreshold);
            AddResultLine(L.Count + " Large Itemsets");
            foreach (Itemset itemset in L)
            {
                AddResultLine(itemset.ToString());
            }
            AddResultLine(string.Empty);

            //do mining
            double confidenceThreshold = double.Parse(ConfidenceThresholdTxtbx.Text);

            List<AssociationRule> allRules = AprioriMining.Mine(_db, L, confidenceThreshold);
            AddResultLine(allRules.Count + " Association Rules");
            foreach (AssociationRule rule in allRules)
            {
                AddResultLine(rule.ToString());
            }
            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(para);
            ResultRTxtAr.Document = myFlowDoc;
            ResultRTxtAr.IsEnabled = true;
        }

        private void AddResultLine(string text)
        {
            para.Inlines.Add(new Bold(new Run(text + "\r\n")));
        }

        private void GetFromDbBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateFromDb context = new GenerateFromDb();
            _items = context.Products;
            _db = context.Transaction;

            ItemsTxtbx.Text = string.Join(",", _items.Take(20).ToArray());

            TransactionTxtbx.Document.Blocks.Clear();
            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run(_db.ToString())));
            TransactionTxtbx.Document = myFlowDoc;

            TransactionNumberNTxtbx.Text = _db.Count.ToString();
            ResultRTxtAr.Document.Blocks.Clear();

            ResultRTxtAr.IsEnabled = false;
            TransactionTxtbx.IsEnabled = true;
            ItemsTxtbx.IsEnabled = true;
        }


    }
}
