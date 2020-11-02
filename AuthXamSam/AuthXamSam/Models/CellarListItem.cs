using System;
using System.Collections.Generic;
using System.Text;

namespace AuthXamSam.Models
{
    public class CellarListItem
    {
        public string Text { get; set; }
        public AzureTableKey Key { get; set; }
    }
}
