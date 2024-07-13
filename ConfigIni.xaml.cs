using System;
using System.IO;
using System.Windows;
using MahApps.Metro.Controls;

namespace ZeusTopClientes
{
    public partial class ConfigIni : MetroWindow // Herda de MetroWindow para recursos do MahApps.Metro
    {
        // Constante para o caminho do arquivo INI
        private const string IniFilePath = "ZTopclientes.ini";

        public ConfigIni()
        {
            InitializeComponent();
        }

        // Método para salvar a configuração no arquivo INI
        private void SaveConfiguration()
        {
            try
            {
                // Abre o StreamWriter para escrever no arquivo INI
                using (var writer = new StreamWriter(IniFilePath))
                {
                    writer.WriteLine("[Connection]");
                    writer.WriteLine("Host=" + txtHost.Text);
                    writer.WriteLine("Port=" + txtPort.Text);
                    writer.WriteLine("Database=" + txtDatabase.Text);
                }

                // Se a escrita foi bem-sucedida, abre a janela de login
                OpenLoginWindow();

                // Fecha a janela atual
                this.Close();
            }
            catch (Exception ex) // Captura exceções para mostrar mensagens de erro
            {
                MessageBox.Show("Erro ao salvar a configuração: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para abrir a janela de login
        private void OpenLoginWindow()
        {
            try
            {
                var loginWindow = new LoginWindow(); // Instancia a janela de login
                loginWindow.Show(); // Mostra a janela de login
            }
            catch (Exception ex) // Captura exceções ao abrir a janela
            {
                MessageBox.Show("Erro ao abrir a janela de login: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento para o clique do botão de salvar
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration(); // Salva a configuração quando o botão é clicado
        }
    }
}
