using System;
using System.Collections.Generic;
using System.IO;

namespace webtester
{
    class Program
    {


        public static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                runOneTest(@"" + args[0], @"" + args[1]);

            }
            else if (args.Length == 1)
            {
                runAllTests(@"" + args[0]);

            }
            else
            {
                runInConsole();

            }

            Console.Read();
        }

        static void runInConsole()
        {
            CaseRunner r = new CaseRunner();
            Console.WriteLine("running in console");
            r.runWithoutFile();
        }

        static void runOneTest(string test, string output)
        {
            CaseRunner r = new CaseRunner();
            Console.WriteLine("test file : " + test + " exists: " + File.Exists(test));
            Console.WriteLine("output file : " + output + " exists: " + File.Exists(output));
            if (!File.Exists(output)) File.Create(output);
            r.runFromFile(test, output);

        }

        static void runAllTests(string root)
        {
            if (!Directory.Exists(root))
            {
                Console.WriteLine("does not exist");
                Directory.CreateDirectory(root);
                createSubDirectory(root, "tests");
                createSubDirectory(root, "results");
                return;
            }

            if (!doesSubFolderExist(root, "results")) createSubDirectory(root, "results");
            if (!doesSubFolderExist(root, "tests"))
            {
                createSubDirectory(root, "tests");
                Console.WriteLine("test directory created, please populate it with tests: " + root + "\\tests");
                return;
            }

            string tests = root + "\\tests";
            string results = root + "\\results";

            string[] files = Directory.GetFiles(tests);
            string[,] testList = new string[files.Length, 2];
            int x = 0;
            foreach (string s in files)
            {
                Console.WriteLine(s);
                string str = getNameFile(s);
                if (!doesSubFolderExist(results, str))
                {
                    Console.WriteLine("path does not exist");
                    createSubDirectory(results, str);
                }
                testList[x, 0] = s;
                testList[x, 1] = results + "\\" + str;
                //Console.WriteLine(str);
                x++;
            }

            for (int y = 0; y < testList.GetLength(0); y++)
            {
                CaseRunner r = new CaseRunner();
                Console.WriteLine(testList[y, 0] + " : " + testList[y, 1] + "\\" + DateTime.Now.ToString() + ".TXT");
                string output =  DateTime.Now.ToString() + ".TXT";
                Console.WriteLine("proper output path: " + testList[y, 1] + "\\" + string.Join("_", output.Split(Path.GetInvalidFileNameChars())));
                r.runFromFile(testList[y, 0], testList[y, 1] + "\\" + string.Join("_",output.Split(Path.GetInvalidFileNameChars())));
            }
        }

        static bool doesSubFolderExist(string folder, string subfolder)
        {
            string[] sub = Directory.GetDirectories(folder);

            bool hasSub = false;


            foreach (string s in sub)
            {
                string str = getNameOfFolder(s);
                if (str.Equals(subfolder)) hasSub = true;

                //Console.WriteLine(str);
            }
            Console.WriteLine(hasSub);
            return hasSub;
        }

        static void createResultFolders(string root)
        {
            //NOT MADE YET
        }

        static string getNameFile(string path)
        {
            path = path.Substring(path.LastIndexOf("\\") + 1);
            path = path.ToLower();
            return path.Remove(path.IndexOf(".txt"));

        }

        static void createSubDirectory(string root, string sub)
        {
            Directory.CreateDirectory(root + "\\" + sub);
            Console.WriteLine("creating path: " + root + "\\" + sub);
        }

        static string getNameOfFolder(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1);
        }


    }
}
