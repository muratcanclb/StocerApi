using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Intra.Api.Infrastructure.Cache;
using Meb.Core.Domain.Search;
using Meb.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Intra.Api.Infrastructure.Helpers
{
    public class SearchHelper<T> : Controller where T : class
    {

        public class SearchResponseModel
        {
            public int TotalCount { get; set; }
            public IEnumerable<T> Data { get; set; }

        }


        public SearchResponseModel Search(HttpContext httpContext, IQueryable<T> table, IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        {
            if (searchModels != null && searchModels.Any()) table = table.Where(searchModels);


            //httpContext.Response.Headers.Add("X-TOTAL-COUNT", table.Count().ToString());
            int tmpCount = table.Count();

            if (!string.IsNullOrEmpty(sort))
            {
                table = table.Sort(sort).PagedList(pagesize, pagenumber);
            }
            else
            {
                //return table.AsEnumerable().PagedListEnumerable(pagesize, pagenumber);

                return new SearchResponseModel
                {
                    Data = table.AsEnumerable().PagedListEnumerable(pagesize, pagenumber),
                    TotalCount = tmpCount
                };
            }

            return new SearchResponseModel
            {
                Data = table,
                TotalCount = tmpCount
            };
        }

        public IEnumerable<T> Search(IQueryable<T> table, int pagesize, int pagenumber, string sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                table = table.Sort(sort).PagedList(pagesize, pagenumber);
            }
            else
            {
                return table.AsEnumerable().PagedListEnumerable(pagesize, pagenumber);
            }
            return table;
        }

        public SearchResponseModel Search(HttpContext httpContext, IQueryable<T> table, IEnumerable<SearchModel> searchModels)
        {
            return Search(HttpContext, table, searchModels, 0, 0, "");
        }

        //public class SearchHelper<T> where T : class, new()
        //{
        //    private static SearchHelper<T> instance = null;
        //    private static readonly object _lock = new object();

        //    private SearchHelper()
        //    {
        //    }

        //    public static SearchHelper<T> Instance
        //    {
        //        get
        //        {
        //            lock (_lock)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new SearchHelper<T>();
        //                }
        //                return instance;
        //            }
        //        }
        //    }

        //    public IEnumerable<T> Search(HttpContext httpContext, IQueryable<T> table, IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        //    {
        //        if (searchModels != null && searchModels.Any()) table = table.Where(searchModels);
        //        httpContext.Response.Headers.Add(ConstantValues.XTotalCount, table.Count().ToString());
        //        table = SortHelper<T>.Instance.Sort(table, pagesize, pagenumber, sort);
        //        return table;
        //    }

        //    public IEnumerable<T> Search(HttpContext httpContext, IQueryable<T> table, IEnumerable<SearchModel> searchModels)
        //    {
        //        return Search(httpContext, table, searchModels, 0, 0, "");
        //    }

        //    public IQueryable<T> Search(IQueryable<T> table, IEnumerable<SearchModel> searchModels, HttpContext httpContext = null)
        //    {
        //        if (searchModels != null && searchModels.Any()) table = table.Where(searchModels);
        //        if (httpContext != null) httpContext.Response.Headers.Add(ConstantValues.XTotalCount, table.Count().ToString());
        //        return table;
        //    }
    }
}