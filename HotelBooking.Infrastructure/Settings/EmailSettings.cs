namespace HotelBooking.Infrastructure.Settings
{
    /// <summary>
    /// Represents the email settings configuration for the SMTP server.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the sender's email address.
        /// </summary>
        public string SenderEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sender's display name.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username for SMTP authentication.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for SMTP authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled.
        /// </summary>
        public bool EnableSSL { get; set; }
    }
}
