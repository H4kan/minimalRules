
using minimalRules;




var path = "C:\\Users\\Szymon\\Desktop\\msi-projects\\TravelAgencySE\\DataBase.pl";
var newPath = "C:\\Users\\Szymon\\Desktop\\msi-projects\\TravelAgencySE\\DataBase1.pl";

if(File.Exists(newPath))
{
    System.IO.File.Delete(newPath);
}

var reader = new PrologReader();

reader.ReadFile(path);

var ruleTrimmer = new RuleTrimmer();

_ = ruleTrimmer.TrimRules(reader.solutions, out var trimmedSolutions);


var writer = new DataBaseWriter();

writer.WriteDatabase(reader.solutions, newPath, path);




