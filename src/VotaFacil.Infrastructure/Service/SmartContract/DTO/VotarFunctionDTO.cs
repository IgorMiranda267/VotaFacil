using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotaFacil.Infrastructure.Service.SmartContract.DTO
{
    public class VotarFunctionDTO : FunctionMessage
    {
        [Parameter("bytes32", "eleitorId", 1)]
        public byte[] EleitorId { get; set; }

        [Parameter("bytes32", "opcaoVotoId", 2)]
        public byte[] OpcaoVotoId { get; set; }

        [Parameter("string", "hashAnterior", 3)]
        public string HashAnterior { get; set; }

        [Parameter("string", "numeroBloco", 4)]
        public string NumeroBloco { get; set; }
    }
}
