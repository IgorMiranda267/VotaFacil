namespace VotaFacil.Domain.Validacao
{
    public class ValidacaoDeExcecaoDominio : Exception
    {
        public ValidacaoDeExcecaoDominio(string error) : base(error)
        { }
    
        public static void When(bool hasError, string error)
        {
            if (hasError)
                throw new ValidacaoDeExcecaoDominio(error);
        }
    
    }
}
