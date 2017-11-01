namespace GameStore.App.Services
{
    using Data;
    using Data.Models;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly GameStoreDbContext context;

        public OrderService(GameStoreDbContext context)
        {
            this.context = context;
        }

        public void Create(IEnumerable<int> gameIds, int userId)
        {
            var boughtGames = this.context
                .Orders
                .Where(o => o.UserId == userId && gameIds.Contains(o.GameId))
                .Select(o => o.GameId)
                .ToList();

            var newGames = gameIds
                .Where(id => !boughtGames.Contains(id));

            foreach (var gameId in newGames)
            {
                var order = new Order
                {
                    GameId = gameId,
                    UserId = userId
                };

                this.context.Orders.Add(order);
            }

            this.context.SaveChanges();
        }
    }
}
