using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LogParser
{
    public static class LogParser
    {
        private static readonly string[] X =
        {
            "thermometer", "humidity", "monoxide"
        };


        public static string Parse(string content)
        {
            using var sr = new StringReader(content);
            var info = GetReferenceValues(sr.ReadLine());
            var result = new Dictionary<string, string>();

            (string, string) active = (null, null);
            var value = new List<double>();
            var line = "";
            while ((line = sr.ReadLine()) != null)
            {
                var parts = line.Split(' ');

                if (X.Contains(parts[0]))
                {
                    if (active.Item1 != null)
                    {
                        var deviation = CalculateStdDev(value);
                        if (active.Item1 == "thermometer")
                        {
                            result.Add(active.Item2, GetThermoValue(value.Average(), deviation, info["thermometer"]));
                        }
                        else if (active.Item1 == "humidity")
                        {
                            var reference = info["humidity"];
                            var text = value.All(o => o >= (reference - 1) && o <= (reference + 1))
                                ? "keep"
                                : "discard";
                            result.Add(active.Item2, text);
                        }
                        else
                        {
                            var reference = info["monoxide"];
                            var text = value.All(o => o >= (reference - 3) && o <= (reference + 3))
                                ? "keep"
                                : "discard";
                            result.Add(active.Item2, text);
                        }
                    }

                    active = (parts[0], parts[1]);
                    value.Clear();
                }
                else
                {
                    value.Add(double.Parse(parts[1], CultureInfo.InvariantCulture));
                }
            }

            if (active.Item1 == "thermometer")
            {
                result.Add(active.Item2, GetThermoValue(value.Average(), CalculateStdDev(value), info["thermometer"]));
            }
            else if (active.Item1 == "humidity")
            {
                var reference = info["humidity"];
                var text = value.All(o => o >= (reference - 1) && o <= (reference + 1))
                    ? "keep"
                    : "discard";
                result.Add(active.Item2, text);
            }
            else
            {
                var reference = info["monoxide"];
                var text = value.All(o => o >= (reference - 3) && o <= (reference + 3))
                    ? "keep"
                    : "discard";
                result.Add(active.Item2, text);
            }

            return JsonConvert.SerializeObject(result);
        }

        private static Dictionary<string, double> GetReferenceValues(string line)
        {
            var x = line.Split(' ');

            return new Dictionary<string, double>
            {
                {"thermometer", double.Parse(x[1], CultureInfo.InvariantCulture)},
                {"humidity", double.Parse(x[2], CultureInfo.InvariantCulture)},
                {"monoxide", double.Parse(x[3], CultureInfo.InvariantCulture)},
            };
        }

        private static string GetThermoValue(double average, double deviation, double reference)
        {
            var isMean = average <= (reference + 0.5) && average >= (reference - 0.5);

            if (deviation < 3 && isMean)
            {
                return "ultra precise";
            }

            if (deviation < 5 && isMean)
            {
                return "very precise";
            }

            return "precise";
        }

        private static double CalculateStdDev(IEnumerable<double> values)
        {
            var ret = 0.0;
            var enumerable = values.ToList();
            if (!enumerable.Any())
            {
                return ret;
            }

            var avg = enumerable.Average();
            var sum = enumerable.Sum(d => Math.Pow(d - avg, 2));
            ret = Math.Sqrt((sum) / (enumerable.Count() - 1));

            return ret;
        }
    }
}
