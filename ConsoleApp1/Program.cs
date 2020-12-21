using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        static void Main(string[] args)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;

            while (true) { }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task<IEnumerable<string>> GetItems(string path)
        {
            var response = await Client.GetAsync(path);

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsAsync<List<string>>();
        }

        private static async Task RunAsync()
        {
            // Update your local service port no. / service APIs etc in the following line
            if (Client.BaseAddress == null)
            {
                Client.BaseAddress = new Uri("https://localhost:44378/api/Movie/GetPeriodList");
            }
            
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var items = await GetItems("https://localhost:44378/api/Movie/GetPeriodList");
                Console.WriteLine("Items read using the web api GET");
                Console.WriteLine(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
