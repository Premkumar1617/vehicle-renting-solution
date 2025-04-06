namespace VehicleRentingApi.DTO
{
    public class BookingDTO
    {
        public string CustomerName { get; set; } = null!;

        public string? CustomerCity { get; set; }

        public string? MobileNo { get; set; }

        public string? Email { get; set; }
        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public int Year { get; set; }

        public string Color { get; set; } = null!;

        public double DailyRate { get; set; }

        public string? CarImage { get; set; }

        public string? RegNo { get; set; }
        public int BookingId { get; set; }

        public int CustId { get; set; }

        public int CarId { get; set; }

        public DateTime BookingDate { get; set; }

        public int? Discount { get; set; }

        public double? TotalBillAmount { get; set; }

        public string? BookingUid { get; set; }
    }
}
