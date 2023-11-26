

namespace BaseCodeDotNetCore.Domain.Transactions
{
    using System;
    using System.Linq;
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Paging;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.Transactions.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<Transaction> transactionRepository;
        private readonly IMapper mapper;

        public TransactionService(IUnitOfWork unit, IMapper mapper)
        {
            uow = unit;
            transactionRepository = uow == null ? null : uow.GetRepository<Transaction>();

            this.mapper = mapper;
        }

        public IPaginate<TransactionViewModel> GetTransactionPaginated(TransactionSearchViewModel search)
        {
            IPaginate<TransactionViewModel> transactions = null;

            if (search != null)
            {
                transactions = transactionRepository.GetList(
                selector: source => mapper.Map<TransactionViewModel>(source),
                predicate: source => source.TransactionDate >= search.TransactionStartDate && source.TransactionDate <= search.TransactionEndDate,
                include: source => source.Include(s => s.SubCategory),
                orderBy: source => source.OrderByDescending(x => x.TransactionDate),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize);
            }

            return transactions;
        }

        public int AddTransaction(TransactionViewModel transaction)
        {
            int returnValue = 0;

            Transaction entity = mapper.Map<Transaction>(transaction);
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = null;

            transactionRepository.Add(entity);
            uow.SaveChanges();

            returnValue = entity.TransactionId;

            return returnValue;
        }

        public int UpdateTransaction(TransactionViewModel transaction)
        {
            int returnValue = 0;

            Transaction dbTransaction = transactionRepository.SingleEntity(e => e.TransactionId == transaction.TransactionId);

            if (dbTransaction != null)
            {


                dbTransaction = mapper.Map<Transaction>(transaction);
                dbTransaction.ModifiedDate = DateTime.Now;

                transactionRepository.Update(dbTransaction);
                uow.SaveChanges();

                returnValue = dbTransaction.TransactionId;
            }

            return returnValue;
        }
    }
}
