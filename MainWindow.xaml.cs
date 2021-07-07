using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vortex_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ServerItem> servers = new ObservableCollection<ServerItem>();

        public MainWindow()
        {
            InitializeComponent();

            setupBindings();

            _ = loadDataAsync();

        }

        private async Task loadDataAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://vortex.skullzbones.com/api2/listServers.php");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            ServerResponse res = JsonConvert.DeserializeObject<ServerResponse>(responseBody);
            if (res.Code != 1)
            {
                return;
            }
            List<ServerItem> servs = res.Data;

            foreach (ServerItem item in servs)
            {
                HttpResponseMessage r = await client.GetAsync("https://api.mcsrvstat.us/bedrock/2/"+item.HostExtip+":"+item.HostPort);
                r.EnsureSuccessStatusCode();
                string outp = await r.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(outp);

                item.HostExtip = (string)json["ip"];

                bool isOn = (bool)json["online"];
                String onlineStr = isOn == true ? "Online" : "Offline";
                item.isOnline = onlineStr;
                String pString;
                if (isOn)
                {
                    JObject player = (JObject)json["players"];
                    String online = (string)player["online"];
                    String max = (string)player["max"];
                    pString = online + "/" + max;
                }
                else
                {
                    pString = "0/0";
                }
                item.player = pString;

                servers.Add(item);
            }
        }

        private void setupBindings()
        {
            listtt.ItemsSource = servers;
        }

        void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ServerItem;
            if (item != null)
            {
                MessageBox.Show("Starting connection to server! Go to Friend's Tab in Minecraft!!");
                startConnector(item);
            }
        }

        private void startConnector(ServerItem item)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"bedrockProxy100\bedrockProxy");
            info.Arguments = item.HostExtip + " " + item.HostPort;

            info.UseShellExecute = true;
            info.Verb = "runas";
            var v = Process.Start(info);

            Trace.WriteLine(info.Arguments);
        }
    }


}
