using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baixarMusicasYoutube
{
    internal static class Program
    {
        ////MÉTODO PARA PEGAR O ENDEREÇO MAC DA PLACA DE REDE DO COMPUTADOR
        public static string GetMacAddress()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic =>
                    nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
        }
        /// ///////////////////////////////////////////////////////////////////

        ///LISTA DOS MAC PERMITIDOS PARA A CONEXAO
        static readonly string[] MacsPermitidos =
        {
            "D09466BA4CC4",
            "047C16C4C7E1",
            "441CA8BFCAB3"
        };
        /// /////////////////////////////////////////////////////////////////////////


        [STAThread]
        static void Main()
        {
            ///PEGANDO MAC DO COMPUTADOR E COMPARANDO COM A LISTA//////
            string macAtual = GetMacAddress();
            if (macAtual == null || !MacsPermitidos.Contains(macAtual))
            {
                MessageBox.Show(
                    "Dispositivo sem acesso a internet, ou sem permissão",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            ///////////////////////////////////////////////////////


            ///INICIANDO APLICAÇÃO
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
