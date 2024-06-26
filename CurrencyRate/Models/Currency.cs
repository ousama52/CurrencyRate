namespace MyProject.Controllers.Model
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyN { get; set; } = "";
        public DateTime Exchangetime { get; set; }
        public decimal Exchangerate { get; set; }
    }
}
