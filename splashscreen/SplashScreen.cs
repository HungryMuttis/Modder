namespace Modder
{
    public partial class SplashScreen : Form
    {
        public SplashScreen(string name, string bottomText)
        {
            InitializeComponent();
            lblName.Text = name;
            lbl.Text = bottomText;
        }
    }
}
