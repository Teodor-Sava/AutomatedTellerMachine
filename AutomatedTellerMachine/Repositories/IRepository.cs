using AutomatedTellerMachine.Models;

namespace AutomatedTellerMachine.Repositories
{
    public interface IRepository
    {
        bool DepositAmount(Transaction transaction);

        bool AddTransaction(int checkingAccountId, decimal amount, string message = null);
        decimal GetBalace(int checkingAccountId);
        bool CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance);
        bool AccountExists(int destinationCheckingAccount);
        bool Transfer(int sourceAccount, int destinationAccount, decimal amount, string message = null);
        CheckingAccount FindById(int id);
        CheckingAccount FindByAccountId(string userId);
    }
}