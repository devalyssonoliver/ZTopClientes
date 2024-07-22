using System;
using System.IO;
using System.Windows;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using MahApps.Metro.Controls;

namespace ZeusTopClientes
{
    public partial class LoginWindow : MetroWindow
    {
        private readonly NpgsqlConnection connection;
        private const string IniFilePath = "ZTopclientes.ini";
        private const string Username = "postgres";
        private const string Password = "pepino";

        public LoginWindow()
        {
            InitializeComponent();

            // Verifique se o arquivo INI existe e está completo
            if (!File.Exists(IniFilePath) || !IsIniComplete())
            {
                
            }

            var iniManager = new IniManager(IniFilePath);
            var config = iniManager.ReadIniFile();

            // Inicialize a conexão com o banco de dados
            string connectionString = $"Host={config["Host"]};Port={config["Port"]};Database={config["Database"]};Username={Username};Password={Password}";
            connection = new NpgsqlConnection(connectionString);
        }

        private bool IsIniComplete()
        {
            var iniManager = new IniManager(IniFilePath);
            var config = iniManager.ReadIniFile();

            // Verifique se todas as chaves necessárias estão presentes
            return
                config.ContainsKey("Host") &&
                config.ContainsKey("Port") &&
                config.ContainsKey("Database");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string hashedPassword = GetMd5Hash(txtPassword.Password);

            if (AuthenticateUser(username, hashedPassword))
            {
                MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                try
                {
                    MainWindow mainWindow = new MainWindow();  // Instancia a janela principal
                    mainWindow.Show();  // Mostra a janela principal
                    this.Close();  // Fecha a janela de login após abrir a janela principal
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao abrir a janela principal: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro de Autenticação", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string username, string hashedPassword)
        {
            try
            {
                connection.Open();  // Abra a conexão com o banco de dados

                string sql = "SELECT pass FROM usuarios WHERE login = @username;";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    object result = command.ExecuteScalar();  // Retorna a senha hash do banco de dados

                    if (result != null)
                    {
                        string storedHashedPassword = result.ToString();  // Senha hash no banco

                        // Verifique se a senha hash no banco de dados corresponde à senha fornecida
                        return storedHashedPassword == hashedPassword;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.Close();  // Feche a conexão para evitar vazamentos
            }

            return false;  // Se a autenticação falhar
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));  // Converte para hexadecimal
                }

                return sb.ToString();  // Retorna o hash MD5
            }
        }

        private void OpenConfigWindow()
        {
            ConfigIni configWindow = new ConfigIni();  // Cria a tela de configuração
            configWindow.Show();  // Abra a tela de configuração
            this.Hide();  // Esconde a janela de login para evitar fechá-la
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Manipulador de eventos para mudanças de texto no campo de usuário
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ConfigIni configIni = new ConfigIni();
            configIni.Show();
            this.Close();  // Fecha a janela de login após abrir a tela de configuração
        }
    }
}
