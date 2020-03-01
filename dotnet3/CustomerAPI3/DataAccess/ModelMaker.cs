using System;
using BlitzkriegSoftware.SecureRandomLibrary;

namespace CustomerAPI3.DataAccess
{
    /// <summary>
    /// Model Maker
    /// </summary>
    public static class ModelMaker
    {
        private static readonly SecureRandom Dice = new SecureRandom();

        /// <summary>
        /// Make a Person Full of Data
        /// </summary>
        /// <returns></returns>
        public static Models.Customer PersonMake()
        {
            var person = new Models.Customer()
            {
                _id = Guid.NewGuid().ToString(),
                Birthday =Faker.DateOfBirth.Next(),
                EMail = Faker.Internet.Email(),
                NameLast = Faker.Name.Last()
            };

            person.NameFirst = Faker.Name.First();

            person.Company = person.EMail.Substring(person.EMail.IndexOf('@') + 1);

            person.EMail = string.Format("{0}.{1}@{2}", person.NameFirst, person.NameLast, person.Company);

            for (int p = 0; p < Dice.Next(2, 6); p++)
            {
                person.Preference.Add(string.Format("{0}-{1}", Faker.Lorem.GetFirstWord(), p), Faker.Lorem.Sentence());
            }

            person.Addresses.Add(new Models.Address()
            {
                Address1 = string.Format("{0} {1} {2}", Dice.Next(101, 8888), Faker.Address.StreetName(), Faker.Address.StreetSuffix()),
                City = Faker.Address.City(),
                State = Faker.Address.UsStateAbbr(),
                Zip = $"{Faker.Address.StreetName()} {Faker.Address.StreetSuffix()}",
                Kind = Models.AddressKind.Mailing
            });

            if (Dice.Next(1, 10) > 7)
            {
                person.Addresses.Add(new Models.Address()
                {
                    Address1 = string.Format("{0} {1} {2}", Dice.Next(101, 8888), Faker.Address.StreetName(), Faker.Address.StreetSuffix()),
                    City = Faker.Address.City(),
                    State = Faker.Address.UsStateAbbr(),
                    Zip = $"{Faker.Address.StreetName()} {Faker.Address.StreetSuffix()}",
                    Kind = Models.AddressKind.Billing
                });
            }

            return person;
        }
    }
}
