namespace Application.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public IEnumerable<T>? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int? StatusCode {get;set;} = 400;
    public Result()
    {
        Success = true;
    }
}
