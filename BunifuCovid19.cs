using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics; 
using System.Windows.Forms;

namespace Bunifu.Covid19.Data
{
    [DebuggerStepThrough]
    public partial class BunifuCovid19 : Component
    {
        private System.Windows.Forms.WebBrowser browser; 
        public BunifuCovid19()
        {
            InitializeComponent();
        }
        public bool IsBusy { get; set; } = false;
        public event EventHandler OnComplete = null;
        public event EventHandler OnBusyStatusChange = null;
        public event EventHandler OnStopped = null;
        public BunifuCovid19(IContainer container)
        {
            
            container.Add(this);
            InitializeComponent();

            browser = new System.Windows.Forms.WebBrowser();
   
            this.browser.Size = new System.Drawing.Size(50, 50);
            this.browser.MinimumSize = new System.Drawing.Size(0, 0);
            this.browser.Location = new System.Drawing.Point(100, 100);

            this.browser.ScriptErrorsSuppressed = true;

            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.browser_DocumentCompleted);

            container.Add(browser);
           
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        
            Scrapper.Scrapper.Init(browser.DocumentText);
            this.IsBusy = false;
            OnBusyStatusChange?.Invoke(this.IsBusy, new EventArgs()); 
            OnComplete?.Invoke(browser.DocumentText, new EventArgs());
        }

        public void Fetch()
        {
            this.Stop();
            this.IsBusy = true;
            OnBusyStatusChange?.Invoke(this.IsBusy, new EventArgs());
            browser.Navigate("https://www.worldometers.info/coronavirus/");
        }

        public void Refresh()
        {
            this.Fetch();
        }

        public List<Scrapper.CovidRecord> GetAffactedCountries()
        {

            if (this.IsBusy) throw new Exception("Component data not fetched/initialied");
            return Scrapper.Scrapper.GetCountries();
        }

        public Scrapper.CovidRecord GetWorldSummary()
        {
            if (this.IsBusy) throw new Exception("Component data not fetched/initialied");
            return Scrapper.Scrapper.GetWorldSummary();
        }
        public void Stop()
        {
            browser.Stop();
            this.IsBusy = false;
            OnBusyStatusChange?.Invoke(this.IsBusy, new EventArgs());
            OnStopped?.Invoke(this, new EventArgs());
        }
    }
}
