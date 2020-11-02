using AuthXamSam.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AuthXamSam.ViewModels
{
    public class CellarViewModel : BaseViewModel
    {
        private CellarListItem selectedCellarListItem;
        private ObservableCollection<CellarListItem> cellarListItems;

        public ObservableCollection<CellarListItem> CellarListItems
        {
            get => cellarListItems;
            set
            {
                SetProperty(ref cellarListItems, value, "CellarListItems");
            }
        }

        public CellarListItem SelectedCellarListItem
        {
            get => selectedCellarListItem;
            set
            {
                SetProperty(ref selectedCellarListItem, value);
            }
        }

        public IAsyncCommand LoadCellarsList { get; private set; }

        public CellarViewModel()
        {
            CellarListItems = new ObservableCollection<CellarListItem>();
            Title = "Home";
            LoadCellarsList = new AsyncCommand(() => PopulateCellarListAsync(), () => CanExecute());

        }

        private async Task PopulateCellarListAsync()
        {
            try
            {
                IsBusy = true;
                var listCellarSummaryModel = await WineStore.GetCellarsAsync();
                var enumCellarListItems = ((from c in listCellarSummaryModel
                                            select new CellarListItem()
                                            {
                                                Text = $"{c.Name}" + $" - Bottles: {c.BottleCount}" + (c.Capacity > 0 ? $" - % Capacity: {((c.BottleCount / c.Capacity) * 100).ToString("P")}" : string.Empty),
                                                Key = c.CellarId,
                                            }).AsEnumerable()).ToList();

                CellarListItems = new ObservableCollection<CellarListItem>(enumCellarListItems);
            }
            finally
            {
                if (CellarListItems.Count > 0)
                {
                    SelectedCellarListItem = CellarListItems.First();
                }

                IsBusy = false;
            }
        }
        private bool CanExecute()
        {
            return !IsBusy;
        }

    }
}
