using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class RecordManager
    {
        private static RecordManager instance = new RecordManager();
        public static RecordManager Instance
        {
            get { return instance; }
        }

        private string path;
        private string roundPath;

        private RecordManager()
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "record.txt");
            roundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "round.txt");
        }

        public void RecordMessage(string msg)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(msg);
                sw.Flush();
                sw.Close();
            }
        }

        public void RecordRound(int round)
        {
            File.WriteAllText(roundPath, round.ToString());
        }

        public string[] GetRecords()
        {
            string[] records = File.ReadAllLines(path);
            return records;
        }

        public int GetRound()
        {
            int round = Convert.ToInt32(File.ReadAllText(roundPath));
            return round;
        }
    }
}
