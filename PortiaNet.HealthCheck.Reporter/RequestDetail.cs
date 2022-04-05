namespace PortiaNet.HealthCheck.Reporter
{
    public class RequestDetail
    {
        /// <summary>
        /// IP Address of the client which has sent the request
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// The username of the user which has logged in. This property will get value when the Authenticate middleware gets called before the health check, otherwise, it will be empty.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// The host name which has received the request.
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// The method of the request, that can be GET, POST, PUT, DELETE, PATCH.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The full path of the API.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// The query string of the reuqest, if exists.
        /// </summary>
        public string? QueryString { get; set; }

        /// <summary>
        /// The agent of the client user, contains the operating system, browser information, and some more information.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Total spent time from calling the main API till receiving the result or exception from it.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// If the API throws an exception which doesn't inherit <u>IHealthCheckKnownException</u>, this property will be true, otherwise it will be false.
        /// </summary>
        public bool HadError { get; set; }
    }
}
