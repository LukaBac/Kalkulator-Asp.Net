using Kalkulator_ASP.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Kalkulator_ASP.Net.CalculatorLib;
using System;
using System.Diagnostics;

namespace Kalkulator_ASP.Net.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new HomeModel());
        }

        

        [HttpPost]
        public IActionResult Index(HomeModel calc)
        {
            string btn = Request.Form["CalcButton"];

            CalculatorInput.registerInput(btn, calc);
            Console.WriteLine(btn);

            return View(calc);
        }




        #region PreGenerated
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        #endregion
    }
}