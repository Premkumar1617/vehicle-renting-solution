using Microsoft.AspNetCore.Mvc;

namespace VehicleRentingApi.DTO
{
    public class ApiResponse<T>:ActionResult
    {
        public ApiResponse(T data, int statusCode=200)
        {
            StatusCode = statusCode;
            Data = data;
            CurrentTime = DateTime.Now;
        }

        public int StatusCode { get; set; } 
        public T Data { get; set; }

        public DateTime CurrentTime { get; set; }
    }
}
