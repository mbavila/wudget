using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Transactions.ViewModels
{
    public class TransactionSearchViewModel
    {
        public int CategoryID { get; set; }

        public DateTime TransactionStartDate { get; set; }

        public DateTime TransactionEndDate { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
