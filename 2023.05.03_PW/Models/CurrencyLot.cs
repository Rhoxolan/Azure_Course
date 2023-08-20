namespace _2023._05._03_PW.Models
{
    public record CurrencyLot
    {
        public required CurrencyType CurrencyType { get; init; }

        public required int Amount { get; init; }

        public required string SellerLastName { get; init; }
    }
}
