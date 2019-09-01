using System;
using DaGetV2.Domain;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF.Test
{
    public abstract class TestBase : IDisposable
    {
        private readonly string _dbName;

        protected DbContextOptions _dbContextOptions;

        protected User _sammy;

        private TestBase() { }

        public TestBase(string dbName)
        {
            _dbName = dbName;
            InitDataBase();
        }

        public void Dispose()
        {
            CleanDataBase();
        }

        protected void CleanDataBase()
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        protected void InitDataBase()
        {
            InitEmptyDataBase();

            _sammy = new User()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                UserName = "Sammy"
            };

            using (var context = new DaGetContext(_dbContextOptions))
            {
                context.Users.Add(_sammy);

                context.Commit();
            }
        }

        protected void ResetDataBase()
        {
            CleanDataBase();
            InitDataBase();
        }

        protected void InitEmptyDataBase()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                    .UseInMemoryDatabase(databaseName: _dbName)
                                    .Options;
        }
    }
}
