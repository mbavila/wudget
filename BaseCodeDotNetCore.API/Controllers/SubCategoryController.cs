using BaseCodeDotNetCore.Domain;
using BaseCodeDotNetCore.Domain.SubCategories;
using BaseCodeDotNetCore.Domain.SubCategories.ViewModels;
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
    public class SubCategoryController : ControllerBase
    {
        private readonly ILogger<SubCategoryController> logger;
        private readonly ISubCategoryService subCategoryService;

        public SubCategoryController(ILogger<SubCategoryController> logger, ISubCategoryService subCategoryService)
        {
            this.logger = logger;
            this.subCategoryService = subCategoryService;
        }

        [HttpGet(Name = "subcategorylistbypage")]
        public IActionResult GetSubCategories([FromQuery] SubCategorySearchViewModel search)
        {
            var returnList = subCategoryService.GetSubCategoryPaginated(search);
            return CreatedAtRoute("subcategorylistbypage", new ResponseModel(0, returnList, string.Empty));
        }

        [HttpPost(Name = "createsubcategory")]
        public IActionResult CreateSubCategory([FromBody] SubCategoryViewModel subcategory)
        {
            try
            {
                var id = subCategoryService.AddNewSubCategory(subcategory);
                var message = id > 0 ? Messages.SubcategoryMessages.SubCategorySaved : Messages.SubcategoryMessages.SubCategoryExist;

                return CreatedAtRoute("createsubcategory", new ResponseModel(id, string.Empty, string.Format(message, subcategory?.Name)));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "CreateSubCategory", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }

        [HttpPut(Name = "updatesubcategory")]
        public IActionResult UpdateSubcategory([FromBody] SubCategoryViewModel subcategory)
        {
            try
            {
                var returnCode = subCategoryService.UpdateSubCategory(subcategory);
                var message = string.Empty;

                if (returnCode > 0)
                {
                    message = string.Format(Messages.SubcategoryMessages.SubCategoryUpdated, subcategory?.Name);
                }
                else if (returnCode == 0)
                {
                    message = Messages.SubcategoryMessages.SubCategoryDoesNotExist;
                }
                else
                {
                    message = string.Format(Messages.SubcategoryMessages.SubCategoryExist, subcategory?.Name);
                }

                return CreatedAtRoute("updatesubcategory", new ResponseModel(returnCode, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "UpdateSubCategory", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }
    }
}
