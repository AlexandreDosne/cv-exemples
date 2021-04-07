/*
 *      Codé par Alexandre Dosne.
 *      Tiré d'un logiciel de contabilité privé développé pour un ami.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniaCompta
{
    static class Program
    {
        public static readonly string PATH_EXE = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static readonly string PATH_DATA = PATH_EXE + "\\data";
        public static readonly string PATH_GFX = PATH_DATA + "\\gfx";
        public static readonly string PATH_OUTPUT = PATH_EXE + "\\output";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

namespace MiniaCompta.DataHandling
{
    public static class Database
    {
        public static readonly string PATH_DATABASE = Program.PATH_DATA + "\\database.dat";
        public static DATA[] database = new DATA[0xFFFF];
        public static readonly string[] clients = {"CLIENT1", "CLIENT2"};
        public static ushort index = 0xFFFF;

        public static void DeletDBEntry(ushort id)
        {
            ushort last_index = 0;

            for (int i = 0; i < database.Length; i++)
            {
                if (i + 1 < database.Length && i + 2 < database.Length)
                {
                    if (database[i].name != null && database[i + 1].name == null && database[i + 2].name == null)
                    {
                        last_index = (ushort)i;
                        break;
                    }
                }
            }

            database[id] = database[last_index];
            database[id].id = id;
            database[last_index] = new DATA();

            index = 0xFFFF;
        }

        public static void LoadDatabase()
        {
            string[] raw_arr = File.ReadAllLines(PATH_DATABASE);
            string[] final_arr = new string[raw_arr.Length];
            DATA[] final_dat_arr = new DATA[0xFFFF];

            for (int i = 0; i < raw_arr.Length; i++)
            {
                string[] sliced = raw_arr[i].Split('@');
                final_dat_arr[i] = new DATA(sliced[0], sliced[1], sliced[2], bool.Parse(sliced[3]), sliced[4], ushort.Parse(i.ToString()));
            }
            database = final_dat_arr;
        }

        public static void WriteDatabase()
        {
            ushort arr_length = 0;
            for (int i = 0; i < database.Length; i++)
            {
                if (database[i].path != null) arr_length++;
            }

            string[] arr_toWrite = new string[arr_length];

            for (int i = 0; i < arr_length; i++)
            {
                arr_toWrite[i] = database[i].path + "@" + database[i].name + "@" + database[i].price + "@" + database[i].paid.ToString().ToLower() + "@" + database[i].client;
            }
            File.WriteAllLines(PATH_DATABASE, arr_toWrite);
        }

        public static void BackupDatabase()
        {
            File.Copy(PATH_DATABASE, PATH_DATABASE + "." + DateTime.Now.ToString("HHmmss-ddMMyyyy") + ".bckp");
        }

        public static void AddToDatabase(DATA nDat)
        {
            for (int i = 0; i < database.Length; i++)
            {
                if (database[i].name == null)
                {
                    nDat.id = (ushort)i;
                    database[i] = nDat;
                    return;
                }
            }
        }
    }

    public struct DATA
    {
        public string path { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public bool paid { get; set; }
        public string client { get; set; }
        public ushort id { get; set; }

        public DATA(string t_path, string t_name, string t_price, bool t_paid, string t_client, ushort t_id)
        {
            path = t_path;
            name = t_name;
            price = t_price;
            paid = t_paid;
            client = t_client;
            id = t_id;
        }
    }
}