using Microsoft.Extensions.DependencyInjection;

namespace QQWRFO_Mobilprog_Feleves
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}