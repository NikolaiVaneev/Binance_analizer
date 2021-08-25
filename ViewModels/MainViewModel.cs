using Binance.Data;
using Binance.Infrastructure.Commands;
using Binance.Services;
using Binance.Views.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Binance.Models;
using System.Windows.Input;
using Binance.Views.Pages;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

using System.Linq;
using System;
using Binance.ViewModels.Controls;

namespace Binance.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        #region Свойства
        private readonly ObservableCollection<CoinView> contentRecession = new ObservableCollection<CoinView>();
        private readonly ObservableCollection<CoinView> contentRise = new ObservableCollection<CoinView>();

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

        #region Главное содержимое (растущее)
        private ObservableCollection<CoinView> _contentRise;
        /// <summary>Главное содержимое</summary>
        public ObservableCollection<CoinView> ContentRise
        {
            get => _contentRise;
            set => SetProperty(ref _contentRise, value);
        }
        #endregion

        #region Главное содержимое (падающее)
        private ObservableCollection<CoinView> _contentRecession;
        /// <summary>Главное содержимое</summary>
        public ObservableCollection<CoinView> ContentRecession
        {
            get => _contentRecession;
            set => SetProperty(ref _contentRecession, value);
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
                InitDataLists();
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

        private void InitDataLists()
        {
            contentRecession.Clear();
            contentRise.Clear();

            foreach (Pair pair in DataBase.GetAllPair())
            {
                contentRecession.Add(new CoinView(pair.Title, CoinView.TypeControlEnum.Recession));
                contentRise.Add(new CoinView(pair.Title, CoinView.TypeControlEnum.Rise));
            }

            ContentRecession = contentRecession;
            ContentRise = contentRise;
        }


        private ObservableCollection<CoinView> CollectionsSort(ObservableCollection<CoinView> coinViews, bool sortABS = true)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {

                // HACK : Пришлось писать сортировку. Сделать асинхроном лучше
                CoinView[] sortedItemsList = coinViews.ToArray();
                // Сортировка массива
                CoinView temp;
                for (int i = 0; i < sortedItemsList.Length - 1; i++)
                {
                    for (int j = i + 1; j < sortedItemsList.Length; j++)
                    {
                        double val1 = double.Parse(sortedItemsList[i].TB_Percent.Text.Replace("%", string.Empty).Replace('.', ','));
                        double val2 = double.Parse(sortedItemsList[j].TB_Percent.Text.Replace("%", string.Empty).Replace('.', ','));
                        if (sortABS)
                        {
                            if (val1 > val2) // operator
                            {
                                temp = sortedItemsList[i];
                                sortedItemsList[i] = sortedItemsList[j];
                                sortedItemsList[j] = temp;
                            }
                        }
                        else
                        {
                            if (val1 < val2) // operator
                            {
                                temp = sortedItemsList[i];
                                sortedItemsList[i] = sortedItemsList[j];
                                sortedItemsList[j] = temp;
                            }
                        }
                    }
                }
                // Установка индексов
                for (int i = 0; i < sortedItemsList.Length; i++)
                {
                    int idx = coinViews.IndexOf(sortedItemsList[i]);
                    if (idx != i)
                    {
                        coinViews.Move(idx, i);
                    }
                }

            });
            return coinViews;
        }


        public MainViewModel()
        {
            #region Команды

            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExcecut);
            SocketInitCommand = new RelayCommand(OnSocketInitCommandExecuted, CanSocketInitCommandExcecut);
            OpenPairsListCommand = new RelayCommand(OnOpenPairsListCommandExecuted, CanOpenPairsListCommandExcecut);

            DataBase.Create();
            DataBase.AddMainCoins();
            InitDataLists();

            Socket.UpdateStatus += SocketSubscription;
            Socket.UpdateCoinsCollection += Socket_UpdateCoinsCollection;
            #endregion
        }

        private void Socket_UpdateCoinsCollection()
        {
            ContentRise = CollectionsSort(ContentRise, false);
            ContentRecession = CollectionsSort(ContentRecession);
        }

        private void SocketSubscription(object sender, EventArgs e)
        {
            Status = Socket.Status;
        }
    }
}
