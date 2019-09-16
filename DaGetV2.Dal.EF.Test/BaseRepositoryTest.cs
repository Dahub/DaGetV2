using System;
using System.Linq;
using DaGetV2.Domain;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class BaseRepositoryTest 
    {
        [Fact]
        public void GetById_Should_Get_Entity()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();
                var entity = userRepository.GetById(user.Id);

                Assert.NotNull(entity);
            }
        }

        [Fact]
        public void Add_Should_Add_Entity()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var idUSer = Guid.NewGuid();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();

                userRepository.Add(new User()
                {
                    Id = idUSer,
                    UserName = "New"
                });

                context.Commit();
            }

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
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
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var newUserName = "newUserName";

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userRepository = context.GetUserRepository();

                user.UserName = newUserName;
                userRepository.Update(user);

                context.Commit();
            }

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var updatedUser = context.Users.FirstOrDefault(u => u.Id.Equals(user.Id));

                Assert.NotNull(updatedUser);
                Assert.Equal(newUserName, updatedUser.UserName);
                Assert.True(updatedUser.ModificationDate < DateTime.Now);
                Assert.NotEqual(updatedUser.CreationDate, updatedUser.ModificationDate);
            }
        }

        [Fact]
        public void Delete_Should_Delete_Entity()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var numberOfUsers = context.Users.Count();

                Assert.True(numberOfUsers > 0);

                var userRepository = context.GetUserRepository();

                userRepository.Delete(user);

                context.Commit();

                var newNumberOfUsers = context.Users.Count();

                Assert.Equal(numberOfUsers - 1, newNumberOfUsers);
            }
        }

        [Fact]
        public void GetAll_Should_Get_All_Entities()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var expectedNumberOfUsers = 0;

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
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

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
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
