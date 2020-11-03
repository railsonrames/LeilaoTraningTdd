using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiz.leilao.API.Handler;
using Wiz.leilao.API.Services.Interfaces;
using Wiz.leilao.API.ViewModels.Customer;

namespace Wiz.leilao.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = SSOAuthenticationOptions.DefaultScheme)]
    [Produces("application/json")]
    [Route("api/v1/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService customerService)
        {
            _produtoService = customerService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> GetAll()
        {
            return Ok(await _produtoService.GetAllAsync());
        }
    }
}