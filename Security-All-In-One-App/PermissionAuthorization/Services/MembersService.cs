using PermissionAuthorization.Models;
using Bogus;

namespace PermissionAuthorization.Services
{
    public class MembersService
    {
        private readonly Faker _faker;
        public MembersService()
        {
            _faker = new Faker();
        }
        public Member GetMemberByIdQuery(int id)
        {
            return new Member
            {
                Id = id,
                Name = _faker.Person.FirstName,
                UserName = _faker.Person.UserName,
            };
        }
    }
}
