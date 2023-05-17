
using minimalRules;




var path = "DataBase.pl";
var newPath = "DataBaseOrigin.pl";

if(File.Exists(newPath))
{
    System.IO.File.Delete(path);
    System.IO.File.Move(newPath,path);
}

var reader = new PrologReader();

reader.ReadFile(path);

var ruleTrimmer = new RuleTrimmer();

_ = ruleTrimmer.TrimRules(reader.solutions, out var trimmedSolutions);


var writer = new DataBaseWriter();

System.IO.File.Move(path, newPath);

writer.WriteDatabase(trimmedSolutions, path, newPath);




