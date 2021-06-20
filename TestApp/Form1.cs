using SharpUpdate;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace TestApp
{
	public partial class Form1 : Form
	{
		private SharpUpdater updater;

		public Form1()
		{
			InitializeComponent();
			
			label1.Text = ProductName + "\n" + ProductVersion;

			updater = new SharpUpdater(Assembly.GetExecutingAssembly(), this, new Uri("https://raw.githubusercontent.com/Omararafa/Tool-Deployment-Order/main/Update.xml"));
			//updater = new SharpUpdater(Assembly.GetExecutingAssembly(), this, new Uri(new System.IO.FileInfo(@"..\..\..\project.xml").FullName));       // for local testing
		}

		private void button1_Click(object sender, EventArgs e)
		{
			updater.DoUpdate();
			string test = "";
		}
		private void TestClick()
		{
			button1.PerformClick();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			TestClick();
			ControlBox = false;
			button1.Visible = false;
			label1.Visible = false;

		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
