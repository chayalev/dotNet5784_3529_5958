
namespace BO;
/// <summary>
/// Bl Does Not Exist Exception
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// Bl Already Exists Exception
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}
/// <summary>
/// Bl Null Property Exception
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
/// <summary>
/// BL Deletion Impossible
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }
}
/// <summary>
/// BL Wrong Input
/// </summary>
[Serializable]
public class BlWrongInputException : Exception
{
    public BlWrongInputException(string? message) : base(message) { }
}
/// <summary>
/// BL Bl Cant Change After Scheduled
/// </summary>
[Serializable]
public class BlCantChangeAfterScheduledException : Exception
{
    public BlCantChangeAfterScheduledException(string? message) : base(message) { }
}
/// <summary>
/// bl wrong date
/// </summary>
[Serializable]
public class BlWrongDateException : Exception
{
    public BlWrongDateException(string? message) : base(message) { }
}

