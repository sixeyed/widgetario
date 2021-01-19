namespace Widgetario.Web.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get;  set; }

        public string DisplayPrice
        {
            get
            {
                if(Stock > 0)
                {
                    return $"${Price.ToString("#.00")}";
                }
                else
                {
                    return "SOLD OUT!";
                }
            }
        }
    }
}
