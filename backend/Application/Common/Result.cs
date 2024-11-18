namespace Application.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public IEnumerable<T>? Data { get; set; }
     public T? SingleData { get; set; }
    public string? Message { get; set; }
    public Result()
    {
        Success = true;
    }
}
