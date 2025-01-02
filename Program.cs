using Microsoft.AspNetCore.Mvc.Razor;
using EX01.Services;

var builder = WebApplication.CreateBuilder(args);

// Services:-----------------------------------------------------------------------------------------------------------------------
builder.Services.AddControllersWithViews(); // Các dịch vụ của MVC
builder.Services.AddRazorPages(); // Dịch vụ của trang Razor
/*
builder.Services.AddTransient(typeof(ILogger<>), typeof(Logger<>)); 
// Dịch vụ Logger đã đặc mặc định Add vào
// Có thể thay thế dịch vụ log của bên thứ 3 như Serilog 
*/

builder.Services.Configure<RazorViewEngineOptions>(option => {
    // /View/Controller/Action.cshtml (Mặc định)
    /* /MyView/Controller/Action.cshtml (Nếu không có trong View thì tìm trong MyView)
        {0} - ten Action
        {1} - ten Controller
        RazorViewEngine.ViewExtension - .cshtml
    */
    option.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);
});
/* 
AddSingleton(tham so 1, tham so 2):
Tham số 1: ServiceType (kiểu dịch vụ).
Tham số 2: ImplementationType (kiểu thực thi dịch vụ).
Có 4 cách Add Service:
    1. builder.Services.AddSingleton<ProductService>(); // không có ServiceType
    2. builder.Services.AddSingleton<ProductService, ProductService>(); // có ServiceType
    3. builder.Services.AddSingleton(typeof (ProductService));
    4. builder.Services.AddSingleton(typeof (ProductService), typeof (ProductService));
*/
builder.Services.AddSingleton(typeof (ProductService), typeof (ProductService));

//Middleware pipeline:------------------------------------------------------------------------------------------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment()) // Configure the HTTP request pipeline.
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); // Thông báo chỉ truy cập bằng https
}

app.UseHttpsRedirection(); // Chuyển hướng http sang https
app.UseStaticFiles(); // Truy cập các file tĩnh trong wwwroot

app.UseRouting();

app.UseAuthentication(); // Xác định danh tính
app.UseAuthorization(); // Xác thực quyền truy cập

//Route:----------------------------------------------------------------------------------------------------------------------------
app.MapControllerRoute( // Tạo ánh xạ cho có url theo mô hình MVC
    name: "default",
    // URL: /controller/action/id
    // Ex: Home/Index => controller = Home, goi method = Index
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*
app.MapControllerRoute(
        name: "myroute1",
        defaults: new {controller="Home", action = "Index"},
        pattern: "{title}-{id}.html");
*/
app.MapControllerRoute (
    name: "myroute3", // đặt tên route
    defaults : new { controller = "Home", action = "Index" },
    pattern: "{title:alpha:maxlength(8)}-{id:int}.html"); // title chỉ chứa các chữ cái, dài tối đa 8, id là số nguyên
 
//--------------------------------------------------------------------------------------------------------------------------------

app.MapRazorPages(); // Truy cập đến các trang Razor

/*
app.Use(async (context, next) => // Debug routing: kiểm tra các route đã được ánh xạ và URL hiện tại
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next.Invoke();
});
*/
app.Run();