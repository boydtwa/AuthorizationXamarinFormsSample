using System;
using System.Collections.Generic;
using System.Text;

namespace AuthXamSam.Models
{
    public class CellarSummaryModel
    {
        public string Name { get; set; }
        public AzureTableKey CellarId { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public int BottleCount { get; set; }

        // Currencey value of Wine in Cellar
        public double Value { get; set; }

        public IList<BottleBriefModel> Bottles { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
