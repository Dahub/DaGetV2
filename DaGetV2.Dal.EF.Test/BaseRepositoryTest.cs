using System;
using System.Linq;
using DaGetV2.Domain;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class BaseRepositoryTest : TestBase
    {
        public BaseRepositoryTest()
            : base("baseRepositoryDbName")
        {           
        }

        [Fact]
        public void GetById_Should_Get_Entity()
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                var userRepository = context.GetUserRepository();
                var entity = userRepository.GetById(_sammy.Id);

                Assert.NotNull(entity);
            }
        }

        [Fact]
        public void Add_Should_Add_Entity()
        {
            var idUSer = Guid.NewGuid();

            using (var context = new DaGetContext(_dbContextOptions))
            {
                var userRepository = context.GetUserRepository();

                userRepository.Add(new User()
                {
                    Id = idUSer,
                    UserName = "New"
                });

                context.Commit();
            }

            using (var context = new DaGetContext(_dbContextOptions))
            {
                var user = context.Users.FirstOrDefault(u => u.Id.Equals(idUSer));

                Assert.NotNull(user);
                Assert.True(user.CreationDate < DateTime.Now);
                Assert.True(user.ModificationDate < DateTime.Now);
                Assert.Equal(user.CreationDate, user.ModificationDate);
            }
        }

        [Fact]
        public void Update_Should_Update_Entity()
        {
            var newUserName = "newUserName";

            using (var context = new DaGetContext(_dbContextOptions))
            {
                var userRepository = context.GetUserRepository();

                _sammy.UserName = newUserName;
                userRepository.Update(_sammy);

                context.Commit();
            }

            using (var context = new DaGetContext(_dbContextOptions))
            {
                var user = context.Users.FirstOrDefault(u => u.Id.Equals(_sammy.Id));

                Assert.NotNull(user);
                Assert.Equal(newUserName, user.UserName);
                Assert.True(user.ModificationDate < DateTime.Now);
                Assert.NotEqual(user.CreationDate, user.ModificationDate);
            }

            ResetDataBase();
        }

        [Fact]
        public void Delete_Should_Delete_Entity()
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                var numberOfUsers = context.Users.Count();

                Assert.True(numberOfUsers > 0);

                var userRepository = context.GetUserRepository();

                userRepository.Delete(_sammy);

                context.Commit();

                var newNumberOfUsers = context.Users.Count();

                Assert.Equal(numberOfUsers - 1, newNumberOfUsers);
            }
        }

        [Fact]
        public void GetAll_Should_Get_All_Entities()
        {
            var expectedNumberOfUsers = 0;

            using (var context = new DaGetContext(_dbContextOptions))
            {
                for (var i = 0; i < 50; i++)
                {
                    var now = DateTime.Now;
                    context.Add(new User()
                    {
                        CreationDate = now,
                        Id = Guid.NewGuid(),
                        ModificationDate = now,
                        UserName = Guid.NewGuid().ToString()
                    });
                }

                context.Commit();

                expectedNumberOfUsers = context.Users.Count();
            }

            using (var context = new DaGetContext(_dbContextOptions))
            {
                var userRepository = context.GetUserRepository();

                var users = userRepository.GetAll();

                Assert.NotNull(users);
                Assert.NotEmpty(users);
                Assert.Equal(expectedNumberOfUsers, users.Count());
            }
        }
    }
}
