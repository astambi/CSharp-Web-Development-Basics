namespace WebServer.Server.Http
{
    using Contracts;
    using System.Collections.Concurrent; // NB!

    public static class SessionStore
    {
        public const string SessionCookieKey = "MY_SID";

        // SessionId => HttpSession
        private static readonly ConcurrentDictionary<string, IHttpSession> sessions =
            new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession Get(string id)
            => sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
