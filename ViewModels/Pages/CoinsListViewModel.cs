using Binance.Data;
using Binance.Infrastructure.Commands;
using Binance.Models;
using Binance.Services;
using Binance.Views.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Binance.ViewModels.Pages
{
    internal class CoinsListViewModel : ViewModel
    {

        #region Prop

        #region PartTitle
        private string _pairTitle;
        public string PairTitle
        {
            get => _pairTitle;
            set => SetProperty(ref _pairTitle, value);
        }


        #endregion
        #region Коллекция пар (контроллов)
        private ObservableCollection<CoinItemView> _pairList = new ObservableCollection<CoinItemView>();
        public ObservableCollection<CoinItemView> PairList
        {
            get => _pairList;
            set => SetProperty(ref _pairList, value);
        }
        #endregion
        #endregion

        #region Command
        #region AddPair
        public ICommand AddPairCommand { get; }
        private bool CanAddPairCommandExcecut(object p) => true;
        private void OnAddPairCommandExecuted(object p)
        {
            DataBase.AddPair(new Pair(PairTitle));
            UpdateList();
        }

        #endregion

        #endregion

        private void UpdateList()
        {
            List<Pair> pairs = DataBase.GetAllPair();
            PairList.Clear();
            foreach (Pair pair in pairs)
            {
                var CIV = new CoinItemView(pair);
                CIV.DeleteItem += CIV_DeleteItem;
                PairList.Add(CIV);
            }
        }

        private void CIV_DeleteItem(object sender, System.EventArgs e)
        {
            var coinView = sender as CoinItemView;
            DataBase.DeletePair(coinView.Pair);
            UpdateList();
        }

        public CoinsListViewModel()
        {
            AddPairCommand = new RelayCommand(OnAddPairCommandExecuted, CanAddPairCommandExcecut);
            UpdateList();
        }
    }
}
