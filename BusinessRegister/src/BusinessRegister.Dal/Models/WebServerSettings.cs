namespace BusinessRegister.Dal.Models
{
    /// <summary>
    /// Web Server Configurations. 
    /// </summary>
    public class WebServerSettings
    {
        /// <summary>
        /// Defines section name in appsettings
        /// </summary>
        public const string SectionName = "WebServerSettings";

        /// <summary>
        /// Path to Hosted Directory
        /// </summary>
        public string VirtualDir { get; set; }
    }
}