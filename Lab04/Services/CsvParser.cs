using Lab04.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab04.Services
{
    internal class CsvParser
    {
        public Customer[] ParseCustomers(string content)
        {
            List<Customer> customers = new List<Customer>();
            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i= 0; i<lines.Length; i++)
            {
                string line = lines[i];
                string[] columns = line.Split(',');
                if (columns.Length != 3)
                {
                    string timestamp = DateTime.Now.ToString("G");
                    Console.WriteLine($"[{timestamp}] Invalid Customer in line {line+1}.");
                }

                string name = columns[0].Trim();
                string phone = columns[1].Trim();
                string email = columns[2].Trim();
                string ratings = columns[3].Trim();
                name = name.ToUpper();
                phone = phone.Replace('6', '9');
                double[] SatRatings = ratings.Trim(new char[] { '[', ']' })      
                    .Split(',')                           
                    .Select(x => double.Parse(x.Trim()))  
                    .ToArray();                          

                foreach (double number in SatRatings)
                {
                    Console.WriteLine(number);
                }

                customers.Add(new Customer(name,
                    name, phone, email, SatRatings));
            }
            return customers.ToArray();
        }
    }
}
