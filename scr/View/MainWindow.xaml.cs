using System.Collections.Generic;
using MahApps.Metro.Controls;
using Npgsql;

namespace ZeusTopClientes
{
    // Classe que representa um cliente, com ajustes para tipos de dados esperados
    public class TopCliente
    {
        public int CodCliente { get; set; } // Campo int
        public string ClienteNome { get; set; } // Campo string
        public string PontualidadePercent { get; set; } // Campo texto, com símbolo de porcentagem
        public string ComprasAVista { get; set; } // Campo texto, com símbolo de porcentagem
        public string ComprasLiq { get; set; } // Campo texto, com símbolo de porcentagem
        public string CadastroAntigo { get; set; } // Campo texto, com símbolo de porcentagem
        public string NumCompras { get; set; } // Campo texto, com símbolo de porcentagem
        public string FrequenciaDiaria { get; set; } // Campo texto, com símbolo de porcentagem
        public string Ranking { get; set; } // Campo texto, com símbolo de porcentagem
        public int Quantidade { get; set; } // Campo int
        public decimal ValorTotal { get; set; } // Campo decimal
    }

    public partial class MainWindow : MetroWindow
    {
        private const string IniFilePath = "ZTopclientes.ini";
        private const string Username = "postgres";  // Usuário fixo
        private const string Password = "pepino";  // Senha fixa

        public MainWindow()
        {
            InitializeComponent();

            var iniManager = new IniManager(IniFilePath);
            var config = iniManager.ReadIniFile();

            string connString = $"Host={config["Host"]};Port={config["Port"]};Username={Username};Password={Password};Database={config["Database"]}";

            var vtopClientes = GetTopClientesFromDatabase(connString, "SELECT * FROM vtopclientes");
            dataCliente.Items.Clear();
            dataCliente.ItemsSource = vtopClientes;
        }

        private List<TopCliente> GetTopClientesFromDatabase(string connString, string query)
        {
            var clientes = new List<TopCliente>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new TopCliente();

                            // Tratamento correto para colunas como texto ou inteiros
                            cliente.CodCliente = reader.GetInt32(0); // Campo int
                            cliente.ClienteNome = reader.GetString(1); // Campo texto

                            cliente.PontualidadePercent = reader.GetString(2); // Campo texto
                            cliente.ComprasAVista = reader.GetString(3); // Campo texto
                            cliente.ComprasLiq = reader.GetString(4); // Campo texto
                            cliente.CadastroAntigo = reader.GetString(5); // Campo texto
                            cliente.NumCompras = reader.GetString(6); // Campo texto
                            cliente.FrequenciaDiaria = reader.GetString(7); // Campo texto
                            cliente.Ranking = reader.GetString(8); // Campo texto

                            cliente.Quantidade = reader.GetInt32(9); // Campo int
                            cliente.ValorTotal = reader.GetDecimal(10); // Campo decimal

                            clientes.Add(cliente);
                        }
                    }
                }

                conn.Close();
            }

            return clientes;
        }

        private void dataCliente_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle the selection changed event here, if needed
        }
    }

}
