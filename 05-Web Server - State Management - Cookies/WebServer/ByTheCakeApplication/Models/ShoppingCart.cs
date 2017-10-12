﻿namespace WebServer.ByTheCakeApplication.Models
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public const string SessionKey = "^%Current_Shopping_Card%^";
        
        public List<Cake> Orders { get; private set; } = new List<Cake>();
    }
}
