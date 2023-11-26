using BaseCodeDotNetCore.Data.Paging;
using BaseCodeDotNetCore.Domain.Budgets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Budgets
{
    public interface IBudgetService
    {
        public IEnumerable<Object> GetBudgetPaginated(BudgetSearchViewModel search);

        public int AddNewBudget(BudgetViewModel budget);

        public DashboardViewModel GetDashboardDetails(BudgetSearchViewModel budget);

        public int UpdateBudget(BudgetViewModel budget);
    }
}
