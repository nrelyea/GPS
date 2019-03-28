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
            /////////////////////////////
            bool newRandomMatrix = true;
            int newRandomMatrixSize = 5;
            /////////////////////////////

            List<List<List<int>>> speedLimitMatrix = new List<List<List<int>>> { };

            if (newRandomMatrix)
            {
                Console.Write("\rBuilding new matrix of size " + newRandomMatrixSize + "...");
                speedLimitMatrix = randomSpeedLimitMatrix(newRandomMatrixSize, 10, 100);
                File.WriteAllText(@"c:../../speedLimitMatrix.json", JsonConvert.SerializeObject(speedLimitMatrix));
            }
            else
            {
                string json = File.ReadAllText(@"c:../../speedLimitMatrix.json");
                speedLimitMatrix = JsonConvert.DeserializeObject<List<List<List<int>>>>(json);
            }





            string path = "";

            path = pickRandomPath(speedLimitMatrix);







            //Console.Read();


            int length = pathLength(speedLimitMatrix, path);
            Console.WriteLine(path + " path length: " + length);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GPS.Form1(75, speedLimitMatrix, path));

        }

        static string pickRandomPath(List<List<List<int>>> matrix)
        {
            Console.Write("\rPicking Random Path...                   ");
            string path = "RDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRDRD";

            path = path.Substring(0, (matrix.Count - 1) * 2);

            for (int i = 0; i < (20 * matrix.Count); i++)
            {
                Thread.Sleep(15);
                Random r1 = new Random();
                int index1 = r1.Next(0, path.Length);
                Random r2 = new Random();
                int index2 = r1.Next(2, path.Length);

                if (index1 != index2)
                {
                    if (index1 > index2)
                    {
                        int temp = index1;
                        index1 = index2;
                        index2 = temp;
                    }

                    string str = "";
                    if (index1 > 0)
                    {
                        str += path.Substring(0, index1);
                    }
                    str += path[index2];
                    str += path.Substring(index1 + 1, index2 - (index1 + 1));
                    str += path[index1];
                    if (index2 < path.Length - 1)
                    {
                        str += path.Substring(index2 + 1, path.Length - (index2 + 1));
                    }
                    path = str;
                }

            }

            Console.WriteLine("\rChosen path = " + path + "\n");
            return path;
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

        static int pathLength(List<List<List<int>>> matrix, string path)
        {
            int length = 0;
            int x = 0;
            int y = 0;


            while (path.Length > 0)
            {
                if (path[0] == 'R')
                {
                    length += matrix[x][y][0];
                    x += 1;
                }
                else if (path[0] == 'D')
                {
                    length += matrix[x][y][1];
                    y += 1;
                }
                path = path.Substring(1);
            }

            return length;
        }

    }
}
