using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace email
{
    public partial class Form1 : Form
    {
        public string userEmail;
        public string userLogin;
        public string userPassword;
        public string smtpServer;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SmtpClient smtp = new SmtpClient(smtpServer);
            smtp.Port =587;
            smtp.Credentials = new System.Net.NetworkCredential(userLogin, userPassword);
            smtp.EnableSsl = true;

            MailMessage mail = new MailMessage(/*from*/userEmail,
                                               /*to*/textBoxTo.Text,
                                               /*subject*/"test stmp",
                                               /*body*/"stmp test");
            smtp.Send(mail);
            MessageBox.Show("Done");
        }

    }
}
