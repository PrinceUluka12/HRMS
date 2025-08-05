namespace HRMS.Application.Wrappers;

// Represents a base result for handling success and failure scenarios
public class BaseResult
{
    // Indicates whether the operation was successful
    public bool Success { get; set; }

    // Stores a list of errors if the operation fails
    public List<Error> Errors { get; set; }

    // Creates a successful result
    public static BaseResult Ok()
        => new() { Success = true };

    // Creates a failure result without specifying an error
    public static BaseResult Failure()
        => new() { Success = false };

    // Creates a failure result with a single error
    public static BaseResult Failure(Error error)
        => new() { Success = false, Errors = [error] };

    // Creates a failure result with multiple errors
    public static BaseResult Failure(IEnumerable<Error> errors)
        => new() { Success = false, Errors = errors.ToList() };

    // Implicitly converts a single error to a failure result
    public static implicit operator BaseResult(Error error)
        => new() { Success = false, Errors = [error] };

    // Implicitly converts a list of errors to a failure result
    public static implicit operator BaseResult(List<Error> errors)
        => new() { Success = false, Errors = errors };

    // Adds an error to the result and marks it as a failure
    public BaseResult AddError(Error error)
    {
        Errors ??= []; // Ensure the list is initialized before adding
        Errors.Add(error);
        Success = false;
        return this;
    }
}


// Generic version of BaseResult that contains a result data type
public class BaseResult<TData> : BaseResult
{
    // The actual result data
    public TData Data { get; set; }

    // Creates a successful result with data
    public static BaseResult<TData> Ok(TData data)
        => new() { Success = true, Data = data };

    // Creates a failure result without specifying an error
    public new static BaseResult<TData> Failure()
        => new() { Success = false };

    // Creates a failure result with a single error
    public new static BaseResult<TData> Failure(Error error)
        => new() { Success = false, Errors = [error] };

    // Creates a failure result with multiple errors
    public new static BaseResult<TData> Failure(IEnumerable<Error> errors)
        => new() { Success = false, Errors = errors.ToList() };

    // Implicitly converts a successful data response to BaseResult
    public static implicit operator BaseResult<TData>(TData data)
        => new() { Success = true, Data = data };

    // Implicitly converts a single error to a failure result
    public static implicit operator BaseResult<TData>(Error error)
        => new() { Success = false, Errors = [error] };

    // Implicitly converts a list of errors to a failure result
    public static implicit operator BaseResult<TData>(List<Error> errors)
        => new() { Success = false, Errors = errors };
}

