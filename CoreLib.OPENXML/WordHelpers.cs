#region

using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

#endregion

namespace CoreLib.OPENXML
{
    public static class WordHelpers
    {
        public static void FindAndReplaceTextInWordDocument(string docFullPath,
            Dictionary<string, string> findReplaceDictionary)
        {
            if(findReplaceDictionary.Count==0)
                return;
            using (var wordDoc = WordprocessingDocument.Open(
                docFullPath,
                true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var elements = new List<OpenXmlCompositeElement>();
                elements.AddRange(body.Elements<Paragraph>());
                elements.AddRange(body.Elements<Table>());
                foreach (var e in elements)
                foreach (var de in findReplaceDictionary)
                    ReplaceText(e, de.Key, de.Value);
            }
        }

        public static void FindAndSetBoldTextInWordDocument(string docFullPath, IEnumerable<string> findText)
        {
            if(!findText.Any())
                return;
            using (var wordDoc = WordprocessingDocument.Open(
                docFullPath,
                true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var elements = new List<OpenXmlCompositeElement>();
                elements.AddRange(body.Elements<Paragraph>());
                elements.AddRange(body.Elements<Table>());
                foreach (var e in elements)
                foreach (var t in findText)
                    SetBoldText(e, t);
            }
        }

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

        private static void ReplaceText(OpenXmlCompositeElement element, string find, string replaceWith)
        {
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
                            lines[0] = txt.Text.Substring(0, c) + lines[0];
                        if (match.EndCharIndex + 1 < texts.ElementAt(match.EndElementIndex).Text.Length)
                            lines[lines.Length - 1] = lines[lines.Length - 1] + texts.ElementAt(match.EndElementIndex)
                                                          .Text.Substring(match.EndCharIndex + 1);
                        txt.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues
                            .Preserve);
                        txt.Text = lines[0];
                        for (var i = t + 1; i <= match.EndElementIndex; i++)
                            texts.ElementAt(i).Text = string.Empty;
                        if (lines.Count() > 1)
                        {
                            OpenXmlElement currEl = txt;
                            var run = txt.Parent as Run;
                            for (var i = 1; i < lines.Count(); i++)
                            {
                                var br = new Break();
                                run.InsertAfter(br, currEl);
                                currEl = br;
                                txt = new Text(lines[i]);
                                run.InsertAfter(txt, currEl);
                                t++;
                                currEl = txt;
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

        private static Match IsMatch(IEnumerable<Text> texts, int t, int c, string find)
        {
            var ix = 0;
            for (var i = t; i < texts.Count(); i++)
            {
                for (var j = c; j < texts.ElementAt(i).Text.Length; j++)
                {
                    if (find[ix] != texts.ElementAt(i).Text[j])
                        return null;
                    ix++;
                    if (ix == find.Length)
                        return new Match {EndElementIndex = i, EndCharIndex = j};
                }

                c = 0;
            }

            return null;
        }

        private class Match
        {
            public int EndElementIndex { get; set; }

            public int EndCharIndex { get; set; }
        }
    }
}