using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lab04.Models
{
    public class Customer
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string PhoneNumber { get; }
        public string EmailAddress { get; }
        public double[] SatisfactionRatings { get; }

        public Customer(string firstname, string lastname, string phoneNumber, string emailAddress, double[] satisfactionRatings)
        {
            FirstName = firstname;
            LastName = lastname;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            SatisfactionRatings = satisfactionRatings;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, {PhoneNumber}, {EmailAddress}, [ {string.Join(", ", SatisfactionRatings)} ])";
        }
    }
}

