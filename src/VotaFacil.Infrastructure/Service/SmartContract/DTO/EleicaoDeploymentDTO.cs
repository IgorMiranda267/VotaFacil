using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace VotaFacil.Infrastructure.Service.SmartContract.DTO
{
    internal class EleicaoDeploymentDTO : ContractDeploymentMessage
    {
        public static string BYTECODE = "YOUR_CONTRACT_BYTECODE";
        public EleicaoDeploymentDTO() : base(BYTECODE) { }
        [Parameter("bytes32", "_id", 1)]
        public string Id { get; set; }
        [Parameter("string", "_nome", 2)]
        public string Nome { get; set; }
    }
}
