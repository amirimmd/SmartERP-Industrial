using System;
using System.ComponentModel.DataAnnotations;

namespace SmartERP.Core
{
    public class OfficialLetter
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string LetterNumber { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        public string Recipient { get; set; } = string.Empty;
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime IssueDate { get; set; } = DateTime.Now;
        
        public string Status { get; set; } = "صادر شده";
    }
}