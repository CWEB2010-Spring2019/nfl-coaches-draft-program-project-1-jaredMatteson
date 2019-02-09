using System;
using System.Collections.Generic;


namespace project1
{
    class Program
    {
       
    }
    class Players
    {
        private string name { get; set; }
        private string school { get; set; }
        private int salary { get; set; }
        private int rank { get; set;  }
        private string position { get; set; }

        private Players(string name, string school, int salary, int rank, string position )
        {
            this.name = name;
            this.school = school;
            this.salary = salary;
            this.rank = rank;
            this.position = position;

        }
        
        
    }
}
