using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    public class MatrixBuilder
    {
        private List<string>[,] matrix;
        private string[] rowNames;
        public MatrixBuilder() { }

        public List<string>[,] Build(List<Solution> solutions) {
            
            matrix = new List<string>[solutions.Count, solutions.Count];
            rowNames = solutions.Select(x => x.Name).ToArray();

            for (int i = 0; i < solutions.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    matrix[i, j] = GetSolutionDiscrimination(solutions[i], solutions[j]);
                }
            }
            return matrix;
        }

        private List<string> GetSolutionDiscrimination(Solution solution1, Solution solution2)
        {
            var discrimination = new List<string>();
            foreach (var rule in solution1.Rules)
            {
                if (solution2.Rules.Any(r => r.Item1 == rule.Item1))
                {
                    var matchingRule = solution2.Rules.First(r => r.Item1 == rule.Item1);

                    if (matchingRule.Item2 != rule.Item2)
                    {
                        discrimination.Add(rule.Item1);
                    }
                }
                else if (!rule.Item2.EndsWith("1"))
                {
                    discrimination.Add(rule.Item1);
                }
            }

            foreach (var rule in solution2.Rules)
            {
                if (!solution1.Rules.Any(r => r.Item1 == rule.Item1) && !rule.Item2.EndsWith("1"))
                {
                    discrimination.Add(rule.Item1);
                }
     
            }
            return discrimination;
        }
    }
}
