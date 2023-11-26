using BaseCodeDotNetCore.Domain;
using BaseCodeDotNetCore.Domain.Budgets;
using BaseCodeDotNetCore.Domain.Budgets.ViewModels;
using BaseCodeDotNetCore.Domain.Transactions;
using BaseCodeDotNetCore.Domain.Transactions.ViewModels;
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
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> logger;
        private readonly ITransactionService transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
        {
            this.logger = logger;
            this.transactionService = transactionService;
        }

        [HttpGet(Name = "transactionlistbypage")]
        public IActionResult GetBudget([FromQuery] TransactionSearchViewModel search)
        {
            var returnList = transactionService.GetTransactionPaginated(search);
            return CreatedAtRoute("transactionlistbypage", new ResponseModel(0, returnList, string.Empty));
        }

        [HttpPost(Name = "createtransaction")]
        public IActionResult CreateTransaction([FromBody] TransactionViewModel transaction)
        {
            try
            {
                var id = transactionService.AddTransaction(transaction);
                var message = Messages.TransactionMessages.TransactionSaved;

                return CreatedAtRoute("createtransaction", new ResponseModel(id, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "CreateTransaction", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }

        [HttpPut(Name = "updatetransaction")]
        public IActionResult UpdateTransaction([FromBody] TransactionViewModel transaction)
        {
            try
            {
                var returnCode = transactionService.UpdateTransaction(transaction);
                var message = string.Empty;

                if (returnCode > 0)
                {
                    message = Messages.TransactionMessages.TransactionUpdated;
                }
                else if (returnCode == 0)
                {
                    message = Messages.TransactionMessages.TransactionDoesNotExist;
                }

                return CreatedAtRoute("updatetransaction", new ResponseModel(returnCode, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "UpdateBudget", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }
    }
}
