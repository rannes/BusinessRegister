using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using BusinessRegister.Dal.Exceptions;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Api.Services.Helpers
{
    /// <summary>
    /// Helper for File related methods
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Last modified date for XML in Avaandmed.rik.ee in string format
        /// </summary>
        /// <param name="fileName">fileName to check</param>
        /// <returns>Last modified date in raw string format</returns>
        public static string XmlRawLastModifiedDate(string fileName)
        {
            var webClient = new WebClient();
            var page = webClient.DownloadString("http://avaandmed.rik.ee/andmed/ARIREGISTER/");

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            var xmlFileData = doc.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                .Where(tr => tr.Contains(fileName))
                .ToList();

            if (xmlFileData.Count > 0 && xmlFileData[0].Count > 3 && !string.IsNullOrWhiteSpace(xmlFileData[0][2]))
                return xmlFileData[0][2];
            else
                return string.Empty;
        }

        /// <summary>
        /// Download file to: ExeLocation\Downloads\fileName
        /// </summary>
        /// <param name="url">URL where to download the XML</param>
        /// <param name="fileName">File name what to download it as.</param>
        /// <returns>File location where it was downloaded</returns>
        public static string DownloadFile(string url, string fileName)
        {
            var downloadsFolder = AppContext.BaseDirectory + "Downloads" + Path.DirectorySeparatorChar;

            if (!Directory.Exists(downloadsFolder))
                Directory.CreateDirectory(downloadsFolder);

            var fileLocation = Path.Combine(downloadsFolder, fileName);
                
            if (File.Exists(fileLocation))
                File.Delete(fileLocation);

            using (var wc = new WebClient())
            {
                wc.DownloadFile(url, fileLocation);
            }

            return fileLocation;
        }

        /// <summary>
        /// Extract zip file that contains only single file. Otherwise 
        /// </summary>
        /// <param name="zipFileLocation"></param>
        /// <returns></returns>
        public static string ExtractFile(string zipFileLocation)
        {
            if (string.IsNullOrWhiteSpace(zipFileLocation))
                throw new BrArgumentException("Zip file location is empty.", ResultCode.ZipFileLocationInvalid);

            if (Path.GetExtension(zipFileLocation) != ".zip")
                throw new BrArgumentException("File extension is not Zip", ResultCode.FileExtensionMustBeZip);

            var directoryPath = Path.GetDirectoryName(zipFileLocation);

            using (var archive = ZipFile.OpenRead(zipFileLocation))
            {
                if (archive.Entries == null)
                    throw new BrArgumentException("Archive contains no files", ResultCode.ZipFileDoesNotCotainAnyFiles);

                foreach (var zipArchiveEntry in archive.Entries)
                {
                    if (zipArchiveEntry.FullName.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase)
                        && zipArchiveEntry.FullName.Contains("ettevotja_rekvisiidid"))
                    {
                        var fileName = Path.Combine(directoryPath, zipArchiveEntry.FullName);

                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        zipArchiveEntry.ExtractToFile(fileName);
                        return fileName;
                    }
                }

                throw new BrArgumentException("Zip file did not contain correct file.", ResultCode.ZipFileDidNotContainCorrectFile);
            }
        }
    }
}