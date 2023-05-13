// See https://aka.ms/new-console-template for more information
using minimalRules;

Console.WriteLine("Hello, World!");




var path = "C:\\Users\\Szymon\\Desktop\\msi-projects\\TravelAgencySE\\DataBase.pl";

var reader = new PrologReader();

reader.ReadFile(path);