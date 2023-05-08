using Mmvm.Initializer.Impl;

namespace SampleWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationInitializer().Initialize().CreateTree(nameof(MainVm));
        }
    }
}