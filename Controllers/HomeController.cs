using Microsoft.AspNetCore.Mvc;
using ModalWindows.Models;
using ModalWindows.Services;
using System.Diagnostics;

namespace ModalWindows.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportService _reportService;

        public HomeController(ILogger<HomeController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        // in use
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ReportFilterViewModel();
            return View(model);
        }

        // in use
        [HttpPost]
        public async Task<IActionResult> Index(ReportFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Имитируем запрос в "базу данных"
                    var reportData = await _reportService.GetReportDataAsync(model.StartDate, model.EndDate, model.SelectedWeigherId);

                    // 2. Обрабатываем результат
                    if (reportData.Any())
                    {
                        // Используем TempData
                        TempData["NotificationType"] = "success";
                        TempData["NotificationMessage"] = "Отчет успешно сформирован!";
                        ViewData["ReportData"] = reportData;
                    }
                    else
                    {
                        TempData["NotificationType"] = "warning";
                        TempData["NotificationMessage"] = "По вашему запросу данные не найдены.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["NotificationType"] = "danger";
                    TempData["NotificationMessage"] = $"Произошла ошибка: {ex.Message}";
                }
            }
            else
            {
                // Ошибка валидации модели - тоже можно показать в модальном окне
                TempData["ModalErrorMessage"] = "Пожалуйста, исправьте ошибки в форме.";
            }

            // В любом случае возвращаем то же представление, чтобы пользователь мог видеть результат
            return View(model);
        }

        // unused
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
