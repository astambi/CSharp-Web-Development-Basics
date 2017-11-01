namespace GameStore.App.Models.Orders
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        private readonly ICollection<int> gameIds;

        public ShoppingCart()
        {
            this.gameIds = new HashSet<int>();
        }

        public void AddGame(int id) => this.gameIds.Add(id);

        public void RemoveGame(int id) => this.gameIds.Remove(id);

        public IEnumerable<int> AllGames() => new List<int>(this.gameIds);

        public void Empty() => this.gameIds.Clear();
    }
}
