using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Decrapify_Windows_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ListEmbeddedResources();
        }

        private void btnExecuteAll_Click(object sender, EventArgs e)
        {
            RunEmbeddedPowerShellScript("debloat.ps1");
        }

        private void RunEmbeddedPowerShellScript(string scriptName)
        {
            string namespaceName = Assembly.GetExecutingAssembly().GetName().Name;
            string resourceName = $"{namespaceName}.{scriptName}";
            string script = GetEmbeddedResource(resourceName);
            if (script != null)
            {
                RunPowerShellScript(script);
            }
            else
            {
                MessageBox.Show("Script não encontrado: " + resourceName, "Erro");
            }
        }

        private string GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void RunPowerShellScript(string scriptContent)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell.exe";
            psi.Arguments = $"-Command \"{scriptContent}\"";
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();

            // Opcional: Capturar e exibir a saída do script
            string output = process.StandardOutput.ReadToEnd();
            MessageBox.Show(output, "Resultado do Script");
        }

        private void ListEmbeddedResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (string resourceName in resourceNames)
            {
                Console.WriteLine(resourceName);
            }
        }

    }
}
