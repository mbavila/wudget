using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Data.Entities
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
     
        public string Name { get; set; }

        public bool? Active { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
