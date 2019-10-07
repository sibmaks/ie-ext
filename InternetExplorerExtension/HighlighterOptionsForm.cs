using System;
using System.Windows.Forms;
namespace InternetExplorerExtension
{
    public partial class HighlighterOptionsForm : Form
    {
        public HighlighterOptionsForm()
        {
            InitializeComponent();

            SetAsWindowSize();
        }

        private void SetAsWindowSize()
        {
            webBrowser1.Height = this.Height;
            webBrowser1.Width = this.Width;
        }








        //public string InputText
        //{
        //    get { return this.textBox1.Text; }
        //    set { this.textBox1.Text = value; }
        //}

        private void Button2_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void HighlighterOptionsForm_Load(object sender, EventArgs e)
        {
                 var htmlinject = @"<html><body style='background-color: green;'>Please enter your name:<br/><input type='text' name='userName'/><br/><a href='@\xd.html'>continue</a></body></html>";



            //     webBrowser1.Navigate("about:blank");
            //     while (webBrowser1.Document == null || webBrowser1.Document.Body == null)
            //         Application.DoEvents();
            //     webBrowser1.Document.OpenNew(true).Write(htmlinject);

            //webBrowser1.Refresh();
            //var str = "<html><head></head><body>" + sender.ToString() + "</body></html>";
            //webBrowser1.DocumentText = str;
            webBrowser1.Document.OpenNew(true).Write(htmlinject);



            MessageBox.Show(webBrowser1.DocumentText.ToString());
        }
    }
}