using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    public class RuleTrimmer
    {

        private MatrixBuilder builder;

        public RuleTrimmer() { 
            this.builder = new MatrixBuilder();
        }


        public List<string> TrimRules(List<Solution> solutions, out List<Solution> trimmedSolutions)
        {
            var unnecessaryRules = new List<string>();

            var baseMatrix = this.builder.Build(solutions);

            var distinctRules = solutions.SelectMany(s => s.Rules).GroupBy(r => r.Name).Select(g => g.First()).ToList();

            var distinctProps = distinctRules.OrderBy(d => this.CountProps(baseMatrix, d)).Select(r => r.Name).ToList();

            var handlerSolution = solutions.Select(s => s).ToList();
            

            foreach (var prop in distinctProps)
            {
                var excludedSolution = this.ExcludeProp(handlerSolution, prop);

                var currentmatrix = this.builder.Build(excludedSolution);

                if (IsMatrixHealthy(currentmatrix))
                {
                    unnecessaryRules.Add(prop);
                    handlerSolution = excludedSolution;
                }
            }
            trimmedSolutions = handlerSolution;

            return unnecessaryRules;
        }

        private int CountProps(List<string>[,] matrix, Rule rule)
        {
            int count = rule.IsRRule ? -1000 : 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < i; j++)
                {
                    if (matrix[i, j].Contains(rule.Name))
                    {
                        count++;
                    }
                }

            return count;
        }

        private List<Solution> ExcludeProp(List<Solution> solutions, string prop)
        {
            var excludeSolutions = new List<Solution>();

            foreach (var solution in solutions)
            {
                var newSolution = new Solution()
                {
                    Name = solution.Name,
                    Rules = solution.Rules.Where(r => r.Name != prop).ToList()
                };
                excludeSolutions.Add(newSolution);
            }
            return excludeSolutions;
        }

        private bool IsMatrixHealthy(List<string>[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (matrix[i,j].Count ==0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
