using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Web3;
using VotaFacil.Domain.Model;

namespace VotaFacil.Infrastructure.Service
{
    public class EthereumService
    {
        private readonly Web3 _web3;
        private readonly string _contractAddress;
        private readonly string _accountAddress;
        private readonly string _privateKey;

        public EthereumService(string url, string contractAddress, string accountAddress, string privateKey)
        {
            _web3 = new Web3(new ContaEthereum(privateKey), url);
            _contractAddress = contractAddress;
            _accountAddress = accountAddress;
            _privateKey = privateKey;
        }

        public async Task<string> EnviarVotoAsync(Guid eleitorId, Guid opcaoVotoId, string hashAnterior)
        {
            var contrato = _web3.Eth.GetContract(ABI, _contractAddress);
            var funcaoVotar = contrato.GetFunction("votar");

            var gas = new HexBigInteger(3000000);
            var valor = new HexBigInteger(0);

            var txHash = await funcaoVotar.SendTransactionAsync(_accountAddress, gas, valor, eleitorId, opcaoVotoId, hashAnterior);
            return txHash;
        }

        public async Task<string> ConsultarVotoAsync(Guid eleitorId)
        {
            var contrato = _web3.Eth.GetContract(ABI, _contractAddress);
            var funcaoConsultarVoto = contrato.GetFunction("consultarVoto");

            var resultado = await funcaoConsultarVoto.CallAsync<string>(eleitorId);
            return resultado;
        }

        private const string ABI = @"[
            {
                'constant': false,
                'inputs': [
                    { 'name': 'eleitorId', 'type': 'bytes32' },
                    { 'name': 'opcaoVotoId', 'type': 'bytes32' },
                    { 'name': 'hashAnterior', 'type': 'string' }
                ],
                'name': 'votar',
                'outputs': [],
                'payable': false,
                'stateMutability': 'nonpayable',
                'type': 'function'
            },
            {
                'constant': true,
                'inputs': [
                    { 'name': 'eleitorId', 'type': 'bytes32' }
                ],
                'name': 'consultarVoto',
                'outputs': [
                    { 'name': '', 'type': 'string' }
                ],
                'payable': false,
                'stateMutability': 'view',
                'type': 'function'
            }
        ]";
    }
}
