using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GPS
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            List<int> list = new List<int> { 0, 11, 22 };

            File.WriteAllText(@"c:../../list_test.json", JsonConvert.SerializeObject(list));

            string json = File.ReadAllText(@"c:../../list_test.json");
            List<int> newList = JsonConvert.DeserializeObject<List<int>>(json);
            Console.WriteLine(newList[1]);
            */

            /*
            List<List<int>> listlist = new List<List<int>> { };
            listlist.Add(new List<int> { 0, 11, 22 });
            listlist.Add(new List<int> { 33, 44, 55 });

            File.WriteAllText(@"c:../../list_test.json", JsonConvert.SerializeObject(listlist));

            string json = File.ReadAllText(@"c:../../list_test.json");
            List<List<int>> newList = JsonConvert.DeserializeObject<List<List<int>>>(json);
            Console.WriteLine(newList[1][1]);
            */





            Console.WriteLine("HELLO");


            List<List<List<int>>> speedLimitMatrix = randomSpeedLimitMatrix(3, 10, 100);
            File.WriteAllText(@"c:../../speedLimitMatrix.json", JsonConvert.SerializeObject(speedLimitMatrix));

            string json = File.ReadAllText(@"c:../../speedLimitMatrix.json");
            List<List<List<int>>> newMatrix = JsonConvert.DeserializeObject<List<List<List<int>>>>(json);
            Console.WriteLine(newMatrix[1][1][1]);
            Console.WriteLine(speedLimitMatrix[1][1][1]);






            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GPS.Form1(100, speedLimitMatrix));

        }

        static List<List<List<int>>> randomSpeedLimitMatrix(int size, int min, int max)
        {
            List<List<List<int>>> matrix = new List<List<List<int>>> { };

            for (int i = 0; i < size; i++)
            {
                List<List<int>> column = new List<List<int>> { };
                for (int j = 0; j < size; j++)
                {
                    List<int> point = new List<int> { };

                    Random r1 = new Random();
                    point.Add(10 * (r1.Next(min, max + 1) / 10));
                    Thread.Sleep(20);
                    Random r2 = new Random();
                    point.Add(10 * (r2.Next(min, max + 1) / 10));

                    column.Add(point);
                }
                matrix.Add(column);
            }




            return matrix;
        }

    }
}
