using System;
using System.Globalization;
using System.Linq;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using (var db = new BillsPaymentSystemContext())
            {
                Seed(db);

                var userId = int.Parse(Console.ReadLine());

                ListUserDetails(db, userId);

                PayBills(db, userId);

            }
        }

        private static void PayBills(BillsPaymentSystemContext db, int userId)
        {
            decimal withdrawAmount = decimal.Parse(Console.ReadLine());


            decimal userBankAccountBalance = db.PaymentMethods
                                .Where(pm => pm.UserId == userId)
                                .Select(x => x.BankAccount.Balance)
                                .Sum();

            decimal userCreditCardBalance = db.PaymentMethods
                .Where(pm => pm.UserId == userId)
                .Select(x => x.CreditCard.MoneyOwned)
                .Sum();

            if (userBankAccountBalance + userCreditCardBalance < withdrawAmount)
            {
                Console.WriteLine("Insufficient funds!");
                return;
            }

            var bankAccounts = db.PaymentMethods
                .Where(pm => pm.UserId == userId)
                .Select(x => x.BankAccount)
                .Where(x => x != null)
                .OrderBy(x => x.BankAccountId)
                .ToList();

            foreach (var method in bankAccounts)
            {
                decimal accountBalance = method.Balance;

                method.Withdraw(withdrawAmount);

                withdrawAmount -= Math.Abs(accountBalance - method.Balance);

                if (withdrawAmount == 0)
                {
                    break;
                }
            }

            if (withdrawAmount > 0)
            {
                var creditCards = db.PaymentMethods
                    .Where(pm => pm.UserId == userId)
                    .Select(x => x.CreditCard)
                    .Where(x => x != null)
                    .OrderBy(x => x.CreditCardId)
                    .ToList();

                foreach (var method in creditCards)
                {
                    decimal credit = method.MoneyOwned;

                    method.Withdraw(withdrawAmount);

                    withdrawAmount -= Math.Abs(credit - method.MoneyOwned);

                    if (withdrawAmount == 0)
                    {
                        break;
                    }
                }

                db.SaveChanges();
            }
        }

        private static void ListUserDetails(BillsPaymentSystemContext db, int userId)
        {
            var user = db.Users
                                .Where(u => u.UserId == userId)
                                .Select(x => new
                                {
                                    x.UserId,
                                    Name = $"{x.FirstName} {x.LastName}",
                                    BankAccounts = x.PaymentMethods
                                                    .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                                                    .Select(b => b.BankAccount)
                                                    .ToList(),
                                    CreditCards = x.PaymentMethods
                                                    .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                                                    .Select(c => c.CreditCard)
                                                    .ToList()
                                })
                                .SingleOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                return;
            }

            Console.WriteLine($"User: {user.Name}");
            Console.WriteLine("Bank Accounts:");

            foreach (var method in user.BankAccounts)
            {
                if (method != null)
                {
                    Console.WriteLine($"-- ID: {method.BankAccountId}");
                    Console.WriteLine($"--- Balance: {method.Balance:f2}");
                    Console.WriteLine($"--- Bank: {method.BankName}");
                    Console.WriteLine($"--- SWIFT: {method.SwiftCode}");
                }
            }

            Console.WriteLine("Credit Cards:");

            foreach (var method in user.CreditCards)
            {
                if (method != null)
                {
                    Console.WriteLine($"-- ID: {method.CreditCardId}");
                    Console.WriteLine($"--- Limit: {method.Limit:f2}");
                    Console.WriteLine($"--- Money Owed: {method.MoneyOwned:f2}");
                    Console.WriteLine($"--- Limit Left:: {method.LimitLeft:f2}");
                    Console.WriteLine($"--- Expiration Date: {method.ExpirationDate.ToString("yyyy/mm", CultureInfo.InvariantCulture)}");
                }
            }
        }

        private static void Seed(BillsPaymentSystemContext db)
        {
            using (db)
            {
                var users = new User[]
                {
                    new User() {FirstName = "Gay", LastName = "Gilbert", Email = "a@d.com", Password = "asaasas"},
                    new User() {FirstName = "John", LastName = "Petrov", Email = "a@d.com", Password = "asaasas"},
                };

                var creditCards = new CreditCard[]
                {
                    new CreditCard(2000, 1000, DateTime.Now),
                    new CreditCard(2000, 500, DateTime.Now),
                    new CreditCard(2000, 2000, DateTime.Now),
                    new CreditCard(2000, 0, DateTime.Now),
                };

                var bankAccounts = new BankAccount[]
                {
                    new BankAccount(3000, "DSK", "SDAKLA73JD"),
                    new BankAccount(500, "OBB", "SAD87FH6FD"),
                    new BankAccount(1000, "UNICREDIT", "8CDS8C8Z8"),
                    new BankAccount(0, "PIREUS", "AD89D7AS"),
                };

                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod(PaymentMethodType.CreditCard, users[0], creditCards[0]),
                    new PaymentMethod(PaymentMethodType.BankAccount, users[1], bankAccounts[2]),
                    new PaymentMethod(PaymentMethodType.CreditCard, users[1], creditCards[1]),
                    new PaymentMethod(PaymentMethodType.CreditCard, users[0], creditCards[2]),
                    new PaymentMethod(PaymentMethodType.BankAccount, users[0], bankAccounts[1]),
                    new PaymentMethod(PaymentMethodType.BankAccount, users[1], bankAccounts[0]),
                    new PaymentMethod(PaymentMethodType.BankAccount, users[1], bankAccounts[3]),
                    new PaymentMethod(PaymentMethodType.CreditCard, users[1], creditCards[3]),
                };

                db.Users.AddRange(users);
                db.CreditCards.AddRange(creditCards);
                db.BankAccounts.AddRange(bankAccounts);
                db.PaymentMethods.AddRange(paymentMethods);

                db.SaveChanges();
            }
        }
    }
}
