namespace Restaurants.Domain.Exceptions;
//public class NotFoundException(string message)
//    : Exception(message)
//{
//}
public class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} with id: {resourceIdentifier} doesn't exist")
{
}
