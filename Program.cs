using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            bool newRandomMatrix = false;
            int newRandomMatrixSize = 15;
            /////////////////////////////

            List<List<List<int>>> speedLimitMatrix = new List<List<List<int>>> { };

            if (newRandomMatrix)
            {
                Console.Write("\rBuilding new matrix of size " + newRandomMatrixSize + "x" + newRandomMatrixSize + "...");
                speedLimitMatrix = randomSpeedLimitMatrix(newRandomMatrixSize, 10, 100);
                File.WriteAllText(@"c:../../speedLimitMatrix.json", JsonConvert.SerializeObject(speedLimitMatrix));
            }
            else
            {
                string json = File.ReadAllText(@"c:../../speedLimitMatrix.json");
                speedLimitMatrix = JsonConvert.DeserializeObject<List<List<List<int>>>>(json);
            }

            List<string> pathList = new List<string> { };

            pathList.Add(pickSimplePath(speedLimitMatrix));
            pathList.Add(pickRandomPath(speedLimitMatrix));
            pathList.Add(pickGreedyPath(speedLimitMatrix));
            pathList.Add(pickDynamicPath(speedLimitMatrix));

            //path = pickRandomPath(speedLimitMatrix);
            //path = pickSimplePath(speedLimitMatrix);
            //path = pickGreedyPath(speedLimitMatrix);
            //path = pickBruteForcePath(speedLimitMatrix);
            //path = pickDynamicPath(speedLimitMatrix);







            //Console.Read();


            //int length = pathLength(speedLimitMatrix, path);
            //Console.WriteLine("\n\n" + path + " path length: " + length);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GPS.Form1(45, speedLimitMatrix, pathList));
            Console.WriteLine("yeet");

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

            Console.WriteLine("\rRandom path = " + path + "                      \n");
            return path;
        }

        static string pickSimplePath(List<List<List<int>>> matrix)
        {
            string r = "RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR";
            string d = "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";

            string path = r.Substring(0, matrix.Count - 1) + d.Substring(0, matrix.Count - 1);

            Console.WriteLine("\rSimple path = " + path + "                         \n");
            return path;
        }

        static string pickGreedyPath(List<List<List<int>>> matrix)
        {
            string path = "";
            int x = 0;
            int y = 0;

            for (int i = 0; i < (matrix.Count - 1) * 2; i++)
            {
                if ((matrix[x][y][0] > matrix[x][y][1] && y < matrix.Count - 1) || x == matrix.Count - 1)
                {
                    path += "D";
                    y += 1;
                }
                else
                {
                    path += "R";
                    x += 1;
                }
            }

            Console.WriteLine("\rGreedy path = " + path + "                                    \n");
            return path;
        }

        static string pickBruteForcePath(List<List<List<int>>> matrix)
        {
            Console.Write("\rDetermining all possible paths...          ");
            List<string> lst = allPossiblePaths((matrix.Count - 1) * 2, matrix.Count - 1);
            Console.Write("\rCalculating fastest path...                ");

            string minPath = lst[0];
            int minPathLength = pathLength(matrix, lst[0]);
            for (int i = 1; i < lst.Count; i++)
            {
                int currentPathLength = pathLength(matrix, lst[i]);
                if (currentPathLength < minPathLength)
                {
                    minPath = lst[i];
                    minPathLength = currentPathLength;
                }
            }

            Console.Write("\rBrute Force path = " + minPath + "                  ");
            return minPath;

            List<string> allPossiblePaths(int length, int rCount)
            {
                List<string> list = new List<string> { };

                if (length == 0 && rCount == 0)
                {
                    return list;
                }
                else if (rCount == 0)
                {
                    list.Add("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD".Substring(0, length));
                    return list;
                }
                else if (rCount == length)
                {
                    list.Add("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR".Substring(0, length));
                    return list;
                }
                else
                {
                    List<string> rChoice = allPossiblePaths(length - 1, rCount - 1);
                    for (int i = 0; i < rChoice.Count; i++)
                    {
                        list.Add("R" + rChoice[i]);
                    }
                    List<string> dChoice = allPossiblePaths(length - 1, rCount);
                    for (int i = 0; i < dChoice.Count; i++)
                    {
                        list.Add("D" + dChoice[i]);
                    }
                }

                return list;
            }
        }

        static string pickDynamicPath(List<List<List<int>>> matrix)
        {
            string path = "";

            List<List<int>> minMatrix = new List<List<int>> { };
            List<List<string>> directionMatrix = new List<List<string>> { };
            for (int i = 0; i < matrix.Count; i++)
            {
                minMatrix.Add(new List<int> { });
                directionMatrix.Add(new List<string> { });
                for (int j = 0; j < matrix.Count; j++)
                {
                    minMatrix[i].Add(0);
                    directionMatrix[i].Add("/");
                }
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                minMatrix.Add(new List<int> { });
                directionMatrix.Add(new List<string> { });
                for (int j = 0; j < matrix.Count; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if (i == 0)
                        {
                            minMatrix[i][j] = minMatrix[i][j - 1] + matrix[i][j - 1][1];
                            directionMatrix[i][j] = directionMatrix[i][j - 1] + "U";
                        }
                        else if (j == 0)
                        {
                            minMatrix[i][j] = minMatrix[i - 1][j] + matrix[i - 1][j][0];
                            directionMatrix[i][j] = directionMatrix[i - 1][j] + "L";
                        }
                        else if (minMatrix[i][j - 1] + matrix[i][j - 1][1] < minMatrix[i - 1][j] + matrix[i - 1][j][0])
                        {
                            minMatrix[i][j] = minMatrix[i][j - 1] + matrix[i][j - 1][1];
                            directionMatrix[i][j] = directionMatrix[i][j - 1] + "U";
                        }
                        else
                        {
                            minMatrix[i][j] = minMatrix[i - 1][j] + matrix[i - 1][j][0];
                            directionMatrix[i][j] = directionMatrix[i - 1][j] + "L";
                        }
                    }

                }
            }

            path = directionMatrix[matrix.Count - 1][matrix.Count - 1];
            path = path.Substring(1);
            path = Regex.Replace(path, "L|R", (m) => m.Value == "L" ? "R" : "L");
            path = Regex.Replace(path, "U|D", (m) => m.Value == "U" ? "D" : "U");

            Console.WriteLine("\rDynamic path = " + path + "                                    \n");
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

        static void PrintStringList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(i + ": " + list[i]);
            }
            Console.WriteLine();
        }

    }
}
