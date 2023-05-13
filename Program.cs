// See https://aka.ms/new-console-template for more information
using minimalRules;

Console.WriteLine("Hello, World!");




var path = "C:\\Users\\rafci\\Desktop\\TravelAgencySE\\DataBase.pl";

var reader = new PrologReader();

reader.ReadFile(path);

var ruleTrimmer = new RuleTrimmer();

var unnecessaryRules = ruleTrimmer.TrimRules(reader.solutions, out var trimmedSolutions);

var newPath = "C:\\Users\\rafci\\Desktop\\TravelAgencySE\\DataBase1.pl";
System.IO.File.Move(path, newPath);


var writer = new DataBaseWriter();

writer.WriteDatabase(unnecessaryRules, path, newPath);


System.IO.File.Delete(path);
System.IO.File.Move(newPath,path);
System.IO.File.Delete(newPath);

