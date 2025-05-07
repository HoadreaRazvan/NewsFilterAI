using FileClassification;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NewsFilterAI;

public partial class App : Application
{
    private FileReader fileReader;

    public App()
    {
        this.fileReader=new FileReader();
        this.fileReader.processReuters();

        MessageBox.Show("Exit");
    }

}

