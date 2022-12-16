using Contract;
using DynamicBatchRename;
using System.Reflection;

var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
var folderInfo = new DirectoryInfo(exeFolder);
var dllFiles = folderInfo.GetFiles("*.dll");

foreach (var file in dllFiles)
{
    var assembly = Assembly.LoadFrom(file.FullName);
    var types = assembly.GetTypes();

    foreach (var type in types)
    {
        if (type.IsClass && typeof(IRule).IsAssignableFrom(type)) 
        {
            IRule rule = (IRule) Activator.CreateInstance(type)!;
            RuleFactory.Register(rule);
        }
    }
}

// Tai tao lai luat tu preset
string presetPath = "HrRules.txt";
var rulesData = File.ReadAllLines(presetPath);
var rules = new List<IRule?>();

foreach (var line in rulesData)
{
    var rule = RuleFactory.Instance().Parse(line);

    if (rule != null)
    {
        rules.Add(rule);
    }
}

var filenames = new List<string>
{
    "WARD_VS_VITAL_SIGNS",
    "Lyhai      google.pdf",
    "  david------osilic  giant.pdf",
    "CV  apply______ UltraMegaCop.pdf"
};

var newFilenmes = new List<string>();

foreach (var line in filenames)
{
    string newName = line.Trim();

    foreach (var rule in rules)
    {
        newName = rule?.Rename(newName)!;
    }

    newFilenmes.Add(newName);
}

foreach (var name in newFilenmes)
{
    Console.WriteLine(name);
}