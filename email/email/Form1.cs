using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace email
{
    public partial class Form1 : Form
    {
        public string[] userID=new string[4];
        
        public Form1()
        {
            try
            {
                userID = System.IO.File.ReadAllLines(@"userID");
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd odczytu pliku.\n" + e.ToString(),"Error");
            }
            try
            {
                if (userID[0].Length == 0 || userID[1].Length == 0 || userID[2].Length == 0 || userID[3].Length == 0)
                {
                    throw new System.ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Plik userID.txt nie jest kompletny!","Error");
            }
            InitializeComponent();
            try
            {
                Load("lastKnownEmailAdress");
            }
            catch (Exception)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBarSend.Value = 0;
            Thread thread = new Thread(Send);
            thread.Start();
        }

        private void Send()
        {
            try
            {
                Invoke(new Action(() => { progressBarSend.Value += 20; }));
                SmtpClient smtp = new SmtpClient(userID[3]);
                Invoke(new Action(() => { progressBarSend.Value += 20; }));
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(userID[1], userID[2]);
                smtp.EnableSsl = true;
                Invoke(new Action(() => { progressBarSend.Value += 20; }));

                MailMessage mail = new MailMessage(/*from*/userID[0],
                                                   /*to*/textBoxTo.Text,
                                                   /*subject*/textBoxSubject.Text,
                                                   /*body*/textBoxMessage.Text);
                Invoke(new Action(() => { progressBarSend.Value += 20; }));
                smtp.Send(mail);
                Invoke(new Action(() => { progressBarSend.Value += 20; }));
                MessageBox.Show("Done");
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie powiodło się wysłanie wiadomości! \nSprawdzi dane w pliku userID.txt oraz poprawność adresu email osoby do której piszesz. \nPoprawna kolejność w pliku:\nemail użytkownika\nlogin\nhasło\nsmtp server","Error");
            }
            Invoke(new Action(() => { progressBarSend.Value = 0; }));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Save(saveFileDialog.FileName);
            }
        }

        private void Save(string saveFileDialog)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(textBoxTo.Text);
            stringBuilder.Append("*");
            stringBuilder.Append(textBoxSubject.Text);
            stringBuilder.Append("*");
            stringBuilder.Append(textBoxMessage.Text);
            File.WriteAllText(saveFileDialog, stringBuilder.ToString());
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Load(openFileDialog.FileName);
            }
        }

        private void Load(string openFileDialog)
        {
            string read = File.ReadAllText(openFileDialog);
            string[] splitedTxt = read.Split('*');
            try
            {
                textBoxTo.Text = splitedTxt[0];
                textBoxSubject.Text = splitedTxt[1];
                textBoxMessage.Text = splitedTxt[2];
            }
            catch(Exception)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(@"lastKnownEmailAdress",textBoxTo.Text);
        }
    }
}
