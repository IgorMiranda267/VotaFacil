using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    public class PeriodoVotacaoModel
    {
        public DateTime Inicio { get; private set; }
        public DateTime Fim { get; private set; }

        public  PeriodoVotacaoModel(DateTime inicio, DateTime fim)
        {
            ValidacaoDeExcecaoDominio.When(inicio == default(DateTime), "A data de início não pode ser vazia.");
            ValidacaoDeExcecaoDominio.When(fim == default(DateTime), "A data de fim não pode ser vazia.");

            Inicio = inicio;
            Fim = fim;
        }

        public bool EstaDentroDoPeriodo(DateTime dataAtual)
        {
            return dataAtual >= Inicio && dataAtual <= Fim;
        }
    }
}
