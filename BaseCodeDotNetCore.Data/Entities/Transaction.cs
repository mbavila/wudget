using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Data.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public string Details { get; set; }

        public DateTime TransactionDate { get; set; }

        public double Amount { get; set; }

        public int SubCategoryID { get; set; }

        [ForeignKey("SubCategoryID")]
        [JsonIgnore]
        public virtual SubCategory SubCategory { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
