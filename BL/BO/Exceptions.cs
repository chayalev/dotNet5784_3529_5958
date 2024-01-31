
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
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
}
/// <summary>
/// BL Wrong Input
/// </summary>
[Serializable]
public class BlWrongInput : Exception
{
    public BlWrongInput(string? message) : base(message) { }
}

