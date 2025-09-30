namespace ModalWindows.Services;

public interface IReportService
{
    Task<List<string>> GetReportDataAsync(DateTime startDate, DateTime endDate, int weigherId);
}
