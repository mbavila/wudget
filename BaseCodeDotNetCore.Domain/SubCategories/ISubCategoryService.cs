using BaseCodeDotNetCore.Data.Paging;
using BaseCodeDotNetCore.Domain.SubCategories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.SubCategories
{
    public interface ISubCategoryService
    {
        public IPaginate<SubCategoryViewModel> GetSubCategoryPaginated(SubCategorySearchViewModel search);

        public int AddNewSubCategory(SubCategoryViewModel subCategory);

        public int UpdateSubCategory(SubCategoryViewModel subCategory);
    }
}
