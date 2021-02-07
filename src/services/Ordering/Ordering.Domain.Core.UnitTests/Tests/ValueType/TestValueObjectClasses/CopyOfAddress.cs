using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject;

namespace Ordering.Domain.Core.UnitTests
{
    public class CopyOfAddress : ValueObject
    {

        public CopyOfAddress(string addressLine1, string addressLine2)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
        }

        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string PostalCode { get; init; }
    }
}
