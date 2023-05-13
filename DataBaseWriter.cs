using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimalRules
{
    internal class DataBaseWriter
    {
        public void WriteDatabase(List<string> unnecessaryRules, string WritePath, string ReadPath)
        {
            string[] lines = File.ReadAllLines(ReadPath);
            using (StreamWriter writer = new StreamWriter(WritePath))
            {
                int it = 0;

                while (CopyDataAndModifySolution(lines, ref  it,  writer, unnecessaryRules)) { };
            }

        }
        
        private bool CopyDataAndModifySolution(string[] lines, ref int it, StreamWriter writer, List<string> unnecessaryRules)
        {
            CopyUnchangedData(lines, ref it, writer, out bool EOF);

            if (EOF) return false;

            writer.WriteLine(lines[it]);
            it++;
            if (!lines[it-1].StartsWith("solution(unknown)"))
            {    
                    ModifySolution(lines,ref it, writer, unnecessaryRules);                    
                
            }
            return true;
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

        private void ModifySolution(string[] lines,ref int it, StreamWriter writer, List<string> unnecessaryRules)
        {
            string attribute, value;
            string? attributeToWrite, valueToWrite;


            (attributeToWrite, valueToWrite) = (null, null);

            while (!string.IsNullOrWhiteSpace(lines[it]))
            {
                (attribute, value) = ParseLine(lines[it]);
                if (!attribute.StartsWith("%") && !unnecessaryRules.Contains(attribute))
                {
                    if(attributeToWrite != null) writer.WriteLine($"  {attributeToWrite}({valueToWrite}),");

                    (attributeToWrite, valueToWrite) = (attribute, value);
                }
                it++;
            }
            if (attributeToWrite != null) writer.WriteLine($"  {attributeToWrite}({valueToWrite}).");
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