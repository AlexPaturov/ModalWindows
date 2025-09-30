
namespace ModalWindows.Services;

public class FakeReportService : IReportService
{
    public async Task<List<string>> GetReportDataAsync(DateTime startDate, DateTime endDate, int weigherId)
    {
        // Имитируем асинхронную операцию (задержка 1 секунда)
        await Task.Delay(1000);

        // --- ИМИТАЦИЯ РАЗЛИЧНЫХ СЦЕНАРИЕВ ---

        // 1. Имитация ошибки базы данных
        if (weigherId == 10)
        {
            throw new Exception("Не удалось подключиться к базе данных. Попробуйте позже.");
        }

        // 2. Имитация отсутствия данных
        if (weigherId == 9)
        {
            return new List<string>(); // Возвращаем пустой список
        }

        // 3. Имитация успешного получения данных
        var reportLines = new List<string>
        {
            $"Отчет для весов №{weigherId} за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}",
            $"Строка данных 1: {Random.Shared.Next(100, 200)}",
            $"Строка данных 2: {Random.Shared.Next(200, 300)}",
            $"Строка данных 3: {Random.Shared.Next(300, 400)}"
        };

        return reportLines;
    }
}
