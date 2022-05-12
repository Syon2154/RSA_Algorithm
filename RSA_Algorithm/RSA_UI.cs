using System;
using System.Windows.Forms;
using System.IO;
using RSA = RSAClass.RSA;
using System.Text;

namespace EncryptingAlg
{
    public partial class RSA_UI : Form
    {
        private string fName = ""; // змінна для збереження шляху до відкритого файлу   
        RSA rsa = new RSA(); // екземпляр класу RSA
        public RSA_UI()
        {
            InitializeComponent();
        }

        private void wasFileOpen(bool flag = false) // функція перевірки на відкриття файлу 
        {
            if(!flag)
            {
                saveToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveToolStripMenuItem.Enabled = true;
            }            
        }

        private static string UniqueFileName(string path, int count = 0) // функція для створення унікального імені файла
        {
            if (count == 0)
            {
                if (!File.Exists(path))
                {
                    return path;
                }
            }
            else
            {
                var candidatePath = string.Format(
                    @"{0}\{1}_{2}{3}",
                    Path.GetDirectoryName(path),
                    Path.GetFileNameWithoutExtension(path),
                    count,
                    Path.GetExtension(path));

                if (!File.Exists(candidatePath))
                {
                    return candidatePath;
                }
            }

            count++;
            return UniqueFileName(path, count);
        }

        // Компоненти графічного інтерфейсу

        // Обробка події натискання на клавішу "Відкрити" в ToolStripMenuItem
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            OpenFileDialog openFile = new OpenFileDialog(); 
            openFile.Filter = "Текстовые файлы (*.txt)|*.txt|All Files (*.*)| *.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {                
                using (StreamReader readFile = new StreamReader(openFile.FileName, Encoding.GetEncoding(1251)))
                {
                    richTextBox1.Text = readFile.ReadToEnd();
                    fName = openFile.FileName;
                    wasFileOpen(true);                    
                    readFile.Close();
                }                       
            }
        }

        // Обробка події натискання на клавішу "Зберегти" в ToolStripMenuItem
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            using (StreamWriter sw = new StreamWriter(saveFile.FileName = fName))
            {
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
            richTextBox1.Text = "";
        }

        // Обробка події натискання на клавішу "Зберегти як..." в ToolStripMenuItem
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Текстовые файлы (*.txt)|*.txt|All Files (*.*)| *.*";

            if (saveFile.ShowDialog() == DialogResult.OK && saveFile.FileName !=  "")
            {               
                using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                {
                    sw.Write(richTextBox1.Text);
                    sw.Close();                  
                }
                richTextBox1.Text = "";

            }
        }

        // Обробка події натискання на клавішу "Про програму" в ToolStripMenuItem
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Версія : 2.0.1 \nСтворено : Omega4Tech \nАвтор : Синельник А. О. \nГрупа : 302-ТН \nНавчальний заклад : Полтавський Національний Технічний університет імені Юрія Кондратюка", "Про програму", MessageBoxButtons.OK);
        }

        // Обробка події натискання на клавішу "Вихід" в ToolStripMenuItem
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            Close();
        }

         /*Обробка події, що виникає при закриванні програми
         викликаємо повідомлення з запитом на підтверждення виходу
        якщо відповідь задовільна - закриваємо програму
        якщо негативний передаємо у властивість  Cancel події е значення true*/
        private void RSA_UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("Закрити програму ?", "Вихід", MessageBoxButtons.OKCancel);

            if(res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        // Обробка події натискання на клавішу "Зашифрувати" 
        private void EncryptButton_Click(object sender, EventArgs e)
        {            
            if (richTextBox1.Text.Length > 0)
            {                
                rsa.GenerateKeys();
                string privateKey = rsa.GetPrivateKey.ToString();
                string put = UniqueFileName("data/privateKey.xml"); 
                richTextBox1.Text  = rsa.Encrypt(richTextBox1.Text);
                Directory.CreateDirectory("data");
                File.WriteAllText(put, rsa.EncryptKey(privateKey), Encoding.UTF8);                            
            }
            else
            {
                MessageBox.Show("Шифрування неможливе : відсутній текст.", "Відмова!", MessageBoxButtons.OK);
            }
        }

         // Обробка події натискання на клавішу "Розшифрувати"  
        private void DecryptButton_Click(object sender, EventArgs e)
        {
            bool canDecrypt = true;

            if (richTextBox1.Text.Length == 0)
            {                
                MessageBox.Show("Дешифрування неможливе : відсутній текст.", "Відмова!", MessageBoxButtons.OK);
                canDecrypt = false;
            }

            if (textBox1.Text.Length == 0 && richTextBox1.Text.Length != 0)
            {                
                MessageBox.Show("Введіть приватний ключ", "Відмова!", MessageBoxButtons.OK);
                canDecrypt = false;
            }
            
            if(canDecrypt)
            {
                string key = rsa.DecryptKey(textBox1.Text); 
                string DecryptedText = rsa.Decrypt(richTextBox1.Text, key);
                richTextBox1.Text = DecryptedText;
            }
        }

        private void FindKeyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKey = new OpenFileDialog();
            openKey.Filter = "XML Documents (*.xml)|*.xml|All Files (*.*)| *.*";

            if (openKey.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader readFile = new StreamReader(openKey.FileName, Encoding.UTF8))
                {
                    textBox1.Text = readFile.ReadToEnd();
                    readFile.Close();
                }

            }           
        }

        private void ClearKeyButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
} 