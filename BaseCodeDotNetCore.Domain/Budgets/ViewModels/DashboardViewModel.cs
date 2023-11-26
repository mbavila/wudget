using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Budgets.ViewModels
{
    public class DashboardViewModel
    {
        public float ActualIncomeTotal { get; set; }
        public float ActualExpenseTotal { get; set; }
        public float BudgetIncomeTotal { get; set; }
        public float BudgetExpenseTotal { get; set; }
        public float IncomeVariance { get; set; }
        public float ExpenseVariance { get; set; }
        public float YTDExpenseVariance { get; set; }
        public float YTDBudgetVariance { get; set; }
    }
}
