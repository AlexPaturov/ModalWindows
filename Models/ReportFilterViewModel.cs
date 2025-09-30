using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ModalWindows.Models;

public class ReportFilterViewModel
{
    [Display(Name = "Дата начала")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }

    [Display(Name = "Дата окончания")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Номер весов")]
    public int SelectedWeigherId { get; set; }

    /// <summary>
    /// Список для рендеринга выпадающего списка.
    /// SelectListItem - это стандартный класс MVC для этого.
    /// </summary>
    public List<SelectListItem> AvailableWeighers { get; set; }

    public ReportFilterViewModel()
    {
        // Устанавливаем значения по умолчанию
        StartDate = DateTime.Today;
        EndDate = DateTime.Today;

        AvailableWeighers = new List<SelectListItem>();
        for (int i = 1; i <= 10; i++)
        {
            AvailableWeighers.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = $"Весы №{i}"
            });
        }
    }
}

public class WeigherSelectItem
{
    public int Id { get; set; }
    public string DisplayName => $"Весы №{Id}";
    public bool IsSelected { get; set; }
}
