namespace HRMS.Application.Wrappers;

/// <summary>
/// Represents an error with an associated error code, description, and optional field name.
/// </summary>
public class Error(ErrorCode errorCode, string description = null, string fieldName = null)
{
    /// <summary>
    /// Gets or sets the error code associated with the error.
    /// </summary>
    public ErrorCode ErrorCode { get; set; } = errorCode;

    /// <summary>
    /// Gets or sets the name of the field related to the error, if applicable.
    /// </summary>
    public string FieldName { get; set; } = fieldName;

    /// <summary>
    /// Gets or sets the description of the error.
    /// </summary>
    public string Description { get; set; } = description;
}
