using SQLite;

namespace Binance.Models
{
    public class Pair
    {
        public Pair(string title)
        {
            Title = title;
        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }

        public Pair() {}
    }
}
