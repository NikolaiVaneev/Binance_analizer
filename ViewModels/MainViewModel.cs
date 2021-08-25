using Binance.Data;
using Binance.Infrastructure.Commands;
using Binance.Services;
using Binance.Views.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using Binance.Models;
using System.Windows.Input;
using WebSocketSharp;
using Binance.Views.Pages;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

namespace Binance.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        #region Свойства
        ObservableCollection<CoinView> coins = new ObservableCollection<CoinView>();

        #region Заголовок окна
        private string _title = "Binance analyzer";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        #endregion

        #region Статус программы
        private string _status = "Статус";
        /// <summary>Статус программы</summary>
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        #endregion

        #region Общий период
        private int _totalPeriod;
        /// <summary>Сколько секунд вообще</summary>
        public int TotalPeriod
        {
            get => _totalPeriod;
            set
            {
                AppVar.TolalPeriod = value;
                SetProperty(ref _totalPeriod, value);
            }
        }
        #endregion

        #region Период анализа
        private int _meanPeriod;
        /// <summary>Сколько секунд среднее значение</summary>
        public int MeanPeriod
        {
            get => _meanPeriod;
            set
            {
                AppVar.MeanPeriod = value;
                SetProperty(ref _meanPeriod, value);
            }
        }
        #endregion

        #region Процент уведомления
        private double _alertPercent;
        /// <summary>Сколько секунд среднее значение</summary>
        public double AlertPercent
        {
            get => _alertPercent;
            set
            {
                AppVar.AlertPercent = value;
                SetProperty(ref _alertPercent, value);
            }
        }
        #endregion

        #region Главное содержимое 
        private ObservableCollection<CoinView> _content;
        /// <summary>Главное содержимое</summary>
        public ObservableCollection<CoinView> Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        #endregion
        #endregion

        #region Команды

        #region SocketInit
        public ICommand SocketInitCommand { get; }
        private bool CanSocketInitCommandExcecut(object p) => true;
        private void OnSocketInitCommandExecuted(object p)
        {
            Socket.InitAsync();
        }
        #endregion
        #region Открыть список пар
        public ICommand OpenPairsListCommand { get; }
        private bool CanOpenPairsListCommandExcecut(object p) => true;
        private async void OnOpenPairsListCommandExecuted(object p)
        {
            object result = await DialogHost.Show(new CoinsListView(), "PairsListDialog");
            if (result == null)
            {
                UpdateDataList();
            }
        }


        #endregion
        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExcecut(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
             Application.Current.Shutdown();
        }
        #endregion

        #endregion

        private void UpdateDataList()
        {
            coins.Clear();

            foreach (Pair pair in DataBase.GetAllPair())
            {
                coins.Add(new CoinView(pair.Title));
            }

            Content = coins;
        }


        public MainViewModel()
        {
            #region Команды

            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExcecut);
            SocketInitCommand = new RelayCommand(OnSocketInitCommandExecuted, CanSocketInitCommandExcecut);
            OpenPairsListCommand = new RelayCommand(OnOpenPairsListCommandExecuted, CanOpenPairsListCommandExcecut);

            DataBase.Create();
            DataBase.AddMainCoins();
            UpdateDataList();

            Socket.UpdateStatus += SocketSubscription;

            #endregion
        }

        private void SocketSubscription(object sender, System.EventArgs e)
        {
            Status = Socket.Status;
        }
    }
}
