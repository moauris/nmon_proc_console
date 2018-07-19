using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace console_nmon
{
    class Program
    {
        static void Main(string[] args)
        {
            string fli = 
                @"C:\perfdata\jcs0eb00_180712_0000.nmon";
            Console.WriteLine("Example:");
            Console.WriteLine(fli);
            Console.Write("Enter Target Path & Filename: ");
            //fli = Console.ReadLine();
           
            Proc_nmon.Extract(fli);




            Local_func.End_Program();
        }
    }
    public class Proc_nmon
    {
        public static void Extract(string arg_fli)
        {
            /*This Method extracts all information from a nmon file.
             * MEMNEW, CPU_ALL, DISKWIO, DISKRIO.
             * IOPS means everything aggregated from both RIO and WIO.
             * MEMNEW has Process_MAX, FSCache_MAX, System_MAX.
             * CPU MAX is simpler, it is just CPU Max
             * Need to have a file check system
             */
            string[] key_wd = { "MEMNEW", "CPU_ALL", "DISKWIO", "DISKRIO" };
            string[] arg_flo =
                { arg_fli.Replace(".nmon", "_nmon_" + key_wd[0] + ".txt"),
                arg_fli.Replace(".nmon", "_nmon_" + key_wd[1] + ".txt"),
                arg_fli.Replace(".nmon", "_nmon_" + key_wd[2] + ".txt"),
                arg_fli.Replace(".nmon", "_nmon_" + key_wd[3] + ".txt")};
            for (int i = 0; i < 4; i++)
            {
                if ((File.Exists(arg_flo[i])))
                    Console.WriteLine("{0} Alread Exist.", arg_flo[i]);
                else
                    Gen_File(arg_fli, arg_flo[i], key_wd[i]);
            }
            Console.WriteLine("Starting Calculation:");
            Get_MEMNEW(arg_flo[0]);
            GET_CPUALL(arg_flo[1]);
            GET_IOPS(arg_flo[2], arg_flo[3]);

        }

        public static void Gen_File
            (string arg_fli, string arg_flo, string key_word)
        {
            string[] lines =
                File.ReadAllLines(arg_fli);
            IEnumerable<string> linea =
                from q in lines
                where q.Contains(key_word)
                select q;
            IEnumerable<string> lineq = linea.ToArray()
                .Except(linea.Where(n => n.Contains("PCPU_ALL")));

            foreach (string ln in lineq)
                File.AppendAllText(arg_flo, ln + "\r\n");
            Console.WriteLine
                ("File {0} Run completed, found {1} lines.\r\n" +
                "File output: {2}.", arg_fli, lineq.Count(), arg_flo);
        }

        public static void Get_MEMNEW(string arg_fli)
        {//Get the Max Value of each data.
            Console.WriteLine("Get_MEMNEW => Start");
            //8 col
            string[,] tbl = new string[9,1442];
            int icol = 0;
            int irow = 0;
            string[] str_col = new string[8];
            string[] lines = File.ReadAllLines(arg_fli);
            foreach (string ln in lines)
            {
                str_col = ln.Split(',');
                foreach (string col in str_col)
                {// Generating table 8 x 1441
                    icol += 1;
                    //Console.Write(col + " | ");
                    tbl[icol, irow] = col;
                    if (icol == 8)
                        icol = 0;
                }
                irow += 1;
                //Console.WriteLine();
            }
            List<double> Process_ALL = new List<double>();
            List<double> FSCache_ALL = new List<double>();
            List<double> SYSTEM_ALL = new List<double>();

            for (int r = 1; r <= 1441; r++)
            {
                Process_ALL.Add(Convert.ToDouble(tbl[3, r]));
                FSCache_ALL.Add(Convert.ToDouble(tbl[4, r]));
                SYSTEM_ALL.Add(Convert.ToDouble(tbl[5, r]));
            }
            string sf1 = "{0}'s Max value is {1}";
            string out1 = string.Format(sf1, tbl[3, 0], Process_ALL.Max());
            string out2 = string.Format(sf1, tbl[4, 0], FSCache_ALL.Max());
            string out3 = string.Format(sf1, tbl[5, 0], SYSTEM_ALL.Max());
            Console.WriteLine(out1);
            Console.WriteLine(out2);
            Console.WriteLine(out3);

        }
        public static void GET_CPUALL(string arg_fli)
        {//Get the Max Value of each data.
            Console.WriteLine("Get_CPUALL => Start");
            //8 col
            string[,] tbl = new string[9, 1442];
            int icol = 0;
            int irow = 0;
            string[] str_col = new string[8];
            string[] lines = File.ReadAllLines(arg_fli);
            foreach (string ln in lines)
            {
                str_col = ln.Split(',');
                foreach (string col in str_col)
                {// Generating table 8 x 1441
                    icol += 1;
                    //Console.Write(col + " | ");
                    tbl[icol, irow] = col;
                    if (icol == 8)
                        icol = 0;
                }
                irow += 1;
                //Console.WriteLine();
            }
            List<double> User_Sys_ALL = new List<double>();
            double[] d_a = new double[2];
            for (int r = 1; r <= 1441; r++)
            {
                d_a[0] = Convert.ToDouble(tbl[3, r]);
                d_a[1] = Convert.ToDouble(tbl[4, r]);
                User_Sys_ALL.Add(d_a.Sum());
            }
            string sf1 = "{0}'s Max value is {1}";
            string out1 = string.Format(sf1, "CPU_MAX", User_Sys_ALL.Max());
            Console.WriteLine(out1);
        }
        public static void GET_IOPS (string arg_fli1, string arg_fli2)
        {//Get the Max Value of each data.
            Console.WriteLine("GET_IOPS => Start");
            //8 col
            string[,] tbl = new string[53, 1442];
            int icol = 0;
            int irow = 0;
            string[] str_col = new string[52];
            string[] lines = File.ReadAllLines(arg_fli1);
            string[] lines0 = File.ReadAllLines(arg_fli2);
            foreach (string ln in lines)
            {
                str_col = ln.Split(',');
                foreach (string col in str_col)
                {// Generating table 26 x 1441
                    icol += 1;
                    //Console.Write(col + " | ");
                    tbl[icol, irow] = col;
                    if (icol == 26)
                        icol = 0;
                }
                irow += 1;
                //Console.WriteLine();
            }
            icol = 0;
            irow = 0;
            foreach (string ln in lines0)
            {
                str_col = ln.Split(',');
                foreach (string col in str_col)
                {// Generating table 26 x 1441
                    icol += 1;
                    //Console.Write(col + " | ");
                    tbl[icol + 26, irow] = col;
                    if (icol == 26)
                        icol = 0;
                }
                irow += 1;
                //Console.WriteLine();
            }
            List<double> User_Sys_ALL = new List<double>();
            double[] d_a = new double[46];

            for (int r = 1; r <= 1441; r++)
            {
                for (int i = 0; i <= 22; i ++)
                    d_a[i] = Convert.ToDouble(tbl[i + 3, r]);
                for (int i = 26; i <= 48; i++)
                    d_a[i - 3] = Convert.ToDouble(tbl[i + 3, r]);
                User_Sys_ALL.Add(d_a.Sum());
            }
            string sf1 = "{0}'s Max value is {1}";
            string out1 = string.Format(sf1, "IOPS_ALL", User_Sys_ALL.Max());
            Console.WriteLine(out1);

        }

    }
    public class Local_func
    {
        public static void End_Program()
        {
            Console.WriteLine
                ("Finished, Press Any Key.");
            Console.Read();
        }
    }
}
