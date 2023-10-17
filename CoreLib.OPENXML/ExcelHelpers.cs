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

namespace CoreLib.OPENXML
{
    public static class ExcelHelpers
    {
        /// <summary>
        /// Gets all cells from the specified row
        /// </summary>
        /// <param name="row">Target row</param>
        /// <returns>A sequence of cells in a specified row</returns>
        public static IEnumerable<Cell> GetRowCells(this Row row)
        {
            var currentCount = 0;

            foreach (var cell in
                     row.Descendants<Cell>())
            {
                var columnName = GetColumnName(cell.CellReference);

                var currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    yield return new Cell();
                }

                yield return cell;

                currentCount++;
            }
        }

        /// <summary>
        /// Gets the name of the column using the specified cell reference
        /// </summary>
        /// <param name="cellReference">Cell reference</param>
        /// <returns>Cell column name</returns>
        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Converts a column name to a number
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Column number</returns>
        private static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");

            if (!alpha.IsMatch(columnName))
            {
                throw new ArgumentException();
            }

            var colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            return colLetters.Select((letter, i) => i == 0 ? letter - 65 : letter - 64)
                .Select((current, i) => current * (int)Math.Pow(26, i)).Sum();
        }

        /// <summary>
        /// Creates new <see cref="Cell"/>
        /// </summary>
        /// <param name="value">Cell value</param>
        /// <param name="dataType">Value data type</param>
        /// <param name="styleIndex">Cell style index. Default value: 0</param>
        /// <returns>New <see cref="Cell"/></returns>
        public static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        /// <summary>
        /// Tries to automatically resize columns on a sheet
        /// </summary>
        /// <param name="sheetData">Sheet to resize</param>
        /// <returns>Resized columns</returns>
        public static Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);
            var columns = new Columns();
            const double maxWidth = 4D;

            foreach (var item in maxColWidth)
            {
                var width = Math.Truncate((item.Value * maxWidth + 16) / maxWidth * 256) / 256;

                var col = new Column
                {
                    BestFit = true,
                    Min = (uint)(item.Key + 1),
                    Max = (uint)(item.Key + 1),
                    CustomWidth = true,
                    Width = width
                };

                columns.Append(col);
            }

            return columns;
        }

        /// <summary>
        /// Calculates the maximum text size of a cell in each row
        /// </summary>
        /// <param name="sheetData">A sheet to get column sizes</param>
        /// <returns>A dictionary where the key is the column index and the value is the calculated text size</returns>
        private static Dictionary<int, int> GetMaxCharacterWidth(OpenXmlElement sheetData)
        {
            var maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            uint[] numberStyles = { }; //styles that will add extra chars
            uint[] boldStyles = { 2 }; //styles that will bold

            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                for (var i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && ((IList)numberStyles).Contains(cell.StyleIndex))
                    {
                        var thousandCount = (int)Math.Truncate((double)cellTextLength / 4);
                        cellTextLength += 3 + thousandCount;
                    }

                    if (cell.StyleIndex != null && ((IList)boldStyles).Contains(cell.StyleIndex))
                    {
                        cellTextLength += 1;
                    }

                    cellTextLength += (int)Math.Round(cellValue.UppercaseCharactersCount() * 0.5, 0,
                        MidpointRounding.AwayFromZero);

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];

                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        /// <summary>
        /// Creates a default stylesheet for a spreadsheet
        /// </summary>
        /// <returns>New stylesheet with following parameters:
        /// 1. Fonts:
        ///     1) Font size: 12, Font name: Times New Roman
        ///     2) Font size: 12, Font name: Times New Roman, Font color: White, Font weight: Bold
        /// 2. Fills:
        ///     1) No fill
        ///     2) #7D7D7D
        ///     3) #666666
        /// 3. Borders:
        ///     1) No borders
        ///     2) Thin black borders
        /// 4. Cell formats:
        ///     1) Default
        ///     2) Font: 1, Fill: 1, Borders: 2, Alignment: <see cref="HorizontalAlignmentValues.Left"/>-<see cref="VerticalAlignmentValues.Center"/>
        ///     3) Font: 2, Fill: 3, Borders: 2, Alignment: <see cref="HorizontalAlignmentValues.Left"/>-<see cref="VerticalAlignmentValues.Center"/>
        ///     4) Font: 1, Fill: 1, Borders: 1, Alignment: <see cref="HorizontalAlignmentValues.Center"/>-<see cref="VerticalAlignmentValues.Center"/>
        ///     5) Font: 1, Fill: 1, Borders: 2, Alignment: <see cref="HorizontalAlignmentValues.Center"/>-<see cref="VerticalAlignmentValues.Center"/>
        ///     6) Font: 1, Fill: 1, Borders: 2, Alignment: <see cref="HorizontalAlignmentValues.Left"/>-<see cref="VerticalAlignmentValues.Center"/>
        /// </returns>
        public static Stylesheet GenerateStylesheet()
        {
            var fonts = new Fonts(
                new Font(
                    new FontSize { Val = 12 },
                    new FontName { Val = "Times New Roman" }
                ),
                new Font(
                    new FontSize { Val = 12 },
                    new Bold(),
                    new Color { Rgb = "FFFFFF" },
                    new FontName { Val = "Times New Roman" }
                ));

            var fills = new Fills(
                new Fill(new PatternFill { PatternType = PatternValues.None }),
                new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),
                new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue { Value = "66666666" } })
                    { PatternType = PatternValues.Solid })
            );

            var borders = new Borders(
                new Border(),
                new Border(
                    new LeftBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

            var cellFormats = new CellFormats(
                new CellFormat(),
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 1,
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center,
                        Indent = 1
                    },
                    ApplyAlignment = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 1,
                    FillId = 2,
                    BorderId = 1,
                    Alignment = new Alignment
                    {
                        Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center,
                        Indent = 1
                    },
                    ApplyAlignment = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0,
                    Alignment = new Alignment
                        { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center },
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
                        { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center },
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
                    {
                        Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center,
                        Indent = 1
                    },
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = true,
                    ApplyFont = true
                }
            );

            var styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        /// <summary>
        /// Gets the value of the specified cell
        /// </summary>
        /// <param name="cell">Target cell</param>
        /// <param name="sst">Table of string values of cells</param>
        /// <returns>The string value of the specified cell</returns>
        public static string GetCellValue(this Cell cell, SharedStringTable sst)
        {
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var ssid = int.Parse(cell.CellValue.Text);

                return sst.ChildElements[ssid].InnerText.TrimWholeString();
            }

            return cell.CellValue != null ? cell.CellValue.Text.TrimWholeString() : string.Empty;
        }

        /// <summary>
        /// Gets the name of a column by its index
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns>The name of the column</returns>
        public static string GetCellColumnName(int columnIndex)
        {
            var dividend = columnIndex;
            var columnName = string.Empty;

            while (dividend > 0)
            {
                var modulo = dividend % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
    }
}