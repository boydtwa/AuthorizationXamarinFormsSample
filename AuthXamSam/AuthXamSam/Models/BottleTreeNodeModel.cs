using AuthXamSam.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Xamarin.Forms.Internals;

namespace AuthXamSam.Models
{
    public abstract class BottleTreeNodeModel
    {
        private string title;
        private bool isParent;
        private string levelName;
        private int count;
        private AzureTableKey levelId;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
            }
        }
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
            }
        }
        public bool IsParent
        {
            get => isParent;
            set
            {
                isParent = value;
            }
        }
        public string LevelName
        {
            get => levelName;
            set
            {
                levelName = value;
            }
        }

        public AzureTableKey LevelId
        {
            get => levelId;
            set
            {
                levelId = value;
            }
        }
    }

    public class Vintage : BottleTreeNodeModel
    {
        public List<Color> Colors { get; set; }
    }

    public class Color : BottleTreeNodeModel
    {
        public List<WineType> WineTypes { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class WineType : BottleTreeNodeModel
    {
        public List<CountryRegion> CountryRegions { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class CountryRegion : BottleTreeNodeModel
    {
        public List<Winery> Wineries { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class Winery : BottleTreeNodeModel
    {
        public List<Wine> Wines { get; set; }
    }

    public class Wine : BottleTreeNodeModel
    {
        public List<Bottle> Bottles { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class Bottle : BottleTreeNodeModel
    {

    }
}
