using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Apriory
{
    public class ItemsetCollection : List<Itemset>
    {

        public Itemset GetUniqueItems()
        {
            Itemset unique = new Itemset();

            foreach (Itemset itemset in this)
            {
                var result = from item in itemset
                             where !unique.Contains(item)
                             select item;
                unique.AddRange(result);
            }

            return (unique);
        }

        public double FindSupport(string item)
        {
            int matchCount = (from itemset in this
                              where itemset.Contains(item)
                              select itemset).Count();

            double support = ((double)matchCount / (double)this.Count) * 100.0;
            return (support);
        }

        public double FindSupport(Itemset itemset)
        {
            int matchCount = (from i in this
                              where i.Contains(itemset)
                              select i).Count();

            double support = ((double)matchCount / (double)this.Count) * 100.0;
            return (support);
        }

        public override string ToString()
        {
            return (string.Join("\r\n", (from itemset in this select itemset.ToString()).ToArray()));
        }

    }
}
