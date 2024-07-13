using System.IO;
using System.Collections.Generic;

public class IniManager
{
    private readonly string iniFilePath;

    public IniManager(string path)
    {
        iniFilePath = path;

        // Verifique se o arquivo INI existe, caso contrário, crie um com valores padrão
        if (!File.Exists(iniFilePath))
        {
            CreateDefaultIniFile();
        }
    }

    private void CreateDefaultIniFile()
    {
        using (var writer = new StreamWriter(iniFilePath))
        {
            writer.WriteLine("[Connection]");
            writer.WriteLine("Host=localhost");
            writer.WriteLine("Port=5432");
            writer.WriteLine("Database=mydatabase");
        }
    }

    public Dictionary<string, string> ReadIniFile()
    {
        var config = new Dictionary<string, string>();

        using (var reader = new StreamReader(iniFilePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        config[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
        }

        return config;
    }
}
