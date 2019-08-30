using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace FormForCreatingValues
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                string[] valuesFromForm = {textBox1.Text.Replace('.', ','), textBox2.Text.Replace('.', ','), textBox3.Text.Replace('.', ',')};
                string path = @"\\vmware-host\Shared Folders\Projects\WebAppFixed\FormForCreatingValues\Values from the lab\";
                string subpath = $@"{DateTime.Now.Year}\{DateTime.Now.Month}\";
                Value[] valuesToTextFile = new Value[3];

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                dirInfo.CreateSubdirectory(subpath);
                
                IFirebaseConfig config = new FirebaseConfig
                                {
                                    AuthSecret = "r0Dx9Gi0gj7g3kZ4EJQodtFgOIq80IuBfBuko8HS",
                                    BasePath = "https://fir-2d407.firebaseio.com/"
                                };
                IFirebaseClient client = new FirebaseClient(config);
                
                
                var response = client.Get("Last/Laboratory values/Id");
                int displacement = Convert.ToInt16(response.Body); // Смещение Id значений из Firebase
                
                int i;
                for (i = 1 + displacement; i <= 3 + displacement; i++)
                {
                    var labValueToFirebase = new FirebaseValue(i, Convert.ToDouble(valuesFromForm[i - displacement - 1]), DateTime.Now);
                    
                    client.Set("Laboratory values/" + labValueToFirebase.Id, labValueToFirebase);
                    Thread.Sleep(100);
                    
                    response = client.Get("Last/Sensor values/Value");
                    valuesToTextFile[i - displacement - 1] = new Value(i,labValueToFirebase.Value,Convert.ToDouble(response.Body.Replace('.',',')), labValueToFirebase.Time);
                    
                    if (i == 3 + displacement)
                    {
                        client.Set("Last/Laboratory values", labValueToFirebase);
                    }
                }
                
                using (FileStream fstream = new FileStream(path + subpath+ $@"{DateTime.Now.Day}.txt", FileMode.OpenOrCreate))
                {
                    for (i = 0; i < 3; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes
                            ($@"L_{valuesToTextFile[i].LaboratoryValue} S_{valuesToTextFile[i].SensorValue} T_{valuesToTextFile[i].Time}.{valuesToTextFile[i].Time.Millisecond} L-S:{valuesToTextFile[i].LaboratoryValue-valuesToTextFile[i].SensorValue}" + "\n");
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => Environment.Exit(Environment.ExitCode);
    }
}
