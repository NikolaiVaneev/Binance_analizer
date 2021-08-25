using Binance.Models;
using System;
using System.Windows.Controls;

namespace Binance.Views.Controls
{
    public partial class CoinItemView : UserControl
    {
        public event EventHandler DeleteItem;
        public Pair Pair { get; set; }
        public CoinItemView(Pair pair)
        {
            InitializeComponent();
            Pair = pair;
            TB_Title.Text = pair.Title;
        }

        private void BTN_Del_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItem?.Invoke(this, null);
        }
    }
}
