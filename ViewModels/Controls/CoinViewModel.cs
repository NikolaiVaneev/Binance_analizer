using Binance.Data;
using Binance.Models;
using Binance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Binance.ViewModels.Controls
{
    class CoinViewModel : ViewModel
    {
        #region Наименование пары
        private string _title;
        /// <summary>Наименование пары</summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        #endregion

        #region Процент
        private double _percent;
        /// <summary>Процент</summary>
        public double Percent
        {
            get => _percent;
            set => SetProperty(ref _percent, value);
        }
        #endregion

        #region Процент (цвет)
        private Brush _percentColor = Brushes.Gray;
        /// <summary>Процент (цвет)</summary>
        public Brush PercentColor
        {
            get => _percentColor;
            set => SetProperty(ref _percentColor, value);
        }
        #endregion

        #region Заполнение стека
        private int _stack;
        /// <summary>Заполнения стека</summary>
        public int StackComplete
        {
            get => _stack;
            set => SetProperty(ref _stack, value);
        }
        #endregion

        private int _maxStack = 100;
        /// <summary>
        /// Максимальная величина стека
        /// </summary>
        public int MaxStack
        {
            get => _maxStack;
            set => SetProperty(ref _maxStack, value);
        }

        public CoinViewModel()
        {
            Socket.UpdateCoinsCollection += Socket_UpdateCoinsCollection;
        }

        private void Socket_UpdateCoinsCollection()
        {
            
            Coin pair = Socket.Coins.FirstOrDefault(c => c.Title == Title);
            if (pair != null)
            {
                StackComplete = pair.HisroryQueue.Count;
                MaxStack = AppVar.TolalPeriod;

                Percent = pair.Percent;
                if (Percent > AppVar.AlertPercent)
                {
                    PercentColor = Brushes.Green;
                }
                else if
                (Percent < -AppVar.AlertPercent)
                {
                    PercentColor = Brushes.Red;
                }
                else
                {
                    PercentColor = Brushes.Gray;
                }
            }

        }
    }
}
