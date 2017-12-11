using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FastFood.Models.Enums;

namespace FastFood.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public OrderType Type { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
