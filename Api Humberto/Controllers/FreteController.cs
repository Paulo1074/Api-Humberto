using Microsoft.AspNetCore.Mvc;

namespace MeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        public class ProdutoFreteDTO
        {
            public string NomeProduto { get; set; }
            public float Peso { get; set; } 
            public float Altura { get; set; }
            public float Largura { get; set; } 
            public float Comprimento { get; set; } 
            public string UF { get; set; } 
        }

        [HttpPost("calcular-frete")]
        public IActionResult CalcularFrete([FromBody] ProdutoFreteDTO produto)
        {
            if (string.IsNullOrWhiteSpace(produto.NomeProduto) ||
                produto.Altura <= 0 || produto.Largura <= 0 || produto.Comprimento <= 0 || produto.Peso <= 0)
            {
                return BadRequest("Todos os campos devem ser informados corretamente.");
            }

            float volume = produto.Altura * produto.Largura * produto.Comprimento;

            float taxaEstado;
            switch (produto.UF.ToUpper())
            {
                case "SP": taxaEstado = 50.00f; break;
                case "RJ": taxaEstado = 60.00f; break;
                case "MG": taxaEstado = 55.00f; break;
                default: taxaEstado = 70.00f; break;
            }

            float taxaPorCm3 = 0.01f; 

            float valorFrete = (volume * taxaPorCm3) + taxaEstado;

            return Ok(new
            {
                Produto = produto.NomeProduto,
                PesoKg = produto.Peso,
                VolumeCm3 = volume,
                Estado = produto.UF.ToUpper(),
                TaxaEstado = taxaEstado,
                TaxaPorCm3 = taxaPorCm3,
                ValorTotalFrete = Math.Round(valorFrete, 2)
            });
        }
    }
}
