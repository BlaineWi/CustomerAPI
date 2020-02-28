using System;
using BlitzkriegSoftware.SecureRandomLibrary;

namespace customerAPI.DataAccess
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
        public static Models.Person PersonMake()
        {
            var person = new Models.Person()
            {
                _id = Guid.NewGuid().ToString(),
                Birthday = Faker.Date.Birthday(),
                EMail = Faker.User.Email(),
                NameLast = Faker.Name.LastName()
            };

            var gender = Faker.Name.Gender();
            if (gender.ToLowerInvariant().StartsWith("f"))
            {
                person.NameFirst = Faker.Name.FemaleFirstName();
            }
            else
            {
                person.NameFirst = Faker.Name.MaleFirstName();
            }

            person.Company = person.EMail.Substring(person.EMail.IndexOf('@') + 1);

            person.EMail = string.Format("{0}.{1}@{2}", person.NameFirst, person.NameLast, person.Company);

            for (int p = 0; p < Dice.Next(2, 6); p++)
            {
                person.Preference.Add(string.Format("{0}-{1}", Faker.Lorem.Word(), p), Faker.Lorem.Sentence());
            }

            person.Addresses.Add(new Models.Address()
            {
                Address1 = string.Format("{0} {1} {2}", Faker.Number.RandomNumber(101, 8888), Faker.Address.StreetName(), Faker.Address.StreetSuffix()),
                City = Faker.Address.USCity(),
                State = Faker.Address.StateAbbreviation(),
                Zip = Faker.Address.USZipCode(),
                Kind = Models.AddressKind.Mailing
            });

            if (Dice.Next(1, 10) > 7)
            {
                person.Addresses.Add(new Models.Address()
                {
                    Address1 = string.Format("{0} {1} {2}", Faker.Number.RandomNumber(101, 8888), Faker.Address.StreetName(), Faker.Address.StreetSuffix()),
                    City = Faker.Address.USCity(),
                    State = Faker.Address.StateAbbreviation(),
                    Zip = Faker.Address.USZipCode(),
                    Kind = Models.AddressKind.Billing
                });
            }

            return person;
        }
    }
}
