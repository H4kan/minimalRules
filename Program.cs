
using minimalRules;




var path = "C:\\Users\\rafci\\Desktop\\TravelAgencySE\\DataBase.pl";
var newPath = "C:\\Users\\rafci\\Desktop\\TravelAgencySE\\DataBase1.pl";

if(File.Exists(newPath))
{
    System.IO.File.Delete(path);
    System.IO.File.Move(newPath, path);
    System.IO.File.Delete(newPath);
}

var reader = new PrologReader();

reader.ReadFile(path);

var ruleTrimmer = new RuleTrimmer();

var unnecessaryRules = ruleTrimmer.TrimRules(reader.solutions, out var trimmedSolutions);


System.IO.File.Move(path, newPath);


var writer = new DataBaseWriter();

writer.WriteDatabase(unnecessaryRules, path, newPath);




