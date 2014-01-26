using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class PersonalManager
    {
        private static PersonalManager instance = new PersonalManager();
        public static PersonalManager Instance
        {
            get { return instance; }
        }

        private string[] _personalPaths;
        public string[] PersonalPaths
        {
            get
            {
                if (this._personalPaths == null)
                {
                    this.initPersonals();
                }
                return this._personalPaths;
            }
        }

        private PersonalManager() { }
        private void initPersonals()
        {
            string personalDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personals");
            if (Directory.Exists(personalDir))
            {
                _personalPaths = Directory.GetFiles(personalDir, "*.jpg");
            }
            else
            {
                throw new Exception("未找到人员头像....");
            }
        }
    }
}
