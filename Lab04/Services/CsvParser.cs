using Lab04.Models;
using Lab04.Services.Validators;
using System;
using System.Globalization;

namespace Lab04.Services
{
    internal class CsvParser
    {
        private readonly NameValidator _nameValidator;
        private readonly PhoneNumberValidator _phoneValidator;
        private readonly EmailAddressValidator _emailValidator;

        public CsvParser(NameValidator nameValidator, 
                PhoneNumberValidator phoneValidator,
                EmailAddressValidator emailValidator)
        {
            _nameValidator = nameValidator;
            _phoneValidator = phoneValidator;
            _emailValidator = emailValidator;
        }

        public Customer[] ParseCustomers(string content)
        {
            List<Customer> customers = new List<Customer>();
            var lines = content.Split('\n');

            for (int i= 0; i<lines.Length; i++)
            {
                var line = lines[i].Trim();
                if(string.IsNullOrEmpty(line)) continue;
                var parts = line.Split(';');
                if(parts.Length<4)
                {
                    PrintInvalidCustomer(i+1); // I check wheter there are 4 values in a row
                    continue;
                }
                var nameparts = parts[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameparts.Length < 2)
                {
                    PrintInvalidCustomer(i + 1); // say error if there is no | firstname | lastname |
                    continue;
                }
                string firstname = nameparts[0].Trim();
                string lastname = nameparts[1].Trim();
                string phone = parts[1].Trim();
                string email = parts[2].Trim();
                string ratings = parts[3].Trim();
                double[] satisfactionRatings = ratings.Trim('[',']')
                                                 .Split(',')
                                                 .Select(double.Parse)
                                                 .ToArray();
                if(!_nameValidator.Validate(firstname) || !_nameValidator.Validate(lastname))
                {
                    PrintInvalidCustomer(i+1);
                    continue;
                }
                if(!_phoneValidator.Validate(phone))
                {
                    PrintInvalidCustomer(i+1);
                    continue;
                }
                if(!_emailValidator.Validate(email))
                {
                    PrintInvalidCustomer(i+1);
                    continue;
                }
                Customer customer = new Customer(firstname, lastname, phone, email, satisfactionRatings);
                customers.Add(customer);
            }
            return customers.ToArray();
        }
        private void PrintInvalidCustomer(int lineNumber)
        {
            var timeStamp = DateTime.Now.ToString("g", CultureInfo.CreateSpecificCulture("en-US"));
            Console.WriteLine($"[{timeStamp}] Invalid Customer in line {lineNumber}.");
        }
    }
}
