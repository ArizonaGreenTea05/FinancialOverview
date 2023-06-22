using System.Windows.Forms;

namespace WinFormsApp
{
    public static class Run
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Gui());
        }
    }
}
