
namespace DO;
/// <summary>
/// Dal Does Not Exist Exception
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
/// <summary>
/// Dal Already Exists Exception
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// Dal Deletion Impossible
/// </summary>
[Serializable]
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }
}
/// <summary>
/// Dal XMLFile Load Create Exception
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
/// <summary>
/// Dal XMLFormat Exception
/// </summary>
[Serializable]
public class DalXMLFormatException : Exception
{
    public DalXMLFormatException(string? message) : base(message) { }
}
/// <summary>
/// wrong Input
/// </summary>
[Serializable]
public class wrongInput : Exception
{
    public wrongInput(string? message) : base(message) { }
}


