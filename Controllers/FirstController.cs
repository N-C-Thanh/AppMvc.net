using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using EX01.Models;
using EX01.Services;

namespace EX01.Controllers;

// Controller------------------------------------------------------------
public class FirstController(
    ILogger<FirstController> logger,
    IWebHostEnvironment webHostEnvironment,
    ProductService ProductService) : Controller
{
    private readonly ILogger<FirstController> _logger = logger;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly ProductService _ProductService = ProductService;

    //Action--------------------------------------------------------------
    public string Index()
    {
        // this.HttpContext
        // this.Request
        // this.Response 
        // this.RouteData

        // this.User
        // this.ModelState
        // this.ViewData
        // this.ViewBag
        // this.TempData

        _logger.LogInformation("Index Action"); // Tương tự Console.WriteLine("Index Action");

        _logger.LogWarning("Alert1");
        _logger.LogError("Alert2");
        _logger.LogDebug("Alert3");
        _logger.LogCritical("Alert4");
        _logger.LogInformation("Alert5");

        return "Tôi là Index của First";
    }

    public void Nothing()
    {
        _logger.LogInformation("Nothing Ation");
        Response.Headers.Append("hi", "Xin chao cac ban");
    }

    public object Anything()
    {
        return new int[] { 1, 2, 3 };
        // DateTime.Now;
        // Math.Sqrt(2) 
    }

    // IActionResult:----------------------------------------------------------------
    public IActionResult GetContent()
    {
        var content = @"
        Chao cac ban,
        Toi la Thanh


        Cam on.
        ";
        return Content(content, "text/plain", System.Text.Encoding.UTF8);
    }

    public IActionResult FileAnh()
    {
        string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Files", "heart.jpg");
        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "image/jpeg");
    }

    public IActionResult GetJson()
    {
        return Json(
            new
            {
                key1 = 100,
                key2 = new string[] { "a", "b", "c" }
            }
        );
    }

    public IActionResult Redirect()
    {
        var url = Url.Action("Privacy", "Home");

        _logger.LogInformation("Chuyen huong den: " + url);
        return LocalRedirect(url ?? "/");
    }

    public IActionResult Google()
    {
        var url = "https://www.google.com/";
        _logger.LogInformation("Chuyen huong den: " + url);
        return Redirect(url);
    }

    // View:---------------------------------------------------------------------------
    public IActionResult HelloView(string username)
    {
        if (string.IsNullOrEmpty(username))
            username = "Khach";

        // View() -> Razor Engine, doc .cshtml (template)
        /* TH1: View(template)--------------------------------------------
        // template = AbsolutePath.cshtml
        return View("/MyView/XinChao1.cshtml");
        */
        /* TH2.1: View(template, model)-----------------------------------
        // template = AbsolutePath.cshtml
        // model = username
        return View("/MyView/XinChao1.cshtml", username);
        */
        /* TH2.2:--------------------------------------------------------
        // template = xinchao2.cshtml -> /View/First/xinchao2.cshtml
        // model = username
        return View("xinchao2", username);
        */
        /* TH2.3: File.cshtml khác thư mục mặc định ---------------------
        // template = xinchao3.cshtml -> /MyView/First/xinchao3.cshtml (được cấu hình bởi RazorViewEngineOptions trong Program.cs)
        // model = username
        return View("xinchao3", username);
        */
        /* TH3: View(Model)----------------------------------------------
        // template = HelloView.cshtml = ActionName.cshtml -> /View/First/HelloView.cshtml
        // model = (object)username
        return View((object)username);
        */
        /* TH4: View()--------------------------------------------------*/
        // template = HelloView.cshtml = ActionName.cshtml -> /View/First/HelloView.cshtml
        return View();
    }

    // Truyền dữ liệu sang view:---------------------------------------------------------------
    // TempData C2:---------
    [TempData]
    public required string StatusMessage { get; set; }
    //-----------
    public IActionResult ViewProduct(int? id)
    {
        var product = _ProductService.Where(p => p.Id == id).FirstOrDefault();
        if (product == null)
        {
            /* 4.1 TempData C1:---------------
            //TempData["StatusMessage"] = "San pham ban yeu cau khong co";
            */
            // 4.2 TempData C2:---------------
            StatusMessage = "San pham ban yeu cau khong co";
            return Redirect(Url.Action("Index", "Home") ?? "/");
        }
        /* 1 Model:---------------------------    
        return View(product);
        */

        /* 2 ViewData:------------------------
        this.ViewData["product"] = product;
        ViewData["Title"] = product.Name;
        return View("ViewProduct2");
        */

        // 3 ViewBag:--------------------------
        ViewBag.product = product;
        return View("ViewProduct3");
    }
}