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

        private async void btnBaixar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPasta.Text))
            {
                MessageBox.Show("Escolha uma pasta para salvar as músicas.");
                return;
            }

            btnBaixar.Enabled = false;
            progressBar1.Value = 0;
            lblStatus.Text = "Iniciando download...";

            try
            {
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
                txtUrl.Focus();
                txtUrl.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
            finally
            {
                btnBaixar.Enabled = true;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtUrl.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Escolha a pasta para salvar as músicas";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtPasta.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
