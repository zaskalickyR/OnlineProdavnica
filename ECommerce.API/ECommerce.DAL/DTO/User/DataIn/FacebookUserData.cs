using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.DTO.User.DataIn
{
    public class FacebookUserData
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public PictureData Picture { get; set; }
    }

    public class PictureData
    {
        public Picture Data { get; set; }
    }

    public class Picture
    {
        public int Height { get; set; }
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }
}
