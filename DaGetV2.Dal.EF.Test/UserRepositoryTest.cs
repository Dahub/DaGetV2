using System;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class UserRepositoryTest
    {
        [Theory]
        [InlineData("sammy", true)]
        [InlineData("Sammy", true)]
        [InlineData("Sammy ", false)]
        [InlineData("marius", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void UserExists_Should_Return_True_Or_False_When_Ask_For_User(string userName, bool expected)
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            DataBaseHelper.Instance.UseSammyUser(dbName);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();

                var shouldExists = userRepository.UserExists(userName);

                Assert.Equal(expected, shouldExists);
            }
        }

        [Fact]
        public void GetByUserName_Should_Return_User_When_User_Exist()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseSammyUser(dbName);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();

                var dbUser = userRepository.GetByUserName(user.UserName);

                Assert.NotNull(dbUser);
            }
        }

        [Fact]
        public void GetByUserName_Should_Return_User_When_User_Not_Exist()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();

                var dbUser = userRepository.GetByUserName(Guid.NewGuid().ToString());

                Assert.Null(dbUser);
            }
        }
    }
}
