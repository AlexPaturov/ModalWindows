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
                    // 1. ��������� ������ � "���� ������"
                    var reportData = await _reportService.GetReportDataAsync(model.StartDate, model.EndDate, model.SelectedWeigherId);

                    // 2. ������������ ���������
                    if (reportData.Any())
                    {
                        // ���������� TempData
                        TempData["NotificationType"] = "success";
                        TempData["NotificationMessage"] = "����� ������� �����������!";
                        ViewData["ReportData"] = reportData;
                    }
                    else
                    {
                        TempData["NotificationType"] = "warning";
                        TempData["NotificationMessage"] = "�� ������ ������� ������ �� �������.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["NotificationType"] = "danger";
                    TempData["NotificationMessage"] = $"��������� ������: {ex.Message}";
                }
            }
            else
            {
                // ������ ��������� ������ - ���� ����� �������� � ��������� ����
                TempData["ModalErrorMessage"] = "����������, ��������� ������ � �����.";
            }

            // � ����� ������ ���������� �� �� �������������, ����� ������������ ��� ������ ���������
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
