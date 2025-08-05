namespace HRMS.Application.Wrappers;

/// <summary>
/// Defines error codes for application responses.
/// </summary>
public enum ErrorCode : short
{
    /// <summary>
    /// Indicates that the model state is not valid.
    /// </summary>
    ModelStateNotValid = 0,

    /// <summary>
    /// Indicates that a field contains invalid data.
    /// </summary>
    FieldDataInvalid = 1,

    /// <summary>
    /// Indicates that the requested resource was not found.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// Indicates that access to the resource is denied.
    /// </summary>
    AccessDenied = 3,

    /// <summary>
    /// Represents an error related to identity management.
    /// </summary>
    ErrorInIdentity = 4,

    /// <summary>
    /// Represents a general exception.
    /// </summary>
    Exception = 5,
    
    DuplicateResource = 6,
    Conflict = 7,
    DependencyFailure = 8,
    ValidationFailed = 9,
    UnauthorizedAction = 10,
    OperationNotAllowed = 11,
}
