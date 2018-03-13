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

namespace email
{
    public partial class Form1 : Form
    {
        public string[] userID=new string[4];
        
        public Form1()
        {
            try
            {
                userID = System.IO.File.ReadAllLines(@"userID.txt");
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd odczytu pliku.\n" + e.ToString(),"Error");
                Environment.Exit(1);
            }
            try
            {
                if (!(userID[0] == "" || userID[1] == "" || userID[2] == "" || userID[3] == ""))
                {
                    StringBuilder message=new StringBuilder();
                    message.Append("Witaj");
                    message.Append(" ");
                    message.Append(userID[1]);
                    MessageBox.Show(message.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Plik userID.txt nie jest kompletny","Error");
                Environment.Exit(2);
            }
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBarSend.Value = 0;
            Thread thread = new Thread(send);
            thread.Start();
        }

        private void send()
        {
            SmtpClient smtp = new SmtpClient(userID[3]);
            progressBarSend.Value = 10;
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential(userID[1], userID[2]);
            smtp.EnableSsl = true;
            progressBarSend.Value = 30;

            MailMessage mail = new MailMessage(/*from*/userID[0],
                                               /*to*/textBoxTo.Text,
                                               /*subject*/textBoxSubject.Text,
                                               /*body*/"stmp test");
            progressBarSend.Value = 70;
            smtp.Send(mail);
            progressBarSend.Value = 100;
            MessageBox.Show("Done");
        }
    }
}
