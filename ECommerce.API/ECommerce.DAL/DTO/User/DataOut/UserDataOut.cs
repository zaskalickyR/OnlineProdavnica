using ECommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.DTO.User.DataOut
{
    public class UserDataOut
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public DateTime BirthDate { get; set; }

    }

}
