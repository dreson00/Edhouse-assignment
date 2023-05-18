using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegularForestLibrary;

var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddSingleton<RegularForest>();
}).Build();

var forest = host.Services.GetRequiredService<RegularForest>();
string? path = null;

while (CheckPath(path) is false)
{
    Console.WriteLine("Input path to file: ");
    path = Console.ReadLine();
}

Console.WriteLine(forest.CountVisibleTreesInFile(path!));

static bool CheckPath(string? filePath) => string.IsNullOrEmpty(filePath) is false && File.Exists(filePath);