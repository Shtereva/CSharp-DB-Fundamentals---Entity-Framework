using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using FastFood.Models;
using FastFood.Models.Enums;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
    public class OrderDto
    {
        [Required]
        public string Customer { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Employee { get; set; }

        [Required]
        public string DateTime { get; set; }

        public string Type { get; set; } = "ForHere";

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
