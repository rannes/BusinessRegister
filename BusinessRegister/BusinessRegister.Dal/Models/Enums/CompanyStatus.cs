using System.Xml.Serialization;

namespace BusinessRegister.Dal.Models.Enums
{
    /// <summary>
    /// Company status
    /// </summary>
    public enum CompanyStatus
    {
        /// <summary>
        /// Company is still activly registered
        /// </summary>
        [XmlEnum("R")]
        Registered,

        /// <summary>
        /// Company is liquidated
        /// </summary>
        [XmlEnum("L")]
        Liquidated,

        /// <summary>
        /// Company is in bankruptcy
        /// </summary>
        [XmlEnum("N")]
        Bankruptcy
    }
}