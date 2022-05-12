using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Security;

namespace RSAClass
{
    
    class RSA
    {        
        private string privateKey;
        private string publicKey;       
        
        public RSA(){}

        public string GetPrivateKey
        {
            get { return privateKey; }
        }

        public string GetPublicKey
        {
            get { return publicKey; }
        }

        public void GenerateKeys()
        {
            RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider(2048);
            privateKey = RsaKey.ToXmlString(true);
            publicKey = RsaKey.ToXmlString(false);            
        }
        
        public string Encrypt(string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string outputText = "";
            byte[] dataToByte = new byte[2048];
            byte[] encryptedData = new byte[2048];

            try
            {
                rsa.FromXmlString(publicKey);
            }
            catch (XmlSyntaxException xmlEx)
            {
                MessageBox.Show(xmlEx.Message, "Error");
            }

            try
            {
                dataToByte = Encoding.Unicode.GetBytes(data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            try
            {
                encryptedData = rsa.Encrypt(dataToByte, false);
            }
            catch(CryptographicException crEx)
            {
                MessageBox.Show(crEx.Message, "Error");
            }

            finally
            {

                rsa.PersistKeyInCsp = false;
            }

            try
            {
                outputText = Convert.ToBase64String(encryptedData);
            }
            catch(ArgumentNullException arEx)
            {
                MessageBox.Show(arEx.Message, "Error");
            }

            return outputText;
        }

        public string Decrypt(string data, string privateKey)
        {            
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string outputText = "";
            byte[] dataToByte = new byte[2048];
            byte[] decryptedData = new byte[2048];

            try
            {
                rsa.FromXmlString(privateKey);
            }
            catch(XmlSyntaxException xmlEx)
            {
                MessageBox.Show(xmlEx.Message, "Error");
            }

            try
            {
                dataToByte = Convert.FromBase64String(data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            try
            {
                decryptedData = rsa.Decrypt(dataToByte, false);
            }
            catch(CryptographicException )
            {
                MessageBox.Show("Invalid private key", "Error");
            }

            finally
            {
                rsa.PersistKeyInCsp = false;
            }

            try
            {
                outputText = Encoding.Unicode.GetString(decryptedData);
            }
            catch(ArgumentNullException arEx)
            {
                MessageBox.Show(arEx.Message, "Error");
            }
                        
            return outputText;
        }

        public string EncryptKey(string privateKey)
        {
            byte[] data = new byte[2048];
            try
            {
                data = Encoding.Unicode.GetBytes(privateKey);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
         
            string output = Convert.ToBase64String(data);
            return output;
        }

        public string DecryptKey(string encrKey)
        {
            byte[] data = new byte[2048];
            try
            {
                data = Convert.FromBase64String(encrKey);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            
            string output = Encoding.Unicode.GetString(data);
            return output;
        }
    }// кінець класу RSA
}// кінець namespace
