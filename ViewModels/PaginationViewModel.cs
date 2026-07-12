namespace MrMohamedHassan.ViewModels;

public class PaginationViewModel
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public string? Action { get; set; }
    public string? Controller { get; set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
