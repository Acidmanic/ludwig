using System.Threading.Tasks;
using Ludwig.Contracts;
using Ludwig.Contracts.Models;

namespace Ludwig.IssueManager.Fake
{
    public class FakeExporter : IExporter
    {
        public ExportInformation Id { get; } = new ExportInformation
        {
            DisplayName = "Fake Data Exporter",
            UniqueKey = "304ba47e-7a52-11ed-a76a-eb5ed246104a"
        };

        public Task<ExportData> ProvideExport()
        {
            return Task.Run(() => new ExportData
            {
                Data = System.Text.Encoding.Default.GetBytes("This is a fake exported text file."),
                FileName = "fake-info.txt",
                MediaType = "application/text"
            });
        }
    }
}