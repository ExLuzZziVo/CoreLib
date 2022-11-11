#region

using System;
using System.Collections.Generic;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

#endregion

namespace CoreLib.OPENXML
{
    // https://stackoverflow.com/a/29904887
    public static class WordHelpers
    {
        /// <summary>
        /// Finds and replaces all occurrences of text in the specified OpenXML Word document using provided dictionary
        /// </summary>
        /// <param name="docFullPath">Full path to OpenXML Word document</param>
        /// <param name="findReplaceDictionary">Dictionary, where the key is the text to be replaced with a value</param>
        public static void FindAndReplaceTextInWordDocument(string docFullPath,
            IDictionary<string, string> findReplaceDictionary)
        {
            if (findReplaceDictionary.Count == 0)
            {
                return;
            }

            using (var wordDoc = WordprocessingDocument.Open(
                docFullPath,
                true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var elements = new List<OpenXmlCompositeElement>();
                elements.AddRange(body.Elements<Paragraph>());
                elements.AddRange(body.Elements<Table>());

                foreach (var e in elements)
                {
                    foreach (var de in findReplaceDictionary)
                    {
                        ReplaceText(e, de.Key, de.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Finds and bolds all occurrences of text in the specified OpenXML Word document using provided sequence
        /// </summary>
        /// <param name="docFullPath">Full path to OpenXML Word document</param>
        /// <param name="findText">Sequence with text values to be bolded</param>
        public static void FindAndSetBoldTextInWordDocument(string docFullPath, IEnumerable<string> findText)
        {
            if (!findText.Any())
            {
                return;
            }

            using (var wordDoc = WordprocessingDocument.Open(
                docFullPath,
                true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var elements = new List<OpenXmlCompositeElement>();
                elements.AddRange(body.Elements<Paragraph>());
                elements.AddRange(body.Elements<Table>());

                foreach (var e in elements)
                {
                    foreach (var t in findText)
                    {
                        SetBoldText(e, t);
                    }
                }
            }
        }

        /// <summary>
        /// Bolds the specified text in the target element
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="text">Text to be bolded</param>
        private static void SetBoldText(OpenXmlCompositeElement element, string text)
        {
            foreach (var run in element.Descendants<Run>())
            {
                var txt = run.GetFirstChild<Text>();

                if (txt != null)
                {
                    if (txt.Text.Contains(text) && txt.Text != text)
                    {
                        var indexOfText = txt.Text.IndexOf(text);
                        var runBefore = (Run) run.Clone();
                        var runBeforeText = runBefore.GetFirstChild<Text>();
                        runBeforeText.Space = SpaceProcessingModeValues.Preserve;
                        runBeforeText.Text = txt.Text.Substring(0, indexOfText == 0 ? 0 : indexOfText);
                        var runAfter = (Run) run.Clone();
                        var runAfterText = runAfter.GetFirstChild<Text>();
                        runAfterText.Space = SpaceProcessingModeValues.Preserve;
                        runAfterText.Text = txt.Text.Substring(indexOfText + text.Length);
                        run.Parent.InsertBefore(runBefore, run);
                        run.Parent.InsertAfter(runAfter, run);

                        txt.Text = text;
                        run.RunProperties.Bold = new Bold();
                    }
                    else if (txt.Text == text)
                    {
                        run.RunProperties.Bold = new Bold();
                    }
                }
            }
        }

        /// <summary>
        /// Finds and replaces text in the target element
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="find">Text to find and replace</param>
        /// <param name="replaceWith">Text to replace with</param>
        private static void ReplaceText(OpenXmlCompositeElement element, string find, string replaceWith)
        {
            if (find.IsNullOrEmptyOrWhiteSpace())
            {
                return;
            }

            if (replaceWith == null)
            {
                replaceWith = string.Empty;
            }

            var texts = element.Descendants<Text>();

            for (var t = 0; t < texts.Count(); t++)
            {
                var txt = texts.ElementAt(t);

                for (var c = 0; c < txt.Text.Length; c++)
                {
                    var match = IsMatch(texts, t, c, find);

                    if (match != null)
                    {
                        // now replace the text
                        var lines = replaceWith.Replace(Environment.NewLine, "\r")
                            .Split('\n', '\r');

                        var skip = lines[lines.Length - 1].Length - 1;

                        if (c > 0)
                        {
                            lines[0] = txt.Text.Substring(0, c) + lines[0];
                        }

                        if (match.EndCharIndex + 1 < texts.ElementAt(match.EndElementIndex).Text.Length)
                        {
                            lines[lines.Length - 1] = lines[lines.Length - 1] + texts.ElementAt(match.EndElementIndex)
                                .Text.Substring(match.EndCharIndex + 1);
                        }

                        txt.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues
                            .Preserve);

                        txt.Text = lines[0];

                        for (var i = t + 1; i <= match.EndElementIndex; i++)
                        {
                            texts.ElementAt(i).Text = string.Empty;
                        }

                        if (replaceWith.IsNullOrEmptyOrWhiteSpace())
                        {
                            txt.Parent.Remove();
                        }

                        if (lines.Count() > 1)
                        {
                            OpenXmlElement currEl = txt;
                            var run = currEl.Parent as Run;
                            var sampleRun = (Run) run.Clone();
                            var sampleRunProperties = (RunProperties) sampleRun.GetFirstChild<RunProperties>().Clone();
                            var sampleTxt = (Text) sampleRun.GetFirstChild<Text>().Clone();
                            var paragraph = run.Parent as Paragraph;
                            var sampleParagraph = (Paragraph) paragraph.Clone();
                            sampleParagraph.RemoveAllChildren<Run>();
                            currEl = paragraph;

                            for (var i = 1; i < lines.Count(); i++)
                            {
                                var paragraphBefore = currEl;
                                var paragraphAfter = (Paragraph) sampleParagraph.Clone();
                                var newRun = new Run();
                                var newRunProperties = (RunProperties) sampleRunProperties.Clone();
                                var newTxt = (Text) sampleTxt.Clone();
                                newTxt.Space = SpaceProcessingModeValues.Preserve;
                                newTxt.Text = lines[i];
                                paragraphAfter.InsertAfter(newRun, paragraphAfter.LastChild);
                                newRun.InsertAt(newRunProperties, 0);
                                newRun.InsertAt(newTxt, 1);
                                paragraphBefore.InsertAfterSelf(paragraphAfter);
                                currEl = paragraphAfter;
                            }

                            c = skip;
                        }
                        else
                        {
                            c += skip;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determine if the <see cref="Text"/> elements (starting at element t, char c) exactly contain the find text
        /// </summary>
        /// <param name="texts"><see cref="Text"/> elements to search in</param>
        /// <param name="t">Current <see cref="Text"/> element index</param>
        /// <param name="c">Current index of character of <paramref name="t"/></param>
        /// <param name="find">Text to find</param>
        /// <returns>Search result</returns>
        private static Match IsMatch(IEnumerable<Text> texts, int t, int c, string find)
        {
            var ix = 0;

            for (var i = t; i < texts.Count(); i++)
            {
                for (var j = c; j < texts.ElementAt(i).Text.Length; j++)
                {
                    if (find[ix] != texts.ElementAt(i).Text[j])
                    {
                        return null;
                    }

                    ix++;

                    if (ix == find.Length)
                    {
                        return new Match {EndElementIndex = i, EndCharIndex = j};
                    }
                }

                c = 0;
            }

            return null;
        }

        /// <summary>
        /// Defines a match result
        /// </summary>
        private class Match
        {
            /// <summary>
            /// Last matching element index containing part of the search text
            /// </summary>
            public int EndElementIndex { get; set; }

            /// <summary>
            /// Last matching char index of the search text in last matching element
            /// </summary>
            public int EndCharIndex { get; set; }
        }
    }
}