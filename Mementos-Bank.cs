using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        BankAccount account = new BankAccount(5000);
        TransactionHistory history = new TransactionHistory();

        account.Deposit(2000);
        history.AddTransaction(account.Save());  // Save state

        account.Withdraw(1000);
        history.AddTransaction(account.Save());  // Save state

        Console.WriteLine("\nCurrent Balance: " + account.Balance);

        // Undo last transaction
        account.Restore(history.Undo());
        Console.WriteLine("After Undo: " + account.Balance);
    }
}

class BankAccount
{
    public int Balance { get; private set; }

    public BankAccount(int balance) => Balance = balance;

    public void Deposit(int amount) => Balance += amount;
    public void Withdraw(int amount) => Balance -= amount;

    public TransactionMemento Save() => new TransactionMemento(Balance);
    
    public void Restore(TransactionMemento memento) => Balance = memento.GetBalance();
}

class TransactionMemento
{
    private readonly int balance;
    
    public TransactionMemento(int balance) => this.balance = balance;

    public int GetBalance() => balance;
}

class TransactionHistory
{
    private Stack<TransactionMemento> transactions = new Stack<TransactionMemento>();

    public void AddTransaction(TransactionMemento memento) => transactions.Push(memento);
    public TransactionMemento Undo() => transactions.Count > 0 ? transactions.Pop() : new TransactionMemento(0);
}
