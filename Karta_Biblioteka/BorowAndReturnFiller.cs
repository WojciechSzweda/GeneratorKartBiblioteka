using Karta_Biblioteka;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BorowAndReturnFiller { 
    private class BookCopy
    {

        static Random random = new Random();
        public int ID { get; set; }
        public DateTime CurreentDate { get; set; }
        public int State { get; set; }
        public void randomBookDamage()
        {

            if (State == 4) return;

            int damageValue;
            if (State == 3)
                damageValue = 1;
            else damageValue = random.Next(1, 3);


            var r = random.NextDouble();

            if (r > 0.95) State += damageValue;
            else
             if (r > 0.2) State += (damageValue - 1);

        }
    }
    private class Card {
        public int ID { get; set; }
        public DateTime creationDate { get; set; }
    }


    private SqlConnection con;
    private Random random = new Random();

    private int randomInterval = 40;

    private int[] getIdList(string tableName)
    {
        string sql = String.Format("SELECT ID FROM {0}", tableName);
        var command = new SqlCommand(sql, con);
        List<int> ids = new List<int>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())

                ids.Add(reader.GetInt32(0));
        }
        return ids.ToArray();
    }

    private Card[] getCards() {
        var command = new SqlCommand("SELECT ID,[Data wydania]  FROM Karta",con);
        List<Card> cards = new List<Card>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {

                var a = reader["Data wydania"];
                var s = a.ToString();
                cards.Add(new Card()
                {
                    ID = reader.GetInt32(0),
                    creationDate = (DateTime)reader["Data wydania"]

                });
            }
        }
        return cards.ToArray();
    }
 


    private string sqlDateFormat(DateTime time)
    {
        return time.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private DateTime AddRandomNumberOfTime(DateTime startDate, int maxNumberOdDays)
    {
        startDate = startDate.Date + new TimeSpan(9 + random.Next(8), random.Next(60), 0);
        return startDate.AddDays(random.Next(1, maxNumberOdDays));
    }

    public void fillBorwosAndReturns(int recordNumber)
    {

        DBHelper.DeleteTable("Oddanie",con);
        DBHelper.DeleteTable("Wypożyczenie",con);

        int[] cardIds = getIdList("Karta");
        var cards = getCards();
        Func<Card> randomCar = () => cards[random.Next(cards.Length)];

        var copys = getIdList("Kopia").Select(id => new BookCopy()
        {
            ID = id,
            CurreentDate = DateTime.MinValue,
            State = random.Next(1, 3)
        }).ToList();
        Func<BookCopy> randomCopy = () => copys[random.Next(copys.Count())];

        for (int i = 0; i < recordNumber; i++)
        {
            var copy = randomCopy();
            int borowId = insertBorow(copy, randomCar());
            insertReturn(copy, borowId);

        }

        copys = copys.OrderBy(x => Guid.NewGuid()).Take(copys.Count() / 2).ToList();
        foreach (var copy in copys)
        {
            int borowId = insertBorow(copy, randomCar());
        }
    }

    private void insertReturn(BookCopy copy, int borowId)
    {
        SqlCommand returnInsert = new SqlCommand(@"INSERT INTO Oddanie 
                ([Data oddania],[Stan przed oddaniem],[stan po oddaniu],ID_wypożyczenie) 
                VALUES (@returnDate,@stateBefore,@stateAfter,@borowId)", con);

        var returnDate = AddRandomNumberOfTime(copy.CurreentDate, randomInterval);
        copy.CurreentDate = returnDate;
        returnInsert.Parameters.AddWithValue("@returnDate", sqlDateFormat(returnDate));
        returnInsert.Parameters.AddWithValue("@stateBefore", copy.State);

        copy.randomBookDamage();
        returnInsert.Parameters.AddWithValue("@stateAfter", copy.State);

        returnInsert.Parameters.AddWithValue("@borowId", borowId);

        returnInsert.ExecuteNonQuery();


    }

    private int insertBorow(BookCopy copy, Card card)
    {
        SqlCommand borowInsert = new SqlCommand(@"INSERT INTO Wypożyczenie
                (ID_Kopia,[Data wypożyczenia],[ID_Karta],[Oczekiwana data zwrotu]) OUTPUT INSERTED.ID
                VALUES (@idCopy,@borowDate,@idCard,@returnDate)", con);

        DateTime date;

        if (copy.CurreentDate > card.creationDate) {
            date = copy.CurreentDate;
        } else {
            date = card.creationDate;
        }


        var borowDate = AddRandomNumberOfTime(date, randomInterval);
        copy.CurreentDate = borowDate;

        borowInsert.Parameters.AddWithValue("@idCopy", copy.ID);
        borowInsert.Parameters.AddWithValue("@borowDate", sqlDateFormat(borowDate));
        borowInsert.Parameters.AddWithValue("@idCard", card.ID);
        borowInsert.Parameters.AddWithValue("@returnDate", sqlDateFormat(borowDate.AddDays(30)));

        return (int)borowInsert.ExecuteScalar();
    }


    public BorowAndReturnFiller(SqlConnection con)
    {
        this.con = con;
    }

}

