﻿using Eshoppy.FinanceModule;
using Eshoppy.FinanceModule.Interfaces;
using Eshoppy.FinanceModule.Models;
using Eshoppy.TransactionModule;
using Eshoppy.TransactionModule.Interfaces;
using Eshoppy.UserModule.Interfaces;
using Eshoppy.UserModule.Models;
using Eshoppy.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshoppy.UserModule
{
    public class ClientManager : IClientManager
    {
        public ShoppingClient ClientList { get; set;}
        public IFinanceManager FinanceManager { get; set; }

        public ClientManager(ShoppingClient clientList, IFinanceManager financeManager)
        {
            this.ClientList = clientList;
            this.FinanceManager = financeManager;
        }

        public void RegisterUser(String name, String surname, String email, String phone, string address)
        {
            IUser user = new User(name, surname, email, phone, address);
            this.ClientList.AddClient(user);
        }

        public void RegisterOrg(int tin, string name, string adress, string phoneNumber, string email)
        {
            IOrganization organization = new Organization(tin, name, adress, phoneNumber, email);
            this.ClientList.AddClient(organization);
        }

        public void ChangeUserAccount(IUser user, List<IAccount> accounts)
        {
            foreach (IAccount userAccount in user.Accounts)
            {
                foreach (IAccount newAccount in accounts)
                if (userAccount.Id.Equals(newAccount.Id))
                    {
                        userAccount.AccountNumber = newAccount.AccountNumber;
                        break;
                    }
            }
        }

        public void ChangeOrgAccount(IOrganization organization, List<IAccount> accounts)
        {
            foreach (IAccount organizationAccount in organization.Accounts)
            {
                foreach (IAccount newAccount in accounts)
                    if (organizationAccount.Id.Equals(newAccount.Id))
                    {
                        organizationAccount.AccountNumber = newAccount.AccountNumber;
                        break;
                    }
            }
        }

        public List<ITransaction> SearchHistory(IClient client, DateTime date, int transactionCategory)
        {
            List<ITransaction> transactions = new List<ITransaction>();
            if (transactionCategory == 0)
            {
                foreach (ITransaction transaction in client.Transactions)
                {
                    if (DateTime.Compare(transaction.TransactionDate, date) > 0 && transaction.TransactionCategory == 0)
                    {
                        transactions.Add(transaction);
                    }
                }
                return transactions;
            }
            else if (transactionCategory == 1)
            {
                foreach (ITransaction transaction in client.Transactions)
                {
                    if (DateTime.Compare(transaction.TransactionDate, date) > 0 && transaction.TransactionCategory == 1)
                    {
                        transactions.Add(transaction);
                    }
                }
                return transactions;
            }
            else
            {
                throw new Exception("You can only add 1 or 0 as parameter for transaction category");
            }
        }

        public IClient GetClientById(Guid id)
        {
            foreach (IClient client in this.ClientList.Clients)
            {
                if (client.Id.Equals(id))
                {
                    return client;
                }
            }
            return null;
        }


        public void AddFunds(IClient client, double amount, ICurrency currency, IEmailSender emailSender, ILogger logger)
        {
            if (client.Accounts.Count > 0)
            {
                IAccount account = client.Accounts[0];
                if (!(currency is DinarCurrency))
                {
                    amount = FinanceManager.Convert(currency, amount);
                }

                if (client is IOrganization)
                {
                    if (amount < 10000)
                    {
                        logger.ErrorLogg("Amount cannot be smaller than 10000 for organizations");
                        return;
                    }
                }

                if (account.Credit != null)
                {
                    FinanceManager.CreditPayment(account.Id, amount);
                    emailSender.SendEmail("Credit debt is reduced.", client.Email);
                }
                else
                {
                    FinanceManager.AccountPayment(account.Id, amount);
                }
            }
            else
            {
                throw new Exception("This user has 0 accounts");
            }
        }

    }
}
