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
    public class BudgetController : Controller
    {
        private readonly ILogger<BudgetController> logger;
        private readonly IBudgetService budgetService;

        public BudgetController(ILogger<BudgetController> logger, IBudgetService budgetService)
        {
            this.logger = logger;
            this.budgetService = budgetService;
        }

        [HttpGet(Name = "budgetlistbypage")]
        public IActionResult GetBudget([FromQuery] BudgetSearchViewModel search)
        {
            var returnList = budgetService.GetBudgetPaginated(search);
            return CreatedAtRoute("budgetlistbypage", new ResponseModel(0, returnList, string.Empty));
        }

        [HttpPost(Name = "createbudget")]
        public IActionResult CreateBudget([FromBody] BudgetViewModel budget)
        {
            try
            {
                var id = budgetService.AddNewBudget(budget);
                var message = id > 0 ? Messages.SubcategoryMessages.SubCategorySaved : Messages.SubcategoryMessages.SubCategoryExist;

                return CreatedAtRoute("createbudget", new ResponseModel(id, string.Empty, string.Format(message, "")));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "Ceresr", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }

        [HttpPut(Name = "updatebudget")]
        public IActionResult UpdateBudget([FromBody] BudgetViewModel budget)
        {
            try
            {
                var returnCode = budgetService.UpdateBudget(budget);
                var message = string.Empty;

                if (returnCode > 0)
                {
                    message = Messages.BudgetMessages.BudgetUpdated;
                }
                else if (returnCode == 0)
                {
                    message = Messages.BudgetMessages.BudgetDoesNotExist;
                }
                else
                {
                    message = Messages.BudgetMessages.BudgetExist;
                }

                return CreatedAtRoute("updatebudget", new ResponseModel(returnCode, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "UpdateBudget", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }
    }
}
