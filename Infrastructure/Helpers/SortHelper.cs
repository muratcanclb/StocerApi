using System.Linq;
using Meb.Core.Search;

namespace Intra.Api.Infrastructure.Helpers
{
    public class SortHelper<T> where T : class, new()
    {
        private static SortHelper<T> _instance;
        private static readonly object _lock = new object();
        public static SortHelper<T> Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SortHelper<T>();
                    }
                    return _instance;
                }
            }
        }

        private SortHelper()
        {

        }

        public IQueryable<T> Sort(IQueryable<T> list, int pagesize, int pagenumber, string sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                list = list.Sort(sort).PagedList(pagesize, pagenumber);
            }
            else
            {
                list = list.AsEnumerable().PagedListEnumerable(pagesize, pagenumber).AsQueryable();
            }
            return list;
        }
    }
}
