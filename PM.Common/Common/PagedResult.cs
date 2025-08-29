using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace PM.Common.Common
{
    public interface IHasTotalCount
    {
        // Summary:
        //     Total count of Items.
        int TotalCount { get; set; }
    }

    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }

    public interface IListResult<T>
    {
        // Summary:
        //     List of items.
        IReadOnlyList<T> Items { get; set; }
    }

    [Serializable]
    public class PagedResult<T> : ListResult<T>, IPagedResult<T>
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        //public PagedResult()
        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>

        public PagedResult(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }

    [Serializable]
    public class PagedResultWithProgress<T> : ListResult<T>, IPagedResult<T>
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>

        public int TotalCount { get; set; }
        /// <summary>
        /// Mapping progress (0–100).
        /// </summary>

        /// <summary>
        /// Creates a new <see cref="PagedResultWithProgress{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        /// <param name="progress">Progress of mapping</param>
        public PagedResultWithProgress(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }


    [Serializable]
    public class ListResult<T> : IListResult<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>

        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }

        private IReadOnlyList<T> _items;
        /// <summary>
        /// Creates a new <see cref="ListResult{T}"/> object.
        /// </summary>
        //public ListResult()
        //{
        //}
        /// <summary>
        /// Creates a new <see cref="ListResult{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>

        public ListResult(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }

    public interface ISortedResultRequest
    {
        string SortBy { get; set; }
        string SortOrder { get; set; }
    }


    [Serializable]
    public class PagedAndSortedResultRequest : PagedResultRequest, ISortedResultRequest
    {
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }

    [Serializable]
    public class PagedResultRequest : IPagedRequest
    {

        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]

        public int Page { get; set; } = Constants.DefaultPage;

        [Range(1, 200, ErrorMessage = "PageSize must be between 1 and 200.")]

        public int PageSize { get; set; } = Constants.DefaultPageSize;

    }
}
