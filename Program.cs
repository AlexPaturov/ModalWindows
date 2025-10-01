using ModalWindows.Services;

namespace ModalWindows;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        // 2. Добавляем сервисы для Blazor Server
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddScoped<IReportService, FakeReportService>();
        builder.Services.AddSingleton<ModalService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
