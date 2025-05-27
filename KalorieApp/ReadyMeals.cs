using SQLite;

namespace Koryto
{
    [Table("ready_meals")]
    public class ReadyMeals
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "Name";

        [Column("calories")]
        public double  Calories { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("icon_path")]
        public string IconPath { get; set; } = string.Empty;

    }
}
