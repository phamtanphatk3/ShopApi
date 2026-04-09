using Microsoft.AspNetCore.Mvc;
using ShopApi.DTOs.Installment;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/installments")]
    public class InstallmentsController : ControllerBase
    {
        private readonly InstallmentService _service;

        public InstallmentsController(InstallmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstallmentCreateDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }
    }
}
