using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.DTO
{
    public class PaginationDataOut<T>
    {
        public List<T> Data { get; set; }
        public int Count { get; set; }
    }
}
