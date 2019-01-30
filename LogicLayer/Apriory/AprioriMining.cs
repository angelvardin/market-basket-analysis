using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Apriory
{
    public class AprioriMining
    {
        public static ItemsetCollection DoApriori(ItemsetCollection db, double supportThreshold)
        {
            Itemset I = db.GetUniqueItems();
            ItemsetCollection L = new ItemsetCollection(); //resultant large itemsets
            ItemsetCollection Li = new ItemsetCollection(); //large itemset in each iteration
            ItemsetCollection Ci = new ItemsetCollection(); //candidate itemset in each iteration

            //first iteration 
            foreach (string item in I)
            {
                Ci.Add(new Itemset() { item });
            }

            //next iterations
            int k = 2;
            while (Ci.Count != 0)
            {
                //set Li from Ci 
                Li.Clear();
                foreach (Itemset itemset in Ci)
                {
                    itemset.Support = db.FindSupport(itemset);
                    if (itemset.Support >= supportThreshold)
                    {
                        Li.Add(itemset);
                        L.Add(itemset);
                    }
                }

                //set Ci for next iteration 
                Ci.Clear();
                Ci.AddRange(Bit.FindSubsets(Li.GetUniqueItems(), k)); //get k-item subsets
                k += 1;
            }

            return (L);
        }

        public static List<AssociationRule> Mine(ItemsetCollection db, ItemsetCollection L, double confidenceThreshold)
        {
            List<AssociationRule> allRules = new List<AssociationRule>();

            foreach (Itemset itemset in L)
            {
                ItemsetCollection subsets = Bit.FindSubsets(itemset, 0); //get all subsets
                foreach (Itemset subset in subsets)
                {
                    double confidence = (db.FindSupport(itemset) / db.FindSupport(subset)) * 100.0;
                    if (confidence >= confidenceThreshold)
                    {
                        AssociationRule rule = new AssociationRule();
                        rule.X.AddRange(subset);
                        rule.Y.AddRange(itemset.Remove(subset));
                        rule.Support = db.FindSupport(itemset);
                        rule.Confidence = confidence;
                        if (rule.X.Count > 0 && rule.Y.Count > 0)
                        {
                            allRules.Add(rule);
                        }
                    }
                }
            }

            return (allRules);
        }
    }
}
