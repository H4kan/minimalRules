using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    internal class DataBaseWriter
    {
        public void WriteDatabase(List<Solution> solutions, string WritePath, string ReadPath)
        {
            string[] lines = File.ReadAllLines(ReadPath);
            using (StreamWriter writer = new StreamWriter(WritePath))
            {
                int it = 0;

                CopyUnchangedData(lines, ref it, writer, out bool EOF);
                if (EOF) return;
                while (WriteSolution(lines, ref it, writer, solutions)) { };
                it++;

                writer.WriteLine("solution(unknown).");

                CopyUnchangedData(lines, ref it, writer, out _);
            }

        }
       
        private int CopyUnchangedData(string[] lines, ref int it, StreamWriter writer, out bool EOF)
        {
            EOF = false;
            while (lines.Length > it && !lines[it].StartsWith("solution("))
            {
                writer.WriteLine(lines[it]);

                it++;
            }
            if (lines.Length == it) EOF = true;
            return it;
        }

        private bool WriteSolution(string[] lines,ref int it, StreamWriter writer, List<Solution> solutions)
        {
            string solName;

            while (string.IsNullOrWhiteSpace(lines[it]) || !lines[it].StartsWith("solution("))
            {
                it++;  
            }
            solName = PrologReader.GetSolutionName(lines[it]);
            if (solName == "unknown")
            {
                return false;
            }
            it++;

            var targetSolution = solutions.Where(s => s.Name == solName).FirstOrDefault();

            if (targetSolution != null)
            {
                writer.WriteLine(ConstructSolutionHeader(targetSolution));

                int ruleIt = 0;
                targetSolution.Rules.ForEach(r =>
                {
                    writer.WriteLine(this.ConstructRuleLine(r, ref ruleIt));
                });
                writer.WriteLine(ConstructPostPoneLine(ruleIt));

                writer.WriteLine();
            }
            return true;
        }

        private string ConstructSolutionHeader(Solution solution)
        {
            var sb = new StringBuilder();
            sb.Append($"solution({solution.Name}, R) :- ");

            var rRules = solution.Rules.Where(r => r.IsRRule).ToList();

            for (int i = 1; i <= rRules.Count; i++)
            {
                sb.Append($"{rRules[i - 1].Name}(X{i}),");
            }

            return sb.ToString();
        }
        private string ConstructRuleLine(Rule rule, ref int ruleIt)

        {
            return $"  {rule.InFileName}({ConstructRuleValue(rule, ref ruleIt)}),";
        }

        private string ConstructPostPoneLine(int ruleIt)
        {
            if (ruleIt == 0)
            {
                return "  R is 1.";
            }
            var sb = new StringBuilder();
            sb.Append("  R is");
            for (int i = 1; i <= ruleIt - 1; i++)
            {
                sb.Append($" R{i} *");
            }
            sb.Append($" R{ruleIt}.");
            return sb.ToString();
        }

        private string ConstructRuleValue(Rule rule, ref int ruleIt)
        {
            if (rule.IsRRule)
            {
                ruleIt++;
                return $"R{ruleIt}, X{ruleIt}";
            }
            return rule.Value;
        }
    }
}