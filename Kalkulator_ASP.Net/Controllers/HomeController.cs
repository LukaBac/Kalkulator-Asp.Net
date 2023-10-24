using Kalkulator_ASP.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Kalkulator_ASP.Net.Libraries.CalculatorLib;
using Kalkulator_ASP.Net.Libraries.DatabaseLibrary;

namespace Kalkulator_ASP.Net.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Database.CreateOrOpenDatabase();
            return View(new HomeModel());
        }

        

        [HttpPost]
        public IActionResult Index(HomeModel calc)
        {
            string btn = Request.Form["CalcButton"];

            string deleteID = Request.Form["DeleteEntry"];

            //za brisanje podataka iz baze
            if(deleteID != null)
            {
                Database.DeleteEntry(Convert.ToInt32(deleteID));
                Database.ReadData(calc);
            }

            //svo handleanje inputa je ovdje
            CalculatorInput.registerInput(btn, calc);

            //refreshanje liste za prikazivanje baze
            if ((btn == "btn_history" || btn == "btn_modeChange") && calc.isHistory == "yes") {
                Database.ReadData(calc);
            }


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