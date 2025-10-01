using Microsoft.AspNetCore.Mvc;
using ModalWindows.Models;
using ModalWindows.Services;
using System.Diagnostics;
using static ModalWindows.Views.Shared.CustomComponents.NotificationModal;

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
                        // === СЦЕНАРИЙ 1: ДАННЫЕ УСПЕШНО ПОЛУЧЕНЫ ===
                        // УСПЕХ: Передаем сообщение для модального окна
                        // ViewData["ModalSuccessMessage"] = "Отчет успешно сформирован!";
                        _modalService.Show("Отчет успешно сформирован!", NotificationType.Success);
                        ViewData["ReportData"] = reportData;
                    }
                    else
                    {
                        // === СЦЕНАРИЙ 2: ДАННЫЕ НЕ НАЙДЕНЫ ===
                        // НЕТ ДАННЫХ: Передаем сообщение для модального окна
                        // ViewData["ModalWarningMessage"] = "По вашему запросу данные не найдены.";
                        _modalService.Show("По вашему запросу данные не найдены.", NotificationType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // === СЦЕНАРИЙ 3: ПРОИЗОШЛА ОШИБКА ===
                    _logger.LogError(ex, "Ошибка при формировании отчета");
                    // ViewData["ModalErrorMessage"] = $"Произошла ошибка: {ex.Message}";
                    _modalService.Show($"Произошла ошибка: {ex.Message}", NotificationType.Error);
                }
            }
            else 
            {
                // Ошибка валидации модели - тоже можно показать в модальном окне
                // ViewData["ModalErrorMessage"] = "Пожалуйста, исправьте ошибки в форме.";
                _modalService.Show("Пожалуйста, исправьте ошибки в форме.", NotificationType.Error);
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
