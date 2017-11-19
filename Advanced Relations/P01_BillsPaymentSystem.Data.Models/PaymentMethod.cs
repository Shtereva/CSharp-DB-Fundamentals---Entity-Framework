using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
        }

        public PaymentMethod(PaymentMethodType type, User user)
        {
            this.Type = type;
            this.User = user;
        }

        public PaymentMethod(PaymentMethodType type, User user, BankAccount bankAccount)
            : this(type, user)
        {
            BankAccount = bankAccount;
        }

        public PaymentMethod(PaymentMethodType type, User user, CreditCard creditCard)
            : this(type, user)
        {
            CreditCard = creditCard;
        }

        public int Id { get; set; }

        public PaymentMethodType Type { get; set; }

        public int UserId{ get; set; }
        public User User { get; set; }

        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }

    }
}
