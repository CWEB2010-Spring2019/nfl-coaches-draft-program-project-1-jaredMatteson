using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
     class Program
    {
       static List<Players> players;

        public static void LoadJson()
        {
            using (StreamReader r = new StreamReader("players.json"))
            {
                string json = r.ReadToEnd();
                players = JsonConvert.DeserializeObject<List<Players>>(json);
            }
        }
    }

    

    class Players
    {
        private string Name { get; set; }
        private string School { get; set; }
        private int Salary { get; set; }
        private int Rank { get; set;  }
        private string Position { get; set; }

        /*
        private Players(string name, string school, int salary, int rank, string position )
        {
            this.name = name;
            this.school = school;
            this.salary = salary;
            this.rank = rank;
            this.position = position;
        }
        */
        
    }
}
