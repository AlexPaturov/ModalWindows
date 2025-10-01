using Microsoft.AspNetCore.Mvc;
using ModalWindows.Enums;
using ModalWindows.Models;
using ModalWindows.Services;
using System.Diagnostics;

namespace ModalWindows.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportService _reportService;
        private readonly ModalService _modalService;

        public HomeController(
            ILogger<HomeController> logger, 
            IReportService reportService,
            ModalService modalService)
        {
            _logger = logger;
            _reportService = reportService;
            _modalService = modalService;
        }

        // in use
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ReportFilterViewModel();

            // Проверяем, есть ли данные отчета после редиректа
            if (TempData["ReportDataJson"] is string json)
            {
                ViewData["ReportData"] = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
            }

            // Проверяем, есть ли уведомление для модального окна
            if (TempData["NotificationMessage"] != null && TempData["NotificationType"] != null)
            {
                ViewData["ModalMessage"] = TempData["NotificationMessage"];
                ViewData["ModalType"] = TempData["NotificationType"];
            }

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
                    var reportData = await _reportService.GetReportDataAsync(model.StartDate, model.EndDate, model.SelectedWeigherId);

                    if (reportData.Any())
                    {
                        TempData["NotificationType"] = NotificationType.Success.ToString();
                        TempData["NotificationMessage"] = "Отчет успешно сформирован!";
                        // Сохраняем данные отчета тоже в TempData, чтобы они были доступны после редиректа
                        TempData["ReportDataJson"] = System.Text.Json.JsonSerializer.Serialize(reportData);
                    }
                    else
                    {
                        TempData["NotificationType"] = NotificationType.Warning.ToString();
                        TempData["NotificationMessage"] = "По вашему запросу данные не найдены.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["NotificationType"] = NotificationType.Danger.ToString(); // Используем 'danger' для bootstrap
                    TempData["NotificationMessage"] = $"Произошла ошибка: {ex.Message}";
                }
            }

            // ВАЖНО: Делаем редирект на GET-метод Index
            return RedirectToAction(nameof(Index));
        }

        // unused
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
