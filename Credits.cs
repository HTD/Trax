using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trax {
    
    public partial class Credits : Form {

        public static Credits Instance;

        private Timer DTimer = new Timer();

        public Credits() {
            InitializeComponent();
            VersionLabel.Text = Application.ProductVersion.ToString();
            PreviewKeyDown += Credits_PreviewKeyDown;
            Click += Credits_JustClose;
            DTimer.Tick += DTimer_Tick;
            DTimer.Interval = 200;
        }

        void DTimer_Tick(object sender, EventArgs e) {
            DTimer.Stop();
            Deactivate -= Credits_JustClose;
            Deactivate += Credits_JustClose;
            Opacity = 1;
            Activate();
        }

        public static new void Show() {
            if (Instance == null) {
                Instance = new Credits();
                Instance.Opacity = 0;
                Instance.Show(Main.Instance);
                Instance.DTimer.Start();
            }
        }

        protected override void OnClosed(EventArgs e) {
            DTimer.Dispose();
            Instance = null;
        }

        private void Credits_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            Close();
        }

        private void Credits_JustClose(object sender, EventArgs e) {
            Close();
        }



    }
}
