using Bogus;
using MiniCrmApi.Entities;

namespace MiniCrmApi
{
    public static class FakeDataGenerator
    {
        public static List<Customer> GenerateCustomers(int count)
        {
            var faker = new Faker<Customer>("tr")
                .RuleFor(c => c.FullName, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("5#########")) // 5 ile başlayan telefon
                .RuleFor(c => c.CreatedAt, _ => DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 30)))
                .RuleFor(c => c.Company, f => f.Company.CompanyName());

            return faker.Generate(count);
        }
    }
}
