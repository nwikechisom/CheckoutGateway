using CheckoutGateway.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutGateway.BankSimulator.Api.HelperObjects
{
    public static class CardHelper
    {

        public static List<Card> testCards = new List<Card>
        {
            new Card
            {
                CardNumber = "4111111111111111",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                HolderName = "John Doe",
                Customer = testCustomers[0]
            },
            new Card
            {
                CardNumber = "4111111111111112",
                ExpiryMonth = "12",
                ExpiryYear = "22",
                Cvv = "123",
                HolderName = "Jane Doe",
                Customer = testCustomers[1]
            },
            new Card
            {
                CardNumber = "4111111111111113",
                ExpiryMonth = "12",
                ExpiryYear = "22",
                Cvv = "123",
                Status = CardStatus.Frozen,
                HolderName = "John Smith",
                Customer = testCustomers[2]
            },
            new Card
            {
                CardNumber = "4111111111111114",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                Status = CardStatus.NotAcceptingOnlinePayments,
                HolderName = "Jane Doe",
                Customer = testCustomers[3]
            },

            new Card
            {
                CardNumber = "4111111111111115",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                Status = CardStatus.InsufficientFunds,
                HolderName = "Jane Doe",
                Customer = testCustomers[4]
            },
        };

        public static List<Customer> testCustomers = new List<Customer>
        {
            new Customer
            {
                Email = "stevie.wonder@ny.com",
                PhoneNumber = "1234567890",
                Name = "name",
                TotalBalance = 1000,
                Lien = 0
            },
            new Customer
            {
                Email = "vera.wyatt@ny.plat",
                PhoneNumber = "1234567890",
                Name = "name",
                TotalBalance = 80000,
                Lien = 0
            },
            new Customer
            {
                Email = "chloe.darson@ny.zoo",
                PhoneNumber = "1234567890",
                Name = "name",
                TotalBalance = 5000,
                Lien = 0
            },
            new Customer
            {
               Email = "munyi.ariela@mailer.uc",
                PhoneNumber = "1234567890",
                Name = "name",
                TotalBalance = 20000,
                Lien = 0
            },
            new Customer
            {
               Email = "ace.kyd@mailer.uc",
                PhoneNumber = "1234567890",
                Name = "name",
                TotalBalance = 0,
                Lien = 0
            },
        };
    }
}
