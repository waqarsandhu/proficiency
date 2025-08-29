using PM.Common.Common;

namespace PM.Common.Dto
{
    public class GetProductInputDto:PagedAndSortedResultRequest
    {
        public string? Search { get; set; }
    }
}
