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

            var solName = ParseLine(lines[it]).Item2;
            it++;

            var activites = new List<(string, string)>() { };

            while (!string.IsNullOrWhiteSpace(lines[it]))
            {
                var parsedLine = ParseLine(lines[it]);
                if (!parsedLine.Item1.StartsWith("%"))
                {
                    activites.Add(ParseLine(lines[it]));
                }
                it++;
            }

            solutions.Add(new Solution()
            {
                Name = solName,
                Rules = activites
            });
            endIt = it;

            return true;
        }

        private (string, string) ParseLine(string line)
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

            return (rule, value);
        }
    }
}
