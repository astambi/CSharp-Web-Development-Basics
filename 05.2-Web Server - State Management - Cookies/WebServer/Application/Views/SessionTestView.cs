namespace WebServer.Application.Views
{
    using Server.Contracts;
    using System;

    public class SessionTestView : IView
    {
        private readonly DateTime dateTime;

        public SessionTestView(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public string View()
        {
            return $"<h1>Saved date: {this.dateTime}</h1>";
        }
    }
}
