using Binance.Models;
using Binance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WebSocketSharp;

namespace Binance.Data
{
    internal static class Socket
    {
        public static event EventHandler UpdateStatus;

        private static readonly string url = "wss://stream.binance.com:9443/ws/!miniTicker@arr";
        private static WebSocket ws;

        public static event Action UpdateCoinsCollection;

        /// <summary>
        /// Коллекция пар
        /// </summary>
        public static List<Coin> Coins { get; set; }
        public static string Status { get; set; }
        public async static void InitAsync()
        {
            await Task.Run(() =>
            {
                ws = new WebSocket(url);
                ws.OnMessage += Ws_OnMessage;
                ws.OnOpen += Ws_OnOpen;
                ws.OnClose += Ws_OnClose;
                ws.OnError += Ws_OnError;

                ws.Connect();
                Coins = new List<Coin>();

            });
        }
        private static void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Status = "Ошибка связи";
            UpdateStatus?.Invoke(sender, null);
        }
        private static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Status = "Сокет отключен";
            UpdateStatus?.Invoke(sender, null);
        }
        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            Status = "Сокет подключен";
            UpdateStatus?.Invoke(sender, null);
        }
        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic coinJSON = jss.Deserialize<dynamic>(e.Data);

            foreach (dynamic coin in coinJSON)
            {
                string title = coin["s"];
                double currentQuantity = double.Parse(coin["q"].Replace('.', ','));
                double currentPrice = double.Parse(coin["c"].Replace('.', ','));

                Coin coinInCollection = Coins.FirstOrDefault(c => c.Title == title);

                if (coinInCollection == null)
                {
                    Coins.Add(new Coin(title, currentQuantity, currentPrice));
                }
                else
                {
                    coinInCollection.HisroryQueue.Enqueue(currentQuantity);
                    coinInCollection.CurrentPrice = currentPrice;
                    coinInCollection.CurrentQuantity = currentQuantity;
                    coinInCollection.UpdateCoinAsyc();
                }
            }
            UpdateCoinsCollection?.Invoke();
        }


    }
}
