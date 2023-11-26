namespace BaseCodeDotNetCore.Domain.Budgets
{
    using AutoMapper;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Paging;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.Budgets.ViewModels;
    using BaseCodeDotNetCore.Domain.Transactions.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<Budget> budgetRepository;
        readonly IRepository<Transaction> transactionRepository;
        private readonly IMapper mapper;

        public BudgetService(IUnitOfWork unit, IMapper mapper)
        {
            uow = unit;
            budgetRepository = uow == null ? null : uow.GetRepository<Budget>();
            transactionRepository = uow == null ? null : uow.GetRepository<Transaction>();
            this.mapper = mapper;
        }

        public DashboardViewModel GetDashboardDetails(BudgetSearchViewModel search)
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            if (search != null)
            {
                var budgetTable = budgetRepository.GetList(
                selector: source => mapper.Map<BudgetViewModel>(source),
                predicate: source => source.BudgetDate.Year == search.Year && source.BudgetDate.Month == search.Month,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.BudgetDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                dashboardViewModel.BudgetIncomeTotal = (float)budgetTable.Where(x => x.SubCategory.CategoryID == 1).Sum(x => x.Amount);
                dashboardViewModel.BudgetExpenseTotal = (float)budgetTable.Where(x => x.SubCategory.CategoryID == 2).Sum(x => x.Amount);
                
                var transactionTable = transactionRepository.GetList(
                selector: source => mapper.Map<TransactionViewModel>(source),
                predicate: source => source.TransactionDate.Year == search.Year && source.TransactionDate.Month == search.Month,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.TransactionDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                dashboardViewModel.ActualIncomeTotal = (float)transactionTable.Where(x => x.SubCategory.CategoryID == 1).Sum(x => x.Amount);
                dashboardViewModel.ActualExpenseTotal = (float)transactionTable.Where(x => x.SubCategory.CategoryID == 2).Sum(x => x.Amount);
                dashboardViewModel.IncomeVariance = dashboardViewModel.BudgetIncomeTotal - dashboardViewModel.ActualIncomeTotal;
                dashboardViewModel.ExpenseVariance = dashboardViewModel.BudgetExpenseTotal - dashboardViewModel.ActualExpenseTotal;

                var budgetTableYTD = budgetRepository.GetList(
                selector: source => mapper.Map<BudgetViewModel>(source),
                predicate: source => source.BudgetDate.Year == search.Year,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.BudgetDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                var transactionYTDTable = transactionRepository.GetList(
                 selector: source => mapper.Map<TransactionViewModel>(source),
                 predicate: source => source.TransactionDate.Year == search.Year,
                 include: source => source.Include(s => s.SubCategory),
                 orderBy: source => source.OrderByDescending(x => x.TransactionDate),
                 index: search.Pagination == null ? 0 : search.Pagination.Page,
                 size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                float ytdActualIncome = (float)transactionYTDTable.Where(x => x.SubCategory.CategoryID == 1).Sum(x => x.Amount);
                float ytdActualExpense = (float)transactionYTDTable.Where(x => x.SubCategory.CategoryID == 2).Sum(x => x.Amount);
                float ytdBudgetIncome = (float)budgetTableYTD.Where(x => x.SubCategory.CategoryID == 1).Sum(x => x.Amount);
                float ytdBudgetExpense = (float)budgetTableYTD.Where(x => x.SubCategory.CategoryID == 2).Sum(x => x.Amount);
                dashboardViewModel.YTDBudgetVariance = ytdBudgetIncome - ytdActualIncome;
                dashboardViewModel.YTDExpenseVariance = ytdBudgetExpense - ytdActualExpense;

            }

            return dashboardViewModel;
        }
        public IEnumerable<Object> GetBudgetPaginated(BudgetSearchViewModel search)
        {
            IPaginate<BudgetViewModel> budgets = null;
            IEnumerable<Object> grouped = null;

            if (search != null)
            {
                var budgetTable = budgetRepository.GetList(
                selector: source => mapper.Map<BudgetViewModel>(source),
                predicate: source => source.BudgetDate.Year == search.Year && source.BudgetDate.Month == search.Month,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.BudgetDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                var transactionTable = transactionRepository.GetList(
                selector: source => mapper.Map<TransactionViewModel>(source),
                predicate: source => source.TransactionDate.Year == search.Year && source.TransactionDate.Month == search.Month,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.TransactionDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize).Items;

                var query = (from budgetrow in budgetTable
                             join transactionrow in transactionTable 
                             on new { budgetrow.SubCategoryID } equals new { transactionrow.SubCategoryID } into jointable
                             from test in jointable.DefaultIfEmpty()
                             select new
                             {
                                 budgetrow, test
                             });

                grouped = from q in query
                              group q by new { q.budgetrow.BudgetID, q.budgetrow.Amount, q.budgetrow.SubCategoryID, q.budgetrow.SubCategory.Name } into grp
                              select new
                              { 
                                budgetID = grp.Key.BudgetID,
                                budgetAmount = grp.Key.Amount,
                                subCategoryID = grp.Key.SubCategoryID,
                                subCategoryName = grp.Key.Name,
                                transactionAmount = grp.Sum(x => x.test?.Amount ?? 0)

                              };


            }

            return grouped;
        }

        public int AddNewBudget(BudgetViewModel budget)
        {
            int returnValue = 0;

            if (budgetRepository.SingleEntity(e => (e.BudgetDate.Month.Equals(budget.BudgetDate.Month) && e.BudgetDate.Year.Equals(budget.BudgetDate.Year) && e.SubCategoryID.Equals(budget.SubCategoryID))) == null)
            {
                Budget entity = mapper.Map<Budget>(budget);
                entity.CreatedDate = DateTime.Now;
                entity.ModifiedDate = null;

                budgetRepository.Add(entity);
                uow.SaveChanges();

                returnValue = entity.BudgetID;
            }
            else
            {
                returnValue = -1;
            }

            return returnValue;
        }

        public int UpdateBudget(BudgetViewModel budget)
        {
            int returnValue = 0;

            Budget dbBudget = budgetRepository.SingleEntity(e => e.BudgetID == budget.BudgetID);

            if (dbBudget != null)
            {
                Budget dbExisting = budgetRepository.SingleEntity(
                    e => e.BudgetID != budget.BudgetID &&
                    e.BudgetDate.Month.Equals(budget.BudgetDate.Month) &&
                    e.BudgetDate.Year.Equals(budget.BudgetDate.Year) &&
                    e.SubCategoryID.Equals(budget.SubCategoryID));

                if (dbExisting == null)
                {
                    dbBudget = mapper.Map<Budget>(budget);
                    dbBudget.ModifiedDate = DateTime.Now;

                    budgetRepository.Update(dbBudget);
                    uow.SaveChanges();

                    returnValue = dbBudget.BudgetID;
                }
                else
                {
                    returnValue = -1;
                }
            }

            return returnValue;
        }
    }
}
