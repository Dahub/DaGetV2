using System;
using DaGetV2.Domain;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class UserRepositoryTest : TestBase
    {
        public UserRepositoryTest()
           : base("userRepositoryDbName")
        {
        }

        [Theory]
        [InlineData("sammy", true)]
        [InlineData("Sammy", true)]
        [InlineData("Sammy ", false)]
        [InlineData("marius", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void UserExists_Should_Return_True_Or_False_When_Ask_For_User(string userName, bool expected)
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                var userRepository = context.GetUserRepository();

                var shouldExists = userRepository.UserExists(userName);

                Assert.Equal(expected, shouldExists);
            }
        }
    }
}
