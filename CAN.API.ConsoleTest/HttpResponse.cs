using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.ConsoleTest
{
    class HttpResponse
    {
        public async void UpdateReservationPatch(int id)
        {
            
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://192.168.8.98/api/Can/msg"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Byte[]>(apiResponse);
                }
            }

        }
    }
}
