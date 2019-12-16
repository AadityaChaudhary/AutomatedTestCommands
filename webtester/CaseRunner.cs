using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace webtester
{
    class CaseRunner
    {

        Driver driver = new Driver();

        Dictionary<string,string> values = new Dictionary<string, string>();

        List<string> split(string s)
        {
            int start = 0;
            List<string> line = new List<string>();
            bool ignoreSpace = false;
            for (int x = 0; x < s.Length; x++)
            {
                if (x == s.Length - 1)
                {
                    string str = s.Substring(start, s.Length - start);
                    if (str.StartsWith('"'))
                    {
                        str = str.Substring(1, str.Length - 2);
                    }
                    line.Add(str);
                }
                else if (s[x] == ' ' && !ignoreSpace)
                {
                    string str = s.Substring(start, x - start);
                    if (str.StartsWith('"'))
                    {
                        str = str.Substring(1, str.Length - 2);
                    }
                    line.Add(str);
                    start = x + 1;



                }
                if (s[x] == '"')
                {
                    ignoreSpace = !ignoreSpace;
                }
            }
            return line;
        }

        string execute(List<string> par)
        {
            for(int x = 0; x < par.Count; x++)
            {
                Console.Write(par[x] + " >>> ");
            }
            Console.WriteLine();
            switch (par[0])
            {
                case "OPEN":
                    if (par.Count != 2) return "invalid params";
                    else
                    {
                        driver.openLink(par[1]);
                        return "PASS - open";
                    }

                case "CLOSE":
                    if (par.Count != 1) return "invalid params";
                    else
                    {
                        driver.closeBrowser();
                        return "CLOSE";
                    }

                case "CLICK":
                    if (par.Count != 3) return "invalid params";
                    else
                    {
                        return driver.click(par[1].ToLower() + "=" + par[2]);
                    }

                case "TYPE":
                    if (par.Count != 4) return "invalid params";
                    else
                    {
                        return driver.type(par[1].ToLower() + "=" + par[2], par[3]);
                    }

                case "WAITFOR":
                    if (par.Count != 3) return "invalid params";
                    else
                    {
                        driver.waitForElement(par[1].ToLower() + "=" + par[2], -1);
                        return "PASS - Waiting for " + par[1] + " " + par[2];
                    }

                case "CHECKVIS":
                    if (par.Count != 3) return "invalid params";
                    else
                    {
                        if (driver.doesElementExist(par[1] + "=" + par[2]))
                        {
                            return par[1] + " " + par[2] + " does exist!";
                        }
                        else
                        {
                            return par[1] + " " + par[2] + " does not exist!";
                        }
                    }
                default:
                    if (par.Contains("="))
                    {
                        addVal(par[0], par[2]);
                        return "set value: " + par[0] + " to : " + par[2];
                    }
                    return "command not recognized";
                    
            }
            
        }

        static void writeResults(string[] lines, string[] results, string filePath)
        {
            File.Create(filePath).Close(); // wipe the file
            using (StreamWriter file = new StreamWriter(filePath))
            {

                for (int y = 0; y < lines.Length; y++)
                {

                    if (lines[y].Equals("") || lines[y].StartsWith(">>"))
                    {
                        continue; //line should not be reprinted, as it's either blank or old results
                    }
                    else if (lines[y].StartsWith("#"))
                    {
                        file.WriteLine(lines[y]); //print the comment, but not the corresponding results string
                    }
                    else if (results[y] == null) // means the test crashed before it reached this line
                    {
                        file.WriteLine(lines[y]);
                        file.WriteLine(">> no results for line");
                    }
                    else //print both line and result
                    {
                        file.WriteLine(lines[y]);
                        file.WriteLine(results[y]);
                    }
                }
            }
        }

        public void runFromFile(string filePath, string outputPath)
        {
            Console.WriteLine(filePath);
            string[] lines = File.ReadAllLines(filePath);
            string[] results = new string[lines.Length];
            int usefulLineCount = 1;
            for (int x = 0; x < lines.Length; x++)
            {
                Console.WriteLine("reading line : " + (x + 1));
                string line = lines[x];

                if (line.StartsWith("#") || line.Equals("") || line.StartsWith(">>")) continue;

                Console.WriteLine(line);

                string res = execute(split(line));
                results[x] = ">> step: " + (usefulLineCount++) + " | result: " + res;

                Console.WriteLine(results[x] == null ? "" : results[x]);

                if (res != null && (res.Equals("CLOSE") || res.ToLower().Contains("fail"))) break;

            }

            writeResults(lines, results, outputPath);
        }

        public void runWithoutFile()
        {
            for (int x = 1; true; x++)
            {
                Console.WriteLine("reading line : " + x);
                Console.Write("> ");
                string line = Console.ReadLine();
                string res = execute(split(line));
                if (res != null)
                {
                    if (res.Equals("CLOSE")) break;

                    Console.WriteLine(res);

                    break;
                }

            }
        }

        bool valExist(string key)
        {
            return values.ContainsKey(key);
        }

        public void addVal(string key, string val)
        {
            values[key] = val;

        }

        public string getVal(string key)
        {
            if (valExist(key)) return values[key];
            else return null;
        }

    }
}
