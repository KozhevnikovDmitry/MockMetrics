using System.Linq;
using Moq;
using NUnit.Framework;

namespace Tested.Tests.FooTests
{
    public interface IEntity
    {
        int Id { get; }

        string Name { get; }
    }

    [TestFixture]
    public class LinqQuerySyntaxTests
    {
        [Test]
        public void LinqQuerySyntaxTest()
        {
            var query = new MockRepository(MockBehavior.Default).Of<IEntity>();
            var more = new MockRepository(MockBehavior.Default).Of<IEntity>();

            var q1 = from t in query
                     select t ;

            var q01 = from t in query
                      let name = t.Name
                      select name;

            var q2 = from t in query
                     where t.Id == 1
                     select t;

            var q3 = from t in query
                     where t.Id == 1
                     orderby t.Name descending
                     select t;

            var q4 = from t in query
                     where t.Id == 1
                     orderby t.Name descending , t.Id descending
                     select t;

            var q5 = from t1 in query
                     from t2 in more
                     select t2;

            var q6 = from t1 in query
                     join t2 in more on t1.Id equals t2.Id
                     select t2;

            var q61 = from t1 in query
                      join t2 in more on t1.Id equals t2.Id into t4
                      select t4;

            var q7 = from t in query
                     group t by t.Name into t1
                     select t1;

            var q71 = from t in query
                      group new { t.Id, t.Name } by t.Name into t1
                      select t1;

            var q8 = from t in query
                     group t by t.Name into t1
                     join t2 in more on t1.Key equals t2.Name
                     where t2.Id == 1
                     select t1;
        }
    }
}
