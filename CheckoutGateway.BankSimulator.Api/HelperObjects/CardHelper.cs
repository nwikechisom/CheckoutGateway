using CheckoutGateway.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutGateway.BankSimulator.Api.HelperObjects
{
    public class CardHelper
    {

        public List<Card> testCards = new List<Card>
        {
            new Card
            {
                CardNumber = "4111111111111111",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                HolderName = "John Doe",
                Customer = new Customer
                {
                    Email = "stevie.wonder@ny.com",
                    PhoneNumber = "1234567890",
                    Name = "Stevie Wonder",
                    TotalBalance = 1000,
                    Lien = 0
                }
            },
            new Card
            {
                CardNumber = "4111111111111112",
                ExpiryMonth = "12",
                ExpiryYear = "22",
                Cvv = "123",
                HolderName = "Jane Doe",
                Customer = new Customer
                {
                    Email = "vera.wyatt@ny.plat",
                    PhoneNumber = "1234567890",
                    Name = "Vera Wyatt",
                    TotalBalance = 80000,
                    Lien = 0
                }
            },
            new Card
            {
                CardNumber = "4111111111111113",
                ExpiryMonth = "12",
                ExpiryYear = "22",
                Cvv = "123",
                Status = CardStatus.Frozen,
                HolderName = "John Smith",
                Customer = new Customer
                {
                    Email = "chloe.darson@ny.zoo",
                    PhoneNumber = "1234567890",
                    Name = "Chloe Darson",
                    TotalBalance = 5000,
                    Lien = 0
                },
            },
            new Card
            {
                CardNumber = "4111111111111114",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                Status = CardStatus.NotAcceptingOnlinePayments,
                HolderName = "Jane Fred",
                Customer = new Customer
                {
                   Email = "munyi.ariela@mailer.uc",
                    PhoneNumber = "1234567890",
                    Name = "Munyi Ariel",
                    TotalBalance = 20000,
                    Lien = 0
                },
            },

            new Card
            {
                CardNumber = "4111111111111115",
                ExpiryMonth = "12",
                ExpiryYear = "23",
                Cvv = "123",
                Status = CardStatus.InsufficientFunds,
                HolderName = "Jane Doe",
                Customer = new Customer
                {
                   Email = "ace.kyd@mailer.uc",
                    PhoneNumber = "1234567890",
                    Name = "Ace Kyd",
                    TotalBalance = 0,
                    Lien = 0
                },
            },
        };
    }
}
