namespace ECommerce.DAL.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}