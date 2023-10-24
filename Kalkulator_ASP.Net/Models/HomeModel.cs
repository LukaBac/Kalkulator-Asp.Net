namespace Kalkulator_ASP.Net.Models
{
    public class HomeModel
    {
        public string Racun1 { get; set; } = "";
        public string Racun2 { get; set; } = "";
        public string Racun3 { get; set; } = "";
        public string Zadnjigumb { get; set; } = "";
        public double Mem { get; set; }
        public string IsScientific { get; set; } = "no";
        public bool test { get; set; }
        public string ScientificMode { get; set; } = "rad";

        public string isHistory { get; set; } = "no";

        public List<DatabaseModel> Database = new List<DatabaseModel>();
    }
}
