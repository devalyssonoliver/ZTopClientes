using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;

namespace ZeusTopClientes
{
    public partial class App : Application
    {
        private const string IniFilePath = "ZTopclientes.ini";
        private const string Usuario = "postgres";
        private const string Password = "postzeus2011";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!File.Exists(IniFilePath))
            {
                // Se o arquivo INI não existir, abra a tela de configuração
                ConfigIni configWindow = new ConfigIni();
                configWindow.Show();  // Mostra a tela de configuração
            }
            else
            {
                var iniManager = new IniManager(IniFilePath);
                var config = iniManager.ReadIniFile();

                if (config.Count == 0 || !IsIniComplete(config))
                {
                    ConfigIni configWindow = new ConfigIni();
                    configWindow.Show();  // Abra a tela de configuração
                }
                else
                {
                    // Se o arquivo INI está completo, abra a tela de login
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();  // Mostra a tela de login
                }
            }
        }

        private bool IsIniComplete(Dictionary<string, string> config)
        {
            // Verifique se todas as chaves necessárias estão presentes
            return config.ContainsKey("Host") &&
                   config.ContainsKey("Port") &&
                   config.ContainsKey("Database") &&
                   config.ContainsKey(Usuario) &&
                   config.ContainsKey(Password);
        }
    }
}
