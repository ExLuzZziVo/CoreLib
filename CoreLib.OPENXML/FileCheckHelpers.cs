using System;
using System.IO.Packaging;
using CoreLib.CORE.Helpers.StringHelpers;

namespace CoreLib.OPENXML
{
    public static class FileCheckHelpers
    {
        public enum DocumentType : byte
        {
            /// <summary>
            /// The file is not an OpenXML document
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// The file is a Word OpenXML document
            /// </summary>
            Word = 1,
            /// <summary>
            /// The file is a Spreadsheet OpenXML document
            /// </summary>
            Spreadsheet = 2,
            /// <summary>
            /// The file is a Presentation OpenXML document
            /// </summary>
            Presentation = 3
        }

        // https://stackoverflow.com/questions/14801852/openxml-documents-how-do-you-know-which-is-which-when-there-is-no-extention
        /// <summary>
        /// Checks if the provided file is an OpenXML document
        /// </summary>
        /// <param name="filePath">Full path to the file</param>
        /// <returns><see cref="DocumentType"/> of the provided file</returns>
        public static DocumentType CheckFileDocumentType(string filePath)
        {
            if (filePath.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            DocumentType documentType;

            try
            {
                using (var package = Package.Open(filePath))
                {
                    if (package.PartExists(new Uri("/word/document.xml", UriKind.Relative)))
                    {
                        documentType = DocumentType.Word;
                    }
                    else if (package.PartExists(new Uri("/xl/workbook.xml", UriKind.Relative)))
                    {
                        documentType = DocumentType.Spreadsheet;
                    }
                    else if (package.PartExists(new Uri("/ppt/presentation.xml", UriKind.Relative)))
                    {
                        documentType = DocumentType.Presentation;
                    }
                    else
                    {
                        documentType = DocumentType.Unknown;
                    }

                    package.Close();
                }
            }
            catch
            {
                documentType = DocumentType.Unknown;
            }

            return documentType;
        }
    }
}