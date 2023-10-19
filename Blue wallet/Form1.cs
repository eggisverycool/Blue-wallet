using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Blue_wallet
{
    public partial class Form1 : Form
    {
        string Username;
        string Password;
        Point LastPoint;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            LastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - LastPoint.X;
                this.Top += e.Y - LastPoint.Y;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();

            // Create the request body.
            var requestBody = $"{{ \"Method\": \"VerifyUser\", \"Username\": \"{textBox1.Text}\", \"Password\": \"{textBox2.Text}\" }}";

            // Add the request body to the request.
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Send the request.
            var response = await client.PostAsync("https://darkbluestealth.pythonanywhere.com", content);

            if (response.Content.ReadAsStringAsync().Result == "False")
            {
                label4.Visible = true;
            }
            if (response.Content.ReadAsStringAsync().Result == "True")
            {
                panel2.Visible = true;
                label4.Visible = false;
                Username = textBox1.Text;
                Password = textBox2.Text;
                timer1.Enabled = true;
                Console.WriteLine(Username);
                Console.WriteLine(Password);
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            var client = new HttpClient();

            // Create an empty request body.
            var content = new StringContent($"{{ \"Method\": \"CheckBalance\", \"Username\": \"{Username}\", \"Password\": \"{Password}\" }}", Encoding.UTF8, "application/json");

            // Send the request.
            var response = await client.PostAsync("https://darkbluestealth.pythonanywhere.com", content);

            // Check for an error message in the response body.
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 400 Bad Request error!
                // Read the error message from the response body.
                var errorMessage = await response.Content.ReadAsStringAsync();

                // Try to fix the cause of the error and send the request again.
                // For example, if the error message is "Invalid username or password", you can try sending the request with a different username or password.

                // If you are unable to fix the cause of the error, you can contact the server administrator for assistance.

                // Log the error message to the console.
                Console.WriteLine($"The request failed with status code {response.StatusCode}. Error message: {errorMessage}");

                // Return from the method.
                return;
            }

            // Success!
            label5.Text = "$" + response.Content.ReadAsStringAsync().Result;
        }
    }
}
