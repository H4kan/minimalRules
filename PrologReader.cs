using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    public class PrologReader
    {

        public List<Solution> solutions= new List<Solution>();

        public PrologReader() { }

        public void ReadFile(string path)
        {

            string[] lines = File.ReadAllLines(path);

            int it = 0;

            while (ReadSolution(lines, it, out int endIt))
            {
                it = endIt;
            }
        }

        private bool ReadSolution(string[] lines, int it, out int endIt)
        {
            endIt = it;
            while (!lines[it].StartsWith("solution("))
            {
                it++;
            }

            if (lines[it].StartsWith("solution(unknown)"))
                return false;

            var solName = GetSolutionName(lines[it]);
            it++;

            var activites = new List<(string, string, bool)>() { };

            while (!string.IsNullOrWhiteSpace(lines[it]) && !IsRLogicLine(lines[it]))
            {
                (string, string, bool) parsedLine;
                if (IsRRuleLine(lines[it]))
                {
                    parsedLine = ParseRLine(lines[it]);
                }
                else
                {
                    parsedLine = ParseLine(lines[it]);
                }
                if (!parsedLine.Item1.StartsWith("%"))
                {
                    activites.Add(parsedLine);
                }
                it++;
            }

            solutions.Add(new Solution()
            {
                Name = solName,
                Rules = activites.Select(a => new Rule()
                {
                    Name = a.Item1,
                    Value = a.Item2,
                    IsRRule = a.Item3
                }).ToList()
            });
            endIt = it;

            return true;
        }

        private bool IsRRuleLine(string line)
        {
            return line.Contains("R");
        }

        private (string, string, bool) ParseRLine(string line)
        {
            var handler = ParseLine(line).Item1.Split("_");
            return (handler[0], handler[1], true);
        }

        private bool IsRLogicLine(string line)
        {
            int it = 0;
            while (line[it] == ' ') it++;
            return line.Substring(it).StartsWith("R is");
        }

        public static (string, string, bool) ParseLine(string line)
        {
            int it = 0;
            while (line[it] != '(')
            {
                it++;
            }
            var rule = line.Substring(2, it - 2);
            var nextIt = it;
            while (line[nextIt] != ')')
            {
                nextIt++;
            }
            var value = line.Substring(it + 1, nextIt - it - 1);

            return (rule, value, false);
        }

        public static string GetSolutionName(string line)
        {
            var parsed = ParseLine(line);
            return parsed.Item2.Split(",")[0];
        }
    }
}
