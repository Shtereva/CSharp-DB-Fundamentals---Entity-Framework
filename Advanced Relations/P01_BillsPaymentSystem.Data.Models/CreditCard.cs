using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
        }

        public CreditCard(decimal limit, decimal moneyOwned, DateTime expirationDate)
        {
            this.Limit = limit;
            this.MoneyOwned = moneyOwned;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; set; }

        private decimal limit;

        public decimal Limit
        {
            get => this.limit;
            private set
            {
                this.limit = value;
            }
        }

        private decimal moneyOwned;
        public decimal MoneyOwned
        {
            get => this. moneyOwned;
            private set { this.moneyOwned = value; }
        }

        public decimal LimitLeft => Limit - MoneyOwned;

        public DateTime ExpirationDate { get; set; }
         
        public int PaymentId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Deposit should be at least 0.01");
            }
            this.MoneyOwned += amount;

            if (this.MoneyOwned > this.Limit)
            {
                this.Limit = MoneyOwned;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Withdraw should be at least 0.01");
            }

            this.MoneyOwned = Math.Max(this.MoneyOwned - amount, 0);
        }
    }
}
