namespace project_kaupskra.Helpers
{
    public class QueryObject
    {
        public string[]? ListofTowns { get; set; } = null;
        public int[]? ListofPostcodes { get; set; } = null;
        
        public string[]? ListOfHousing { get; set; } = null;
        public DateTime? Date1 { get; set; } = null;
        public DateTime? Date2 { get; set; } = null;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }

}
