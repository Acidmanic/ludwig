namespace Ludwig.Contracts.Models
{
    public class ExportData
    {
        public string FileName { get; set; }
        
        public string MediaType { get; set; }
        
        public byte[] Data { get; set; }
    }
}