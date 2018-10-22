using System.Xml.Serialization;

namespace BusinessRegister.Dal.Models
{
    /// <summary>
    /// Company address node
    /// </summary>
    public class CompanyAddress
    {
        [XmlElement("ads_normaliseeritud_taisaadress")]
        public string FullAddress { get; set; }
    }
}