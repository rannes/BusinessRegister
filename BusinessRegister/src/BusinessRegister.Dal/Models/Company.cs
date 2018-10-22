using System.Xml.Serialization;
using BusinessRegister.Dal.Models.Enums;

namespace BusinessRegister.Dal.Models
{
    /// <summary>
    /// Company registry node
    /// </summary>
    [XmlType("ettevotja")]
    public class Company
    {
        /// <summary>
        /// Company name
        /// </summary>
        [XmlElement("nimi")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Business code of the company
        /// </summary>
        [XmlElement("ariregistri_kood")]
        public string BusinessCode { get; set; }

        /// <summary>
        /// Company VAT Reg.No
        /// </summary>
        [XmlElement("kmkr_nr")]
        public string VatNo { get; set; }

        /// <summary>
        /// Company status from <see cref="CompanyStatus"/>
        /// </summary>
        [XmlElement("ettevotja_staatus")]
        public CompanyStatus Status { get; set; }

        [XmlElement("ettevotja_aadress")]
        public CompanyAddress CompanyAddress { get; set; }

        /// <summary>
        /// Link to company in Äriregister
        /// </summary>
        [XmlElement("teabesysteemi_link")]
        public string UrlOfAriregister { get; set; }
    }
}