using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Models
{
    public class Coin
    {
        public Coin(string title, double currentQuantity, double currentPrice)
        {
            HisroryQueue = new Queue<double>();

            Title = title;
            CurrentQuantity = currentQuantity;
            HisroryQueue.Enqueue(currentQuantity);
            CurrentPrice = currentPrice;
            Percent = 0;
        }
        public string Title { get; set; }
        public double CurrentQuantity { get; set; }
        public double CurrentPrice { get; set; }
        public double Percent { get; set; }
        public Queue<double> HisroryQueue { get; set; }
        public double FirstQuantity
        {
            get
            {
                return HisroryQueue.Peek();
            }
        }

        public async void UpdateCoinAsyc()
        {
            await Task.Run(() =>
            {
                // Убраем лишние события вне диапазона
                if (HisroryQueue.Count > AppVar.TolalPeriod)
                {
                    HisroryQueue.Dequeue();
                }
                // Если событий больше, чем период расчета среднего процента
                if (HisroryQueue.Count > AppVar.MeanPeriod)
                {
                    double[] data = new double[AppVar.MeanPeriod];

                    int firstIndex = HisroryQueue.Count - AppVar.MeanPeriod;
                    int count = 0;
                    for (int i = firstIndex;
                          i < HisroryQueue.Count; i++)
                    {
                        data[count++] = HisroryQueue.ToArray()[i];
                    }

                    // HACK : Если массив заполнен не полностью, вылетит эксепшн при Average
                    try
                    {
                        Percent = 100 / FirstQuantity * data.Average() - 100;
                    }
                    catch { }
                }
            });
        }

    }
}
