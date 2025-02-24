using System;
using System.Collection.Generic;

public class User
{
    private BankAccount _account;

    public User(BankAccount account)
    {
        _account = account;
    }

    public void PerformTransaction(int amount, string type)
    {
        if(type == "deposit")
        {
            _account.Deposit(amount);
        } else if (type == "withdraw")
        {
            _account.Withdraw(amount);
        }
    }
}

public class TransactionMemento
{
    public int Balance { get ; private set; }

    public TransactionMemento(int balance)
    {
        Balance = balance;
    }

    public int GetBalance()
    {
        return Balance;
    }
}

public class BankAccount 
{
    public int Balance { get; protected set; }

    public BankAccount(int balance)
    {
        Balance = balance;
    }

    public void Deposit (int main) 
    {
        Balance += amount;
    }

    public void Withdraw(int amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
        }
        else {
            throw new InvalidOperationException("Insufficient Amount");
        }
    }

    public TransactionMemento Save()
    {
        return new TransactionMemento(Balance);
    }

    public void Restore(TransactionMemento memento)
    {
        Balance = memento.GetBalance();
    }

    public class SavingAccount : BankAccount
    {
        public double InterestRate {get; private set;}

        public SavingAccount(int balance, double interestRate) : base(Balance)
        {
            InterestRate = interestRate;
        }

        public double CalculateInterest()
        {
            return Balance * InterestRate;
        }

        public void ApplyInterest()
        {
            Balance += (int)CalculateInterest();
        }
    }

    public class CheckingAccount : BankAccount
    {
        public int OverdraftLimit {get; private set;}

        public CheckingAccount(int balance, int OverdraftLimit) : base(Balance)
        {
            OverdraftLimit = OverdraftLimit;
        }

        public bool CheckOverdraft(int amount)
        {
            return (Balance - amount) >= -OverdraftLimit;
        }

        public void ApplyOverdraftFee()
        {
            if (Balance < 0)
            {
                Balance -= 25;
            }
        }
    }
}

public class TransactionHistory
{
    private List<TransactionMemento> transactions = new List<TransactionMemento>;

    public void AddTransaction(TransactionMemento memento)
    {
        transactions.Add(memento);
    }

    public TransactionMemento Undo()
    {
        if (transactions.Count > 0)
        {
            var lastMemento = transactions[transactions.Count - 1];
            transactions.RemoveAt(transactions.Count -1);
            return lastMemento;
        }
        throw new InvalidOperationException("No transactions to undo");
    }
}

class Program 
{
    static void Main(string[] args)
    {
        var savingAccount = new SavingAccount(100, 0.05);
        var TransactionHistory = new TransactionHistory();

        SavingAccount.Deposit(500);
        TransactionHistory.AddTransaction(savingAccount.Save());

        savingAccount.withdraw(200);
        TransactionHistory.AddTransaction(savingAccount.Save());

        var lastState = TransactionHistory.Undo();
        savingAccount.Restore(lastState);

        Console.WriteLine($"Final Balance: {savingAccount.Balance}");
    }
}