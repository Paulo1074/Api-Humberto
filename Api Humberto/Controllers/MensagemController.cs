using Microsoft.AspNetCore.Mvc;

namespace MeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        // DTO para receber os dados da pessoa
        public class PessoaDTO
        {
            public string Nome { get; set; }
            public double Peso { get; set; } // em kg
            public double Altura { get; set; } // em metros
        }

        // DTO para o retorno
        public class ImcResultado
        {
            public string Nome { get; set; }
            public double IMC { get; set; }
        }

        // a) Ação para calcular o IMC
        [HttpPost("calcular-imc")]
        public IActionResult CalcularIMC([FromBody] PessoaDTO pessoa)
        {
            if (pessoa.Peso <= 0 || pessoa.Altura <= 0)
                return BadRequest("Peso e altura devem ser maiores que zero.");

            double imc = pessoa.Peso / (pessoa.Altura * pessoa.Altura);

            var resultado = new ImcResultado
            {
                Nome = pessoa.Nome,
                IMC = Math.Round(imc, 2) // arredonda para 2 casas decimais
            };

            return Ok(resultado);
        }

        // b) Ação para consultar a tabela do IMC
        [HttpGet("consulta-tabela-imc")]
        public IActionResult ConsultaTabelaIMC([FromQuery] double imc)
        {
            string classificacao;

            if (imc < 18.5)
                classificacao = "Abaixo do peso";
            else if (imc >= 18.5 && imc < 24.9)
                classificacao = "Peso normal";
            else if (imc >= 25 && imc < 29.9)
                classificacao = "Sobrepeso";
            else if (imc >= 30 && imc < 34.9)
                classificacao = "Obesidade Grau I";
            else if (imc >= 35 && imc < 39.9)
                classificacao = "Obesidade Grau II";
            else
                classificacao = "Obesidade Grau III (mórbida)";

            return Ok(new
            {
                IMC = imc,
                Classificacao = classificacao
            });
        }
    }
}
