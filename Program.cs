// See https://aka.ms/new-console-template for more information
using minimalRules;

Console.WriteLine("Hello, World!");




var path = "C:\\Projects\\TravelAgencySE\\DataBase.pl";

var reader = new PrologReader();

reader.ReadFile(path);

var matrix = new MatrixBuilder().Build(reader.solutions);

Console.WriteLine(matrix);