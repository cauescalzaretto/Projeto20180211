using System;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PDVTEF
{
    public class PagAPI
    {
        public string GetMacAddress()
        {

            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Only consider Ethernet network interfaces
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return Convert.ToString(nic.GetPhysicalAddress());
                    }
                }
            }
            catch
            {
                
            }
            return null;
        }

        public async void GetAllVendas()
        {
            string URI = "http://pagapi.azurewebsites.net/API/getStatusVendas";
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var VendasJsonString = await response.Content.ReadAsStringAsync();
                        var lstvendas = JsonConvert.DeserializeObject<PDVTEF.Entity.Vendas[]>(VendasJsonString).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o vendas : " + response.StatusCode);
                    }
                }
            }
        }



    }
}
