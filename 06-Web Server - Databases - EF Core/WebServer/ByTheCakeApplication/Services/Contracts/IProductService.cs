namespace WebServer.ByTheCakeApplication.Services.Contracts
{
    public interface IProductService
    {
        void Create(string name, decimal price, string imageUrl);
    }
}
