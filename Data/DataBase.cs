using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Text;
using System.Threading.Tasks;
using Binance.Models;
using System.IO;

namespace Binance.Data
{
    public static class DataBase
    {
        public static string DataBasePath { get; private set; } = "DataBase.db";

        // Добавить в БД основные пары.
        public static void AddMainCoins()
        {
            DataBase.AddPair(new Pair("1INCHUSDT"));
            DataBase.AddPair(new Pair("AAVEUSDT"));
            DataBase.AddPair(new Pair("ADAUSDT"));
            DataBase.AddPair(new Pair("ALICEUSDT"));
            DataBase.AddPair(new Pair("ATOMUSDT"));
            DataBase.AddPair(new Pair("AVAXUSDT"));
            DataBase.AddPair(new Pair("AXSUSDT"));
            DataBase.AddPair(new Pair("BTCUSDT"));
            DataBase.AddPair(new Pair("COMPUSDT"));
            DataBase.AddPair(new Pair("DASHUSDT"));
            DataBase.AddPair(new Pair("DentUSDT"));
            DataBase.AddPair(new Pair("DodoUSDT"));
            DataBase.AddPair(new Pair("DogeUSDT"));
            DataBase.AddPair(new Pair("DotUSDT"));
            DataBase.AddPair(new Pair("EnjUSDT"));
            DataBase.AddPair(new Pair("EosUSDT"));
            DataBase.AddPair(new Pair("ETHUSDT"));
            DataBase.AddPair(new Pair("GTCUSDT"));
            DataBase.AddPair(new Pair("IcpUSDT"));
            DataBase.AddPair(new Pair("KAVAUSDT"));
            DataBase.AddPair(new Pair("LINAUSDT"));
            DataBase.AddPair(new Pair("LINKUSDT"));
            DataBase.AddPair(new Pair("LTCUSDT"));
            DataBase.AddPair(new Pair("LUNAUSDT"));
            DataBase.AddPair(new Pair("MATICUSDT"));
            DataBase.AddPair(new Pair("ONTUSDT"));
            DataBase.AddPair(new Pair("QTUMUSDT"));
            DataBase.AddPair(new Pair("REEFUSDT"));
            DataBase.AddPair(new Pair("RUNEUSDT"));
            DataBase.AddPair(new Pair("SOLUSDT"));
            DataBase.AddPair(new Pair("SUSHIUSDT"));
            DataBase.AddPair(new Pair("SXPUSDT"));
            DataBase.AddPair(new Pair("UNIUSDT"));
            DataBase.AddPair(new Pair("XRPUSDT"));
            DataBase.AddPair(new Pair("YFIUSDT"));
        }
        public static void Create()
        {
            if (!File.Exists(DataBasePath))
            {
                using (SQLiteConnection connection = new SQLiteConnection(DataBasePath))
                {
                    connection.CreateTable<Pair>();
                }
            };
        }
        public static void AddPair(Pair pair)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataBasePath))
            {
                Pair item = connection.Query<Pair>($"SELECT * FROM Pair WHERE Title = \"{pair.Title}\"").FirstOrDefault();
                if (item == null)
                {
                    connection.Insert(pair);
                }
            }
        }
        public static void DeletePair(Pair pair)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataBasePath))
            {
                connection.Delete(pair);
            }
        }
        public static List<Pair> GetAllPair()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataBasePath))
            {
                return connection.Query<Pair>("SELECT * FROM Pair ORDER BY TItle ASC");
            }
        }
    }
}
