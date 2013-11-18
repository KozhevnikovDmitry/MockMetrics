using System.Collections.Generic;

namespace Tested
{
    public class Aggregator
    {
        private readonly IDepend _depend;
        private readonly IAnother _another;

        public Aggregator(IDepend depend, IAnother another)
        {
            _depend = depend;
            _another = another;
        }

        public Master Aggregate(ISome some)
        {
            return some.Get();
        }

        public IEnumerable<Detail> Aggregate(IService service)
        {
            return service.Details;
        }

        public int Aggregate(string intoCalabanga)
        {
            _depend.Caramba(_another);
            return _another.Calabanga(intoCalabanga);
        }

        public Master GetMaster(Detail detail)
        {
            return detail.Master;
        }
    }

    public interface IService
    {
        IEnumerable<Detail> Details { get; set; }
    }

    public interface ISome
    {
        Master Get();
    }

    public class Master
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Detail> Details { get; set; }
    }

    public class Detail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Master Master { get; set; }
    }

    public interface IAnother
    {
        int Calabanga(string s);
    }

    public interface IDepend
    {
        void Caramba(IAnother another);
    }
}
