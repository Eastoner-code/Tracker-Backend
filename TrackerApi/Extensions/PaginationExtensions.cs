using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Extensions
{
    public static class PaginationExtensions
    {
        public static string LowerForDb(this string src)
        {
            if (!string.IsNullOrWhiteSpace(src))
            {
                return src.ToLower().Trim();
            }

            return string.Empty;
        }

        public static PagedResult<T> GetPaged<T>(this IList<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPagedWhere<T>(this IQueryable<T> query, Func<T, bool> filter, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize
            };
            query = (IQueryable<T>)query.Where(filter);
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPagedWhereAndOrder<T>(this IQueryable<T> query, Func<T, bool> filter, int page, int pageSize, Func<T, object> order) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize
            };
            query = (IQueryable<T>)query.Where(filter);
            query = (IQueryable<T>)query.OrderBy(order);
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<TU> GetPaged<T, TU>(this IQueryable<T> query, int page, int pageSize, IMapper mapper) where TU : class
        {
            var result = new PagedResult<TU>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            try
            {

                result.Results = mapper.Map<List<TU>>(query.Skip(skip)
                    .Take(pageSize)
                    .ToList());
                return result;
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public static PagedResult<TU> GetPagedOrder<T, TU>(this IQueryable<T> query, int page, int pageSize, Func<T, object> order, IMapper mapper) where TU : class
        {
            var result = new PagedResult<TU>
            {
                CurrentPage = page,
                PageSize = pageSize
            };
            query = (IQueryable<T>)query.OrderBy(order);
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            try
            {

                result.Results = mapper.Map<List<TU>>(query.Skip(skip)
                    .Take(pageSize)
                    .ToList());
                return result;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        public static PagedResult<TU> GetPagedWhere<T, TU>(this IQueryable<T> query, Func<T, bool> filter, int page, int pageSize, IMapper mapper) where TU : class
        {
            var result = new PagedResult<TU>
            {
                CurrentPage = page,
                PageSize = pageSize
            };
            query = (IQueryable<T>)query.Where(filter);
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            try
            {

                result.Results = mapper.Map<List<TU>>(query.Skip(skip)
                    .Take(pageSize)
                    .ToList());
                return result;
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public static PagedResult<TU> GetPagedWhereAndOrder<T, TU>(this IQueryable<T> query, Func<T, bool> filter,
            int page, int pageSize, Func<T, object> order, IMapper mapper) where TU : class
        {
            var result = new PagedResult<TU>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            query = query.Where(filter).AsQueryable();
            query = query.OrderBy(order).AsQueryable();
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            try
            {

                result.Results = mapper.Map<List<TU>>(query.Skip(skip)
                    .Take(pageSize)
                    .ToList());
                return result;
            }
            catch (Exception ex)
            {
                // ignored
            }

            return null;
        }
    }
}
