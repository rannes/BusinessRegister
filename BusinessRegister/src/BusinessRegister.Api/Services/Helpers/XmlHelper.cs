using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Api.Services.Helpers
{
    /// <summary>
    /// Xml file handler helpers
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Deserialize XML To Collection of <see cref="Company"/> objects
        /// </summary>
        /// <param name="pathToXml">Location of XML file to deserialize</param>
        /// <returns>Deserialized list of <see cref="Company"/> objects</returns>
        public static IList<Company> DeserializeXml(string pathToXml)
        {
            var serializer = new XmlSerializer(typeof(List<Company>), new XmlRootAttribute("ettevotjad"));

            using (var reader = new StreamReader(pathToXml))
            {
                return (IList<Company>) serializer.Deserialize(reader);
            }
        }
    }
}