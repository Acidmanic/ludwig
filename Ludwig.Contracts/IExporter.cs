using System.Threading.Tasks;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts
{
    public interface IExporter
    {
        
        ExportInformation Id { get; }

        Task<ExportData> ProvideExport();
        
    }
}