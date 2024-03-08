using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers
{
    public class XArticlesController : Controller
    {
        private readonly IArticleBLL _articleBLL;

        public XArticlesController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL;
        }

        public IActionResult Index(int categoryId)
        {
            ViewData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Select Category with id {categoryId} Success !</div>";
            return View();
        }
    }
}
