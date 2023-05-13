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

            var distinctProps = solutions.SelectMany(s => s.Rules).Select(r => r.Item1).Distinct().ToList();

            distinctProps = distinctProps.OrderBy(d => this.CountProps(baseMatrix, d)).ToList();

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

        private int CountProps(List<string>[,] matrix, string prop)
        {
            int count = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < i; j++)
                {
                    if (matrix[i, j].Contains(prop))
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
                    Rules = solution.Rules.Where(r => r.Item1 != prop).ToList()
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
