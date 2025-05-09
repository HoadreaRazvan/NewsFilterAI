using FileClassification;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NewsFilterAI;

public partial class App : Application
{
    private ProcessReuters processReuters;

    public App()
    {
        this.processReuters = new ProcessReuters();
        this.processReuters.processReuters();

        MessageBox.Show("Exit");
    }

}

