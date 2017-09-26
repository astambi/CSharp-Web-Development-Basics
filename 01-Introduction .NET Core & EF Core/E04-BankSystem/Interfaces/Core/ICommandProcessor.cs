namespace BankSystem.Interfaces.Core
{
    public interface ICommandProcessor
    {
        string Execute(string input);
    }
}
