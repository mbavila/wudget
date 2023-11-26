using BaseCodeDotNetCore.Domain;
using BaseCodeDotNetCore.Domain.Budgets;
using BaseCodeDotNetCore.Domain.Budgets.ViewModels;
using BaseCodeDotNetCore.Utils.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IBudgetService budgetService;

        public DashboardController(ILogger<DashboardController> logger, IBudgetService budgetService)
        {
            this.logger = logger;
            this.budgetService = budgetService;
        }

        [HttpGet(Name = "gettotalvalues")]
        public IActionResult GetBudget([FromQuery] BudgetSearchViewModel search)
        {
            var returnList = budgetService.GetDashboardDetails(search);
            return CreatedAtRoute("gettotalvalues", new ResponseModel(0, returnList, string.Empty));
        }

        [HttpPost(Name = "testlang")]
        public IActionResult TestLang([FromBody] BudgetViewModel budget)
        {
            return CreatedAtRoute("testlang", new ResponseModel(1, string.Empty, "Hello World June ASI Newbies"));
        }
    }
}
