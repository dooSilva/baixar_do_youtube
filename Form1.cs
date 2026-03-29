using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static baixarMusicasYoutube.youTubeDownload;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace baixarMusicasYoutube
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Evento de clique para o botão de baixar
        private async void btnBaixar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPasta.Text)) // Verifica se o campo de pasta está vazio
            {
                MessageBox.Show("Escolha uma pasta para salvar as músicas.");
                return; // Sai do método se a pasta não for selecionada
            }

            btnBaixar.Enabled = false; // Desabilita o botão para evitar múltiplos cliques
            progressBar1.Value = 0; // Reseta a barra de progresso
            lblStatus.Text = "Iniciando download..."; // Atualiza o status para indicar que o download está começando

            try
            {
                // Chama o método de download assíncrono, passando os callbacks para atualizar a barra de progresso e o status
                await YouTubeDownloader.BaixarAsync(
                    txtUrl.Text,
                    txtPasta.Text,
                    progress =>
                    {
                        Invoke(new Action(() =>
                            progressBar1.Value = progress));
                    },
                    status =>
                    {
                        Invoke(new Action(() =>
                            lblStatus.Text = status));
                    }
                );
                MessageBox.Show("Sucesso!", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Download concluído!";
                txtUrl.Focus(); // Define o foco de volta para o campo de URL após o download ser concluído
                txtUrl.Text = ""; // Limpa o campo de URL para facilitar o próximo download
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro"); // Exibe a mensagem de erro caso ocorra uma exceção durante o download
            }
            finally
            {
                btnBaixar.Enabled = true; // Reabilita o botão após o download ser concluído ou em caso de erro
            }
        }

        // Evento para definir o foco no campo de URL quando o formulário for exibido
        private void Form1_Shown(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }

        // Evento de clique para o botão de escolher pasta
        private void button1_Click(object sender, EventArgs e)
        {
            // Abre um diálogo para escolher a pasta onde as músicas serão salvas
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Escolha a pasta para salvar as músicas"; // Define a descrição do diálogo
                dialog.ShowNewFolderButton = true; // Permite que o usuário crie uma nova pasta diretamente no diálogo

                if (dialog.ShowDialog() == DialogResult.OK) // Verifica se o usuário selecionou uma pasta e clicou em OK
                {
                    txtPasta.Text = dialog.SelectedPath; // Atualiza o campo de texto com o caminho da pasta selecionada
                }
            }
        }
    }
}
