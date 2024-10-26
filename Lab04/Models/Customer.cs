using System;

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
            FirstName = firstname.ToUpper();
            LastName = lastname.ToUpper();
            PhoneNumber = phoneNumber.Replace('6','9');
            EmailAddress = emailAddress;
            SatisfactionRatings = satisfactionRatings;
        }

        public override string ToString()
        {
            var formattedRatings = SatisfactionRatings.Select(rating => rating.ToString("F2"));
            return $"{FirstName} {LastName}, {PhoneNumber}, {EmailAddress}, [ {string.Join(", ", formattedRatings)} ])";
        }
    }
}

