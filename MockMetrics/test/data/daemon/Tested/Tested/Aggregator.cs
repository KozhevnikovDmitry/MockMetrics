using System.Collections.Generic;

namespace Tested
{
    public class Aggregator
    {
        private readonly IDepend _depend;
        private readonly IAnother _another;

        public IService Service { get; set; }

        public int Serial { get; set; }

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

        public IEnumerable<Detail> Aggregate()
        {
            return Service.Details;
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

        public bool Flag { get; set; }

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
        string Mamba { get; set; }

        int Calabanga(string s);

        void Calabanga();
    }

    public interface IDepend
    {
        void Caramba(IAnother another);
    }
}
