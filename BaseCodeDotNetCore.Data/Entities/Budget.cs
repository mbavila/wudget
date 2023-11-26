using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Data.Entities
{
    public class Budget
    {
        public int BudgetID { get; set; }

        public int SubCategoryID { get; set; }

        [ForeignKey("SubCategoryID")]
        [JsonIgnore]
        public virtual SubCategory SubCategory { get; set; }

        public DateTime BudgetDate { get; set; }

        public double Amount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
