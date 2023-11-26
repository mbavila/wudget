using BaseCodeDotNetCore.Data.Paging;
using BaseCodeDotNetCore.Domain.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Transactions
{
    public interface ITransactionService
    {
        public IPaginate<TransactionViewModel> GetTransactionPaginated(TransactionSearchViewModel search);

        public int AddTransaction(TransactionViewModel transaction);

        public int UpdateTransaction(TransactionViewModel transaction);
    }
}
