using System;
using System.Collections.Generic;

namespace Task04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var accounts = new List<Account>();
            accounts.Add(new Account("Larry"));
            accounts.Add(new Account("Moe", 2000));
            accounts.Add(new Account("Curly", 5000));

            AccountUtil.Display(accounts);
            AccountUtil.ExecuteTransaction(accounts, 1000, "Deposit");
            AccountUtil.ExecuteTransaction(accounts, 2000, "Withdraw");

           
            var savAccounts = new List<Account>();
            savAccounts.Add(new SavingsAccount());
            savAccounts.Add(new SavingsAccount("Superman"));
            savAccounts.Add(new SavingsAccount("Batman", 2000));
            savAccounts.Add(new SavingsAccount("Wonderwoman", 5000, 5.0));

            AccountUtil.Display(savAccounts);
            AccountUtil.ExecuteTransaction(savAccounts, 1000, "Deposit");
            AccountUtil.ExecuteTransaction(savAccounts, 2000, "Withdraw");

           
            var checkingAccounts = new List<Account>();
            checkingAccounts.Add(new CheckingAccount());
            checkingAccounts.Add(new CheckingAccount("Larry2"));
            checkingAccounts.Add(new CheckingAccount("Moe2", 2000));
            checkingAccounts.Add(new CheckingAccount("Curly2", 5000));

            AccountUtil.Display(checkingAccounts);
            AccountUtil.ExecuteTransaction(checkingAccounts, 1000, "Deposit");
            AccountUtil.ExecuteTransaction(checkingAccounts, 2000, "Withdraw");
            AccountUtil.ExecuteTransaction(checkingAccounts, 3000, "Withdraw");

            
            var trustAccounts = new List<Account>();
            trustAccounts.Add(new TrustAccount());
            trustAccounts.Add(new TrustAccount("Vampire"));
            trustAccounts.Add(new TrustAccount("Zombie", 10000, 6.0));

            AccountUtil.Display(trustAccounts);
            AccountUtil.ExecuteTransaction(trustAccounts, 5000, "Deposit");
            AccountUtil.ExecuteTransaction(trustAccounts, 3000, "Withdraw");

            Console.ReadLine();
        }
    }

    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount <= 0)
                return false;

            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"[{Name}: {Balance:C}]";
        }
    }

    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string name = "Unnamed Savings Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                Balance += (amount * (InterestRate / 100));
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"[Savings Account -> {Name}: {Balance:C}, Interest Rate: {InterestRate}%]";
        }
    }

    public class CheckingAccount : Account
    {
        private const double Fee = 1.50;

        public CheckingAccount(string name = "Unnamed Checking Account", double balance = 0.0)
            : base(name, balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            double totalAmount = amount + Fee;
            return base.Withdraw(totalAmount);
        }

        public override string ToString()
        {
            return $"[Checking Account -> {Name}: {Balance:C}, Fee per withdrawal: {Fee:C}]";
        }
    }

    public class TrustAccount : SavingsAccount
    {
        private const double BonusAmount = 50.00;
        private const double MinimumDepositForBonus = 5000.00;
        private const double MaxWithdrawPercent = 0.20;

        public TrustAccount(string name = "Unnamed Trust Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance, interestRate)
        {
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                if (amount >= MinimumDepositForBonus)
                {
                    Balance += BonusAmount;
                }
                return true;
            }
            return false;
        }

        public override bool Withdraw(double amount)
        {
            if (amount > (Balance * MaxWithdrawPercent))
            {
                Console.WriteLine($"[Withdrawal Failed for {Name}]: Amount exceeds 20% of current balance.");
                return false;
            }
            return base.Withdraw(amount);
        }

        public override string ToString()
        {
            return $"[Trust Account -> {Name}: {Balance:C}, Interest Rate: {InterestRate}%]";
        }
    }

    public static class AccountUtil
    {
        
        public static void Display(List<Account> accounts)
        {
            Console.WriteLine("\n--- Current Accounts ---");
            foreach (var acc in accounts)
            {
                Console.WriteLine(acc);
            }
        }
               public static void ExecuteTransaction(List<Account> accounts, double amount, string type)
        {
            Console.WriteLine($"\n--- {type}ing {amount:C} {(type == "Deposit" ? "to" : "from")} Accounts ---");

            foreach (var acc in accounts)
            {
                bool success = false;

                if (type == "Deposit")
                {
                    success = acc.Deposit(amount);
                }
                else if (type == "Withdraw")
                {
                    success = acc.Withdraw(amount);
                }

                if (success)
                    Console.WriteLine($"Successfully {type}ed {(type == "Deposit" ? "to" : "from")}: {acc.Name}");
                else
                    Console.WriteLine($"Failed {type} {(type == "Deposit" ? "to" : "from")}: {acc.Name}");
            }
        }
    }
}