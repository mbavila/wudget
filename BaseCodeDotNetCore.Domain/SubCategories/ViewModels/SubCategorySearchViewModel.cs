using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.SubCategories.ViewModels
{
    public class SubCategorySearchViewModel
    {
        public int CategoryID { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
