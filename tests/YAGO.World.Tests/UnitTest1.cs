using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using Xunit;

namespace YAGO.World.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Test02()
        {
            string svgContent = File.ReadAllText("C:\\Всякое\\YAGO\\ПСК\\MapPlio\\mapСwithotJpg.svg");
            string geoJson = ConvertSvgPathsToGeoJson(svgContent);
            File.WriteAllText("C:\\Всякое\\YAGO\\ПСК\\MapPlio\\output.geojson", geoJson);
        }

        public static string ConvertSvgPathsToGeoJson(string svgContent)
        {
            var doc = new XmlDocument();
            doc.LoadXml(svgContent);

            var features = new List<string>();
            var nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("svg", "http://www.w3.org/2000/svg");

            foreach (XmlNode pathNode in doc.SelectNodes("//svg:path", nsManager))
            {
                var id = pathNode.Attributes?["id"]?.Value ?? "unknown";
                var d = pathNode.Attributes?["d"]?.Value;

                if (!string.IsNullOrEmpty(d))
                {
                    var coordinates = ParseSvgPath(d);
                    if (coordinates.Count > 0)
                    {
                        // Замыкаем полигон (если еще не замкнут)
                        if (coordinates[0] != coordinates[coordinates.Count - 1])
                        {
                            coordinates.Add(coordinates[0]);
                        }

                        var feature = $@"
                    {{
                        ""type"": ""Feature"",
                        ""properties"": {{
                            ""id"": ""{id}"",
                            ""class"": ""{pathNode.Attributes?["class"]?.Value ?? ""}""
                        }},
                        ""geometry"": {{
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    {FormatCoordinates(coordinates)}
                                ]
                            ]
                        }}
                    }}";
                        features.Add(feature);
                    }
                }
            }

            return $@"
        {{
            ""type"": ""FeatureCollection"",
            ""name"": ""entities"",
            ""features"": [
                {string.Join(",", features)}
            ]
        }}";
        }

        private static List<decimal[]> ParseSvgPath(string pathData)
        {
            var coordinates = new List<decimal[]>();
            var currentPoint = new decimal[2];
            var startPoint = new decimal[2];
            var isRelative = false;
            var lastCommand = ' ';

            // Улучшенный regex для обработки отрицательных чисел в командах
            var matches = Regex.Matches(pathData, @"([MLHVCSQTAZmlhvcsqtaz]|(?<=[0-9])-)([^MLHVCSQTAZmlhvcsqtaz]*)");

            foreach (Match match in matches)
            {
                var commandStr = match.Groups[1].Value;
                var parameters = match.Groups[2].Value.Trim();

                // Обработка отрицательных чисел
                char command = commandStr[0];
                if (command == '-' && char.IsLetter(lastCommand))
                {
                    parameters = commandStr + parameters;
                    command = lastCommand;
                }

                // Парсинг чисел с заменой точки на запятую для локали
                var numbers = new List<decimal>();
                var numberMatches = Regex.Matches(parameters, @"-?\d+\.?\d*");
                foreach (Match numMatch in numberMatches)
                {
                    if (decimal.TryParse(numMatch.Value.Replace('.', ','), out decimal num))
                        numbers.Add(num);
                }

                // Обработка всех команд
                switch (char.ToUpper(command))
                {
                    case 'M': // MoveTo
                        if (numbers.Count < 2) break;
                        isRelative = char.IsLower(command);
                        currentPoint[0] = isRelative ? currentPoint[0] + numbers[0] : numbers[0];
                        currentPoint[1] = isRelative ? currentPoint[1] + numbers[1] : numbers[1];
                        startPoint[0] = currentPoint[0];
                        startPoint[1] = currentPoint[1];
                        coordinates.Add(new[] { currentPoint[0], currentPoint[1] });

                        // Неявные LineTo после MoveTo
                        for (int i = 2; i < numbers.Count; i += 2)
                        {
                            if (i + 1 >= numbers.Count) break;
                            currentPoint[0] = isRelative ? currentPoint[0] + numbers[i] : numbers[i];
                            currentPoint[1] = isRelative ? currentPoint[1] + numbers[i + 1] : numbers[i + 1];
                            coordinates.Add(new[] { currentPoint[0], currentPoint[1] });
                        }
                        break;

                    case 'L': // LineTo
                    case 'H': // Horizontal LineTo
                    case 'V': // Vertical LineTo
                        isRelative = char.IsLower(command);
                        for (int i = 0; i < numbers.Count; i++)
                        {
                            switch (char.ToUpper(command))
                            {
                                case 'H':
                                    currentPoint[0] = isRelative ? currentPoint[0] + numbers[i] : numbers[i];
                                    break;
                                case 'V':
                                    currentPoint[1] = isRelative ? currentPoint[1] + numbers[i] : numbers[i];
                                    break;
                                default: // L
                                    if (i + 1 >= numbers.Count) break;
                                    currentPoint[0] = isRelative ? currentPoint[0] + numbers[i] : numbers[i];
                                    currentPoint[1] = isRelative ? currentPoint[1] + numbers[i + 1] : numbers[i + 1];
                                    i++; // Пропускаем второй параметр для L
                                    break;
                            }
                            coordinates.Add(new[] { currentPoint[0], currentPoint[1] });
                        }
                        break;

                    case 'Z': // ClosePath
                        if (coordinates.Count > 0)
                        {
                            coordinates.Add(new[] { startPoint[0], startPoint[1] });
                            currentPoint[0] = startPoint[0];
                            currentPoint[1] = startPoint[1];
                        }
                        break;

                        // По желанию можно добавить обработку кривых (C, S, Q, T) и дуг (A)
                }

                lastCommand = command;
            }

            // Удаляем дубликаты (если нужно)
            return coordinates.Count > 0 ? coordinates : new List<decimal[]>();
        }

        private static string FormatCoordinates(List<decimal[]> coordinates)
        {
            var sb = new StringBuilder();
            foreach (var coord in coordinates)
            {
                var x = (coord[0] - 5.125M).ToString().Replace(",", ".");
                var y = (2076M - coord[1]).ToString().Replace(",", ".");
                sb.Append($"[{x}, {y}],"); // Инвертируем Y-координату
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
