using Binance.ViewModels.Controls;
using System.Windows.Controls;

namespace Binance.Views.Controls
{
    public partial class CoinView : UserControl
    {
        public enum TypeControlEnum
        {
            Recession,
            Rise
        }
        public CoinView(string title, TypeControlEnum typeControl)
        {
            InitializeComponent();
            ((CoinViewModel)DataContext).Title = title.ToUpper();
            ((CoinViewModel)DataContext).TypeControl = (CoinViewModel.TypeControlEnum)typeControl;
        }
    }
}
