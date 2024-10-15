namespace VotaFacil.Apllication.DTO
{
    public class ResultadoVotacaoDTO
    {
        public IEnumerable<EleicaoDTO> EleicoesConcluidas { get; set; }
        public IEnumerable<CandidatoDTO> Candidatos { get; set; }
    }
}
