using Binance.ViewModels.Controls;
using System.Windows.Controls;

namespace Binance.Views.Controls
{
    public partial class CoinView : UserControl
    {
        public CoinView(string title)
        {
            InitializeComponent();
            ((CoinViewModel)DataContext).Title = title.ToUpper();
        }
    }
}
