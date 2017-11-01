namespace GameStore.App.Services.Contracts
{
    using System.Collections.Generic;

    public interface IOrderService
    {
        void Create(IEnumerable<int> gameIds, int userId);
    }
}
