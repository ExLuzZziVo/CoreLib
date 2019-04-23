#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CoreLib.CORE.Helpers.StringHelpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

#endregion

namespace OpenXMLProcLib
{
    public static class ExcelHelpers
    {
        public static IEnumerable<Cell> GetRowCells(this Row row)
        {
            var currentCount = 0;

            foreach (var cell in
                row.Descendants<Cell>())
            {
                var columnName = GetColumnName(cell.CellReference);

                var currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++) yield return new Cell();

                yield return cell;
                currentCount++;
            }
        }

        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            var colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            return colLetters.Select((letter, i) => i == 0 ? letter - 65 : letter - 64)
                .Select((current, i) => current * (int) Math.Pow(26, i)).Sum();
        }

        public static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        public static Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);
            var columns = new Columns();
            const double maxWidth = 3D;
            foreach (var item in maxColWidth)
            {
                var width = Math.Truncate((item.Value * maxWidth + 9) / maxWidth * 256) / 256;
                var col = new Column
                {
                    BestFit = true,
                    Min = (uint) (item.Key + 1),
                    Max = (uint) (item.Key + 1),
                    CustomWidth = true,
                    Width = width
                };
                columns.Append(col);
            }

            return columns;
        }

        private static Dictionary<int, int> GetMaxCharacterWidth(OpenXmlElement sheetData)
        {
            var maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            uint[] numberStyles = {5, 6, 7, 8}; //styles that will add extra chars
            uint[] boldStyles = {1, 2, 3, 4, 6, 7, 8}; //styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();
                for (var i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && ((IList) numberStyles).Contains(cell.StyleIndex))
                    {
                        var thousandCount = (int) Math.Truncate((double) cellTextLength / 4);
                        cellTextLength += 3 + thousandCount;
                    }

                    if (cell.StyleIndex != null && ((IList) boldStyles).Contains(cell.StyleIndex)) cellTextLength += 1;

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current) maxColWidth[i] = cellTextLength;
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        public static Stylesheet GenerateStylesheet()
        {
            var fonts = new Fonts(
                new Font(
                    new FontSize {Val = 12},
                    new FontName {Val = "Times New Roman"}
                ),
                new Font(
                    new FontSize {Val = 12},
                    new Bold(),
                    new Color {Rgb = "FFFFFF"},
                    new FontName {Val = "Times New Roman"}
                ));

            var fills = new Fills(
                new Fill(new PatternFill {PatternType = PatternValues.None}),
                new Fill(new PatternFill {PatternType = PatternValues.Gray125}),
                new Fill(new PatternFill(new ForegroundColor {Rgb = new HexBinaryValue {Value = "66666666"}})
                    {PatternType = PatternValues.Solid})
            );

            var borders = new Borders(
                new Border(),
                new Border(
                    new LeftBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new RightBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new TopBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new BottomBorder(new Color {Auto = true}) {Style = BorderStyleValues.Thin},
                    new DiagonalBorder())
            );


            var cellFormats = new CellFormats(
                new CellFormat(),
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 1,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 1,
                    FillId = 2,
                    BorderId = 1,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0,
                    Alignment = new Alignment {Horizontal = HorizontalAlignmentValues.Center},
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 1,
                    Alignment = new Alignment
                        {Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center},
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 1,
                    Alignment = new Alignment {Vertical = VerticalAlignmentValues.Center},
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    ApplyFont = true
                }
            );

            var styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        public static string GetCellValue(this Cell cell, SharedStringTable sst)
        {
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var ssid = int.Parse(cell.CellValue.Text);
                return sst.ChildElements[ssid].InnerText.TrimWholeString();
            }

            return cell.CellValue != null ? cell.CellValue.Text.TrimWholeString() : string.Empty;
        }
    }
}