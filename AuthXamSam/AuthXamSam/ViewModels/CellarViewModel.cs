using AuthXamSam.Models;
using AuthXamSam.Services;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AuthXamSam.ViewModels
{
    public class CellarViewModel : BaseViewModel
    {
        private bool ThrowExceptionFlag { get; set; }
        private CellarListItem _selectedCellarListItem;
        private ObservableCollection<Vintage> _cellarVintages;
        private ObservableCollection<CellarListItem> cellarListItems;

        public ObservableCollection<CellarListItem> CellarListItems
        {
            get => cellarListItems;
            set => SetProperty(ref cellarListItems, value, "CellarListItems");
        }
        public ObservableCollection<Vintage> CellarVintages
        {
            get => _cellarVintages;
            set => SetProperty(ref _cellarVintages, value, "CellarVintages");
        }
        public IAsyncCommand LoadCellarsList { get; private set; }
        public CellarListItem SelectedCellarListItem
        {
            get => _selectedCellarListItem;
            set => SetProperty(ref _selectedCellarListItem, value);
        }

        [PreferredConstructor]
        public CellarViewModel()
        {
            InitializeViewModel(false);
        }

        public CellarViewModel(bool TestException=false, IWineStore Store = null)
        {
            this.WineStore = Store ?? this.WineStore;
#if DEBUG
            InitializeViewModel(TestException);
#else
            InitializeViewModel(false);
#endif
            
        }

        public async Task PopulateCellarListAsync()
        {
            try
            {
                if (ThrowExceptionFlag)
                {
                    throw new Exception();
                }
                IsBusy = true;
                var listCellarSummaryModel = await WineStore.GetCellarsAsync();
                var enumCellarListItems = ((from c in listCellarSummaryModel
                    select new CellarListItem()
                    {
                        Text = $"{c.Name}" + $" - Bottles: {c.BottleCount}" + (c.Capacity > 0
                            ? $" - % Capacity: {((c.BottleCount / c.Capacity) * 100):P}"
                            : string.Empty),
                        Key = c.CellarId,

                    }).AsEnumerable()).ToList();

                CellarListItems = new ObservableCollection<CellarListItem>(enumCellarListItems);
            }
            catch
            {
                _selectedCellarListItem = new CellarListItem() {Text = "Exception Occurred"};
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

        private void InitializeViewModel(bool TestException)
        {
            ThrowExceptionFlag = TestException;
            CellarListItems = new ObservableCollection<CellarListItem>();
            Title = "Home";
            CellarVintages = new ObservableCollection<Vintage>();
            LoadCellarsList = new AsyncCommand(()=>PopulateCellarListAsync(), ()=>CanExecute());
        }
    }
}
