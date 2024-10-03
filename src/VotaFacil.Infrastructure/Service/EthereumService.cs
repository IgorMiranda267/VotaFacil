using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Signer;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;

namespace VotaFacil.Infrastructure.Service
{
    public class EthereumService : IEthereumService
    {
        private readonly Web3 _web3;
        private static readonly HttpClient client = new HttpClient();
        private readonly string infuraUrl = "https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID";
        private readonly string _contractAddress;
        private readonly string _accountAddress;
        private readonly string _privateKey;

        public EthereumService(string url, string contractAddress, string accountAddress, string privateKey)
        {
            _web3 = new Web3(new ContaEthereumModel(privateKey), url);
            _contractAddress = contractAddress;
            _accountAddress = accountAddress;
            _privateKey = privateKey;
        }

        public async Task<string> GetLatestBlockAsync()
        {
            var response = await client.PostAsync(infuraUrl, new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_blockNumber\",\"params\":[],\"id\":1}"));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            return json["result"].ToString();
        }

        public async Task<string> GetLatestBlockHashAsync()
        {
            var response = await client.PostAsync(infuraUrl, new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBlockByNumber\",\"params\":[\"latest\", false],\"id\":1}"));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            return json["result"]["hash"].ToString();
        }

        public async Task<string> EnviarVotoAsync(Guid eleitorId, Guid opcaoVotoId, string hashAnterior, string numeroBloco)
        {
            var contrato = _web3.Eth.GetContract(ABI, _contractAddress);
            var funcaoVotar = contrato.GetFunction("votar");

            var gas = new HexBigInteger(3000000);
            var valor = new HexBigInteger(0);

            var txHash = await funcaoVotar.SendTransactionAsync(_accountAddress, gas, valor, eleitorId, opcaoVotoId, hashAnterior, numeroBloco);
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
                        { 'name': 'hashAnterior', 'type': 'string' },
                        { 'name': 'numeroBloco', 'type': 'string' }
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
