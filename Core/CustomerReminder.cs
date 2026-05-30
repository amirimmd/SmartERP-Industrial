using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class CustomerReminder
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public DateTime ReminderDate { get; set; } = DateTime.Now.AddDays(1);
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsDismissed { get; set; } = false;

        // کم، متوسط، مهم، فوری
        public string Priority { get; set; } = "متوسط";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
