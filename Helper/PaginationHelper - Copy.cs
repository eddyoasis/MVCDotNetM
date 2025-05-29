﻿//using Microsoft.EntityFrameworkCore;

//namespace MVCWebApp.Helper
//{
//    public class PaginatedList<T> : List<T>
//    {
//        public int PageIndex { get; private set; }  // current page number
//        public int TotalPages { get; private set; } // total available pages
//        public int PageSize { get; private set; }      // Number of items per page
//        public int TotalItems { get; private set; }

//        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
//        {
//            PageIndex = pageIndex;
//            PageSize = pageSize;
//            TotalItems = count;
//            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
//            this.AddRange(items);
//        }

//        public bool HasPreviousPage => PageIndex > 1;

//        public bool HasNextPage => PageIndex < TotalPages;

//        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
//        {
//            var count = await source.CountAsync();

//            var items = await source.Skip((pageIndex - 1) * pageSize)
//                                    .Take(pageSize)
//                                    .ToListAsync();

//            return new PaginatedList<T>(items, count, pageIndex, pageSize);
//        }

//        public static PaginatedList<T> CreateAsync(IEnumerable<T> source, int pageIndex, int pageSize)
//        {
//            var count = source.Count();

//            var items = source.Skip((pageIndex - 1) * pageSize)
//                                    .Take(pageSize)
//                                    .ToList();

//            return new PaginatedList<T>(items, count, pageIndex, pageSize);
//        }
//    }
//}
