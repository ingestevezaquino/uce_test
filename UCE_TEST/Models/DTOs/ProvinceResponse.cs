using System;
namespace UCE_TEST.Models.DTOs
{
    public class ProvinceResponse
    {
        public int Status { get; set; }
        public List<ProvinceData> Data { get; set; } = null!;
    }
}