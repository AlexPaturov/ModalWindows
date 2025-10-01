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
                    // 1. ��������� ������ � "���� ������"
                    var reportData = await _reportService.GetReportDataAsync(model.StartDate, model.EndDate, model.SelectedWeigherId);

                    // 2. ������������ ���������
                    if (reportData.Any())
                    {
                        // === �������� 1: ������ ������� �������� ===
                        // �����: �������� ��������� ��� ���������� ����
                        // ViewData["ModalSuccessMessage"] = "����� ������� �����������!";
                        _modalService.Show("����� ������� �����������!", NotificationType.Success);
                        ViewData["ReportData"] = reportData;
                    }
                    else
                    {
                        // === �������� 2: ������ �� ������� ===
                        // ��� ������: �������� ��������� ��� ���������� ����
                        // ViewData["ModalWarningMessage"] = "�� ������ ������� ������ �� �������.";
                        _modalService.Show("�� ������ ������� ������ �� �������.", NotificationType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // === �������� 3: ��������� ������ ===
                    _logger.LogError(ex, "������ ��� ������������ ������");
                    // ViewData["ModalErrorMessage"] = $"��������� ������: {ex.Message}";
                    _modalService.Show($"��������� ������: {ex.Message}", NotificationType.Error);
                }
            }
            else 
            {
                // ������ ��������� ������ - ���� ����� �������� � ��������� ����
                // ViewData["ModalErrorMessage"] = "����������, ��������� ������ � �����.";
                _modalService.Show("����������, ��������� ������ � �����.", NotificationType.Error);
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
