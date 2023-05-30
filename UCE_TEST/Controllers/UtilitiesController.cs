using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using UCE_TEST.Models.DTOs;

namespace UCE_TEST.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UtilitiesController : ControllerBase
    {
		public UtilitiesController() {}

		internal static async Task<SelectList> GetProvincesDO()
		{
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            var response = await client.GetStringAsync("http://provinciasrd.raydelto.org/provincias");
            ProvinceResponse provinces = JsonConvert.DeserializeObject<ProvinceResponse>(response);

             return new SelectList(provinces.Data.Select(x => x.Nombre));
        }
	}
}

