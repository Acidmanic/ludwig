using Ludwig.Contracts.Models;

namespace Ludwig.Contracts
{
    public interface IExporter
    {
        
        ExportInformation Id { get; }

        ExportData ProvideExport();
        
    }
}