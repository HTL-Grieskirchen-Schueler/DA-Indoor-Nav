namespace DAServer.Dtos
{
    public class CombinedAccessPoint
    {
        public int Id { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public string MacAddress { get; set; } = "";
        public double Distance { get; set; } = 0.0;
    }
}
