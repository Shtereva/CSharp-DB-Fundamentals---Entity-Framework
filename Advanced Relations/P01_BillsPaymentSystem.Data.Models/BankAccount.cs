using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
        }

        public BankAccount(decimal balance, string bankName, string swiftCode)
        {
            this.Balance = balance;
            this.BankName = bankName;
            this.SwiftCode = swiftCode;
        }

        public int BankAccountId { get; set; }

        private decimal balance;
        public decimal Balance
        {
            get => this.balance;
            private set { this.balance = value; }
        }

        public string BankName { get; set; }

        public string SwiftCode { get; set; }

        public int PaymentId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Deposit should be at least 0.01");
            }

            this.Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Withdraw should be at least 0.01");
            }

            this.Balance =  Math.Max(this.Balance - amount, 0);
        }
    }
}
