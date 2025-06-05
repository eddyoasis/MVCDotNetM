using Microsoft.EntityFrameworkCore;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;

namespace MVCWebApp.Helper
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }  // current page number
        public int TotalPages { get; private set; } // total available pages
        public int PageSize { get; private set; }      // Number of items per page
        public int TotalItems { get; private set; }
        public List<T> Items { get; set; } = [];

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
            Items.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static async Task<PaginatedList<TDestination>> GetByPagesAndBaseAsync<TSource, TDestination>(
            IQueryable<TSource> source,
            IMapModel mapper,
            BaseSearchReq req = null) where TSource : ISetUserInfo
        {
            if(!req.IsNullOrEmpty())
            {
                source = source.Where(x =>
                    ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) || 
                        (req.IsSearchByCreatedDate ?
                                x.CreatedAt >= req.DateFrom && x.CreatedAt < req.DateTo.AddDays(1) :
                                x.ModifiedAt >= req.DateFrom && x.ModifiedAt < req.DateTo.AddDays(1))) &&
                    (req.CreatedBy.IsNullOrEmpty() || x.CreatedBy.ToLower().Contains(req.CreatedBy.ToLower())) &&
                    (req.ModifiedBy.IsNullOrEmpty() || x.ModifiedBy.ToLower().Contains(req.ModifiedBy.ToLower()))
                );
            }

            var count = await source.CountAsync();

            var items = await source.Skip((req.PageNumber - 1) * req.PageSize)
                                    .Take(req.PageSize)
                                    .ToListAsync();

            var mappedItems = mapper.MapDto<List<TDestination>>(items);

            return new PaginatedList<TDestination>(mappedItems, count, req.PageNumber, req.PageSize);
        }

        //public static async Task<PaginatedList<TDestination>> GetByPagesAndBaseUpdateAsync<TSource, TDestination>(
        //    IQueryable<TSource> source,
        //    IMapModel mapper,
        //    BaseSearchReq req = null) where TSource : ISetUpdateInfo
        //{
        //    if (!req.IsNullOrEmpty())
        //    {
        //        source = source.Where(x =>
        //            ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
        //                (req.IsSearchByCreatedDate ?
        //                        true :
        //                        x.ModifiedAt >= req.DateFrom && x.ModifiedAt < req.DateTo.AddDays(1))) &&
        //            (req.ModifiedBy.IsNullOrEmpty() || x.ModifiedBy.ToLower().Contains(req.ModifiedBy.ToLower()))
        //        );
        //    }

        //    var count = await source.CountAsync();

        //    var items = await source.Skip((req.PageNumber - 1) * req.PageSize)
        //                            .Take(req.PageSize)
        //                            .ToListAsync();

        //    var mappedItems = mapper.MapDto<List<TDestination>>(items);

        //    return new PaginatedList<TDestination>(mappedItems, count, req.PageNumber, req.PageSize);
        //}

        public static async Task<PaginatedList<TDestination>> GetByPagesAsync<TSource, TDestination>(
            IQueryable<TSource> source,
            IMapModel mapper,
            BaseSearchReq req = null)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((req.PageNumber - 1) * req.PageSize)
                                    .Take(req.PageSize)
                                    .ToListAsync();

            var mappedItems = mapper.MapDto<List<TDestination>>(items);

            return new PaginatedList<TDestination>(mappedItems, count, req.PageNumber, req.PageSize);
        }

        //public static PaginatedList<T> CreateAsync(IEnumerable<T> source, int pageIndex, int pageSize)
        //{
        //    var count = source.Count();

        //    var items = source.Skip((pageIndex - 1) * pageSize)
        //                            .Take(pageSize)
        //                            .ToList();

        //    return new PaginatedList<T>(items, count, pageIndex, pageSize);
        //}
    }
}
