﻿using System;
using System.Linq;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
	        var item = context.Items.SingleOrDefault(i => i.Name == itemName);

	        if (item == null)
	        {
	            return $"Item {itemName} not found!";
	        }

	        decimal oldPrice = item.Price;

	        item.Price = newPrice;

	        context.SaveChanges();

	        var msg = $"{item.Name} Price updated from ${oldPrice:F2} to ${newPrice:F2}";

	        return msg;
	    }
    }
}
