using FileClassification;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NewsFilterAI;

public partial class App : Application
{
    private ProcessReuters processReuters;
    private Entropy entropy;

    public App()
    {
        this.processReuters = new ProcessReuters();
        this.entropy = new Entropy();

        //this.processReuters.processReuters();
       
            
        MessageBox.Show("Exit");
    }

}

