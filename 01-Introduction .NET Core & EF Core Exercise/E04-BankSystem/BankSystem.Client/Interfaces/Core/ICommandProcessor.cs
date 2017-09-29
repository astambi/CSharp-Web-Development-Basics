namespace BankSystem.Client.Interfaces.Core
{
    public interface ICommandProcessor
    {
        string ProcessCommand(string input);
    }
}
