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
            bool newRandomMatrix = false;
            int newRandomMatrixSize = 5;





            List<List<List<int>>> speedLimitMatrix = new List<List<List<int>>> { };

            if (newRandomMatrix)
            {
                speedLimitMatrix = randomSpeedLimitMatrix(newRandomMatrixSize, 10, 100);
                File.WriteAllText(@"c:../../speedLimitMatrix.json", JsonConvert.SerializeObject(speedLimitMatrix));
            }
            else
            {
                string json = File.ReadAllText(@"c:../../speedLimitMatrix.json");
                speedLimitMatrix = JsonConvert.DeserializeObject<List<List<List<int>>>>(json);
            }
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

                    if (i < size - 1)
                    {
                        Random r1 = new Random();
                        point.Add(10 * (r1.Next(min, max + 1) / 10));
                    }
                    else
                    {
                        point.Add(999999);
                    }
                    Thread.Sleep(20);
                    if (j < size - 1)
                    {
                        Random r2 = new Random();
                        point.Add(10 * (r2.Next(min, max + 1) / 10));
                    }
                    else
                    {
                        point.Add(999999);
                    }


                    column.Add(point);
                }
                matrix.Add(column);
            }




            return matrix;
        }

    }
}
