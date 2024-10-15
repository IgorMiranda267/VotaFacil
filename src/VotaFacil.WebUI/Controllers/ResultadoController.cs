using Microsoft.AspNetCore.Mvc;
using System.Text;
using VotaFacil.Apllication.DTO;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.WebUI.Controllers
{
    public class ResultadoController : Controller
    {
        private readonly EleicaoFacade _eleicao;
        private readonly VotoFacade _voto;

        public ResultadoController(EleicaoFacade eleicao, VotoFacade voto)
        {
            _eleicao = eleicao;
            _voto = voto;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<EleicaoModel> eleicoesConcluidas = await _eleicao.ObterTodasEleicoes();
            var eleicoesComCandidatos = eleicoesConcluidas
                .Where(e => e.Candidatos != null && e.Candidatos.Any())
                .Select(e => new EleicaoModel
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Descricao = e.Descricao,
                    ContractAddress = e.ContractAddress,
                    TransactionHash = e.TransactionHash,
                    BlockNumberString = e.BlockNumberString,
                    GasUsedString = e.GasUsedString,
                    Candidatos = e.Candidatos,
                    Votos = e.Votos,
                    Status = e.Fim > DateTime.Now ? "Em Andamento" : "Finalizada"
                })
            .ToList();

            return View("ResultadoVotacao", eleicoesComCandidatos);
        }

        public async Task<IActionResult> GetCandidatoss(Guid eleicaoId)
        {
            var candidatos = await _eleicao.BuscarCandidatoPorEleicao(eleicaoId);
            return PartialView("CandidatosPartial", candidatos);
        }

        public async Task<IActionResult> GetCandidatos(Guid eleicaoId)
        {
            var candidatos = await _eleicao.BuscarCandidatoPorEleicao(eleicaoId);
            if (candidatos == null)
            {
                return Content("<p>Nenhum candidato encontrado para esta eleição.</p>");
            }

            var candidatosHtml = new StringBuilder();
            foreach (var candidato in candidatos)
            {
                var fotoUrl = !string.IsNullOrEmpty(candidato.FotoPath) ? candidato.FotoPath : Url.Content("~/images/default.jpg");
                candidatosHtml.Append($@"
                <div class='col-md-4'>
                    <div class='card mb-4'>
                        <img src='{fotoUrl}' class='card-img-top' alt='Foto do Candidato'>
                        <div class='card-body'>
                            <h5 class='card-title'>{candidato.Nome}</h5>
                            <p class='card-text'>{candidato.Descricao}</p>
                            <p class='card-text'><strong>Votos: {candidato.Votos.Count}</strong></p>
                        </div>
                    </div>
                </div>");
            }

            return Content(candidatosHtml.ToString(), "text/html");
        }

        public async Task<IActionResult> GetVotos(Guid candidatoId)
        {
            var votos = await _voto.ObterVotosPorEleicao(candidatoId);
            return PartialView("VotosPartial", votos);
        }
    }
}
