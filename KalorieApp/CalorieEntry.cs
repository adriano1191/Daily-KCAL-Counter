using System;
using SQLite;

namespace Koryto
{
    [Table("calorie_entry")]
    public class CalorieEntry
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("calories")]
        public int Calories { get; set; }
        [Column("water")]
        public double Water { get; set; }

        [Column("meal_type")]
        public string MealType { get; set; } = "Inny";

        [Column("note")]
        public string Note { get; set; } = string.Empty;

        [Column("entry_time")]
        public DateTime EntryTime { get; set; } = DateTime.Now;
    }
}