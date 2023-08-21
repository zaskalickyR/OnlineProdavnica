using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.DTO
{
    public class PaginationDataIn
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string? SearchName { get; set; }
        public bool? FilterByUserRole { get; set; }
    }
}
