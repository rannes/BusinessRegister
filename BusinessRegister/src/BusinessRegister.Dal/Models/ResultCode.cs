namespace BusinessRegister.Dal.Models
{
    /// <summary>
    /// Result codes to return correct / handled results.
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// OK Result
        /// </summary>
        Ok,

        /// <summary>
        /// General Server Error
        /// </summary>
        ServerError,

        /// <summary>
        /// Database connection failed to open, probably invalid Database name.
        /// </summary>
        DatabaseConnectionFailedToOpen,

        /// <summary>
        /// Username or password is invalid.
        /// </summary>
        UsernameOrPasswordIsInvalid,

        /// <summary>
        /// Zip file location is invalid.
        /// </summary>
        ZipFileLocationInvalid,

        /// <summary>
        /// File extension must be .zip
        /// </summary>
        FileExtensionMustBeZip,

        /// <summary>
        /// Zip file does not contain any files
        /// </summary>
        ZipFileDoesNotCotainAnyFiles,

        /// <summary>
        /// Zip file did not contain correct / expected file.
        /// </summary>
        ZipFileDidNotContainCorrectFile
    }
}