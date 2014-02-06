using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.StatisticsTests
{
    public class SnapshotDbDump
    {
        public void Dump(string setName, IEnumerable<ISnapshot> snapshots)
        {
            var testSet = new TestSet
            {
                Stamp = DateTime.Now, 
                Name = setName,
                UnitTests = snapshots.Select(t => new UnitTest
                {
                    Name = t.UnitTest.DeclaredName,
                    Librarians = t.Librarians.Count,
                    Stubs = t.Stubs.Count,
                    Mocks = t.Mocks.Count,
                    Targets = t.Targets.Count,
                    Services = t.Services.Count,
                    FakeProperties = t.FakeProperties.Count,
                    FakeMethods = t.FakeMethods.Count,
                    FakeCalbacks = t.FakeCallbacks.Count,
                    FakeExceptions = t.FakeExceptions.Count,

                }).ToList()
            };

            Dump(testSet);
        }

        private void Dump(TestSet set)
        {
            using (var session = new Session())
            {
                var removesets = session.TestSets.Where(t => t.Name == set.Name);
                session.TestSets.RemoveRange(removesets);

                session.TestSets.Add(set);
                session.SaveChanges();
            }
        }
    }

    internal class Session : DbContext
    {
        public Session()
            : base("MockMetricsStat")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<TestSet> TestSets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestSet>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<UnitTest>()
                .HasKey(t => t.Id)
                .HasRequired(t => t.TestSet)
                .WithMany(t => t.UnitTests)
                .WillCascadeOnDelete(true);
        }


    }

    public class TestSet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Stamp { get; set; }

        public List<UnitTest> UnitTests { get; set; }
    }

    public class UnitTest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Librarians { get; set; }

        public int Stubs { get; set; }

        public int Mocks { get; set; }

        public int Targets { get; set; }

        public int Services { get; set; }

        public int FakeProperties { get; set; }

        public int FakeMethods { get; set; }

        public int FakeCalbacks { get; set; }

        public int FakeExceptions { get; set; }

        public TestSet TestSet { get; set; }
    }

}
