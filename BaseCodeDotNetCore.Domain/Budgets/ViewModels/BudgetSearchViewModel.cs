using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Budgets.ViewModels
{
    public class BudgetSearchViewModel
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
