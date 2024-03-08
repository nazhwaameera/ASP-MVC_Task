using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleBLL _articleBLL;

        public ArticlesController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL;
        }

        public IActionResult Index(int categoryId)
        {
            if (TempData["message"] != null)
            {
                ViewData["message"] = TempData["message"];
            }
            if (TempData["message2"] != null)
            {
                ViewData["message2"] = TempData["message2"];
            }
            TempData["CategoryID"] = categoryId;
            var models = _articleBLL.GetArticleByCategory(categoryId);
            ViewData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Select Category with id {categoryId} Success !</div>";
            return View(models);
        }

        public IActionResult Create()
        {
            ViewData["CategoryID"] = TempData["CategoryID"];
            return View();
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO model, IFormFile Pic)
        {
            try
            {
                if(Pic != null)
                {
                    if(Helper.IsImageFile(Pic.FileName))
                    {
                        //random file name based on GUID
                        var fileName = $"{Guid.NewGuid()}_{Pic.FileName}";
                        model.Pic = fileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Pic.CopyTo(fileStream);
                        }
                        TempData["message2"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>File uploaded successfully !</div>";
                    }
                    else
                    {
                        TempData["message2"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
                    }
                }
                
                _articleBLL.Insert(model);
                TempData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Article {model.Title} has been created !</div>";
                return RedirectToAction("Index", "Articles", new { categoryId = model.CategoryID });
            }
            catch (System.Exception ex)
            {
                TempData["message"] = $@"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var models = _articleBLL.GetArticleById(id);
            return View(models);
        }

        [HttpPost]
        public IActionResult Edit(ArticleUpdateDTO model, IFormFile Pic, string filename)
        {
            try
            {
                if (Pic != null)
                {
                    if (Helper.IsImageFile(Pic.FileName))
                    {
                        //random file name based on GUID
                        var fileName = $"{Guid.NewGuid()}_{Pic.FileName}";
                        model.Pic = fileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Pic.CopyTo(fileStream);
                        }
                        TempData["message2"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>File uploaded successfully !</div>";
                    }
                    else
                    {
                        TempData["message2"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
                    }
                }
                else 
                {
                    model.Pic = filename;
                }

                _articleBLL.Update(model);
                TempData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Article {model.Title} has been updated !</div>";
                return RedirectToAction("Index", "Articles", new { categoryId = model.CategoryID });
            }
            catch (System.Exception ex)
            {
                TempData["message"] = $@"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            var models = _articleBLL.GetArticleById(id);
            return View(models);
        }

        [HttpPost]
        public IActionResult Delete(int id, int CategoryID)
        {
            try
            {
                return Content(CategoryID.ToString());
                //_articleBLL.Delete(id);
                //TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Article Success !</div>";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(id);
            }
            return RedirectToAction("Index", new { categoryId = CategoryID });

        }
    }
}
