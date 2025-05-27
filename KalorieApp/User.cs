using SQLite;

namespace Koryto
{
    [Table("user")]
    public class User
    {
        /// <summary>
        /// Unikalny identyfikator użytkownika.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Imię lub pseudonim użytkownika.
        /// </summary>
        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Waga użytkownika w kilogramach.
        /// </summary>
        [Column("weight")]
        public int Weight { get; set; }

        /// <summary>
        /// Wzrost użytkownika w centymetrach.
        /// </summary>
        [Column("height")]
        public int Height { get; set; }

        /// <summary>
        /// Wiek użytkownika.
        /// </summary>
        [Column("age")]
        public int Age { get; set; }

        /// <summary>
        /// Płeć użytkownika: "M" dla mężczyzny, "F" dla kobiety.
        /// </summary>
        [Column("gender")]
        public string Gender { get; set; } = "M"; // "M" dla mężczyzny, "F" dla kobiety

        /// <summary>
        /// Wzrost sformatowany do wyświetlenia w UI.
        /// </summary>
        public string FormattedHeight => $"Wzrost: {Height} cm";

        /// <summary>
        /// Waga sformatowana do wyświetlenia w UI.
        /// </summary>
        public string FormattedWeight => $"Waga: {Weight} kg";

        /// <summary>
        /// Wiek sformatowana do wyświetlenia w UI.
        /// </summary>
        public string FormattedAge => $"Wiek: {Age}";
        /// <summary>
        /// Płeć sformatowana do wyświetlenia w UI.
        /// </summary>
        public string FormattedGender => $"Płeć: {Gender}";

        /// <summary>
        /// Oblicza BMI (Body Mass Index)
        /// </summary>
        public double BMI => Height > 0 ? Math.Round(Weight / Math.Pow(Height / 100.0, 2), 2) : 0;

        /// <summary>
        /// Oblicza BMR (Basal Metabolic Rate) wg wzoru Mifflina-St Jeora
        /// </summary>
        public int BMR => Gender == "F"
            ? (int)Math.Round(10 * Weight + 6.25 * Height - 5 * Age - 161)
            : (int)Math.Round(10 * Weight + 6.25 * Height - 5 * Age + 5);

        public string FormattedBMI => $"BMI: {BMI}";
        public string FormattedBMR => $"BMR: {BMR}";

        /// <summary>
        /// Całkowite dzienne zapotrzebowanie na wodę (litry), wyliczane z masy ciała.
        /// Wzór: 0.035 L × masa ciała
        /// </summary>
        public double TotalWaterNeedLiters => Math.Round(Weight * 0.035, 2);

        /// <summary>
        /// Domyślny procent zapotrzebowania, który powinien być wypity w postaci czystej wody.
        /// Np. 60% całkowitego zapotrzebowania.
        /// </summary>
        private const double DrinkingWaterRatio = 0.6;

        /// <summary>
        /// Zalecana ilość czystej wody do wypicia dziennie (w litrach).
        /// </summary>
        public double DailyWaterToDrinkLiters => Math.Round(TotalWaterNeedLiters * DrinkingWaterRatio, 2);

        /// <summary>
        /// Formatka do UI – pełne dzienne zapotrzebowanie.
        /// </summary>
        public string FormattedWaterNeed => $"Zapotrzebowanie (całkowite): {TotalWaterNeedLiters} L";

        /// <summary>
        /// Formatka do UI – ile faktycznie wypić.
        /// </summary>
        public string FormattedWaterToDrink => $"Zalecane picie (60%): {DailyWaterToDrinkLiters} L";
    }
}
