namespace VotaFacil.Domain.Entidades
{
    public class VotacaoModel
    {
        private readonly List<Guid> _votosRegistrados = new List<Guid>();
        public PeriodoVotacaoModel PeriodoVotacao { get; private set; }
        public List<VotoModel> OpcoesDeVoto { get; private set; } = new List<VotoModel>();

        public VotacaoModel(PeriodoVotacaoModel periodoVotacao, List<VotoModel> opcoesDeVoto)
        {
            PeriodoVotacao = periodoVotacao;
            OpcoesDeVoto = opcoesDeVoto;
        }

        public VotacaoModel(EleitorModel votante, VotoModel opcaoVoto)
        {
            if (!PeriodoVotacao.EstaDentroDoPeriodo(DateTime.Now))
                throw new Exception("Fora do período de votação.");

            if (_votosRegistrados.Contains(votante.Id))
                throw new Exception("Votante já votou.");

            if (!OpcoesDeVoto.Any(o => o.Id == opcaoVoto.Id))
                throw new Exception("Opção de voto inválida.");

            // Registrar voto (na vida real, aqui você registraria a transação na blockchain)
            _votosRegistrados.Add(votante.Id);
        }
    }
}
