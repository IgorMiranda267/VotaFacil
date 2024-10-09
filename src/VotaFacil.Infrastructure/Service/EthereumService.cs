using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;
using VotaFacil.Infrastructure.Service.SmartContract.DTO;

namespace VotaFacil.Infrastructure.Service
{
    public class EthereumService : IEthereumService
    {
        private readonly Web3 _web3;
        private static readonly HttpClient client = new HttpClient();
        private readonly string _infuraUrl;
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
        private readonly string _contractAddress;
        private readonly string _accountAddress;
        private readonly string _privateKey;

        public EthereumService(string url, string contractAddress, string accountAddress, string privateKey)
        {
            _infuraUrl = $"{Environment.GetEnvironmentVariable("INFURA_ETHEREUM_CONTRACT_ADDRESS")}{Environment.GetEnvironmentVariable("INFURA_ETHEREUM_ID_ACCOUNT")}";
            _web3 = new Web3(new ContaEthereumModel(privateKey), url);
            _contractAddress = contractAddress;
            _accountAddress = accountAddress;
            _privateKey = privateKey;
        }

        #region BLOCOS
        public async Task<string> GetLatestBlockAsync()
        {
            var response = await client.PostAsync(_infuraUrl, new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_blockNumber\",\"params\":[],\"id\":1}"));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            return json["result"].ToString();
        }

        public async Task<string> GetLatestBlockHashAsync()
        {
            var response = await client.PostAsync(_infuraUrl, new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBlockByNumber\",\"params\":[\"latest\", false],\"id\":1}"));
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            return json["result"]["hash"].ToString();
        }
        #endregion BLOCOS


        #region ELEIÇÂO
        public async Task<string> ConsultarVotoAsync(Guid eleitorId)
        {
            var contrato = _web3.Eth.GetContract(ABI, _contractAddress);
            var funcaoConsultarVoto = contrato.GetFunction("consultarVoto");

            var resultado = await funcaoConsultarVoto.CallAsync<string>(eleitorId);
            return resultado;
        }

        public async Task<string> EnviarVotoAsync(Guid eleitorId, Guid opcaoVotoId, string hashAnterior, string numeroBloco)

        {
            var contrato = _web3.Eth.GetContract(ABI, _contractAddress);
            var funcaoVotar = contrato.GetFunction("votar");

            var gas = new HexBigInteger(50000);
            var valor = new HexBigInteger(0);

            var txHash = await funcaoVotar.SendTransactionAsync(_accountAddress, gas, valor, eleitorId, opcaoVotoId, hashAnterior, numeroBloco);
            return txHash;
        }
        #endregion ELEIÇÂO


        #region SMART CONTRACT
        public async Task<TransactionReceipt> CriarContratoEleicaoAsync(Guid eleicaoId, string nomeEleicao)
        {
            // Verifique o saldo da conta
            //var balance = await GetAccountBalanceAsync(_accountAddress);
            //if (balance <= 0)
            //{
            //    throw new InvalidOperationException("Saldo insuficiente na conta para cobrir o custo da transação.");
            //}

            string bytecode = "6080604052348015600e575f5ffd5b506110568061001c5f395ff3fe608060405234801561000f575f5ffd5b5060043610610060575f3560e01c80632d50b1fa146100645780632deb124b146100805780633b2fda751461009e578063af640d0f146100ce578063c89a33b1146100ec578063e2bf53301461011f575b5f5ffd5b61007e6004803603810190610079919061081f565b61014f565b005b6100886102ff565b604051610095919061091b565b60405180910390f35b6100b860048036038101906100b3919061093b565b61038b565b6040516100c59190610adf565b60405180910390f35b6100d661052b565b6040516100e39190610b0e565b60405180910390f35b61010660048036038101906101019190610b5a565b610530565b6040516101169493929190610b98565b60405180910390f35b6101396004803603810190610134919061093b565b610682565b6040516101469190610c03565b60405180910390f35b60035f8581526020019081526020015f205f9054906101000a900460ff16156101ad576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016101a490610c66565b60405180910390fd5b5f8251116101f0576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016101e790610cce565b60405180910390fd5b5f815111610233576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161022a90610d36565b60405180910390fd5b5f604051806080016040528086815260200185815260200184815260200183815250905060025f8681526020019081526020015f2081908060018154018082558091505060019003905f5260205f2090600402015f909190919091505f820151815f01556020820151816001015560408201518160020190816102b69190610f51565b5060608201518160030190816102cc9190610f51565b505050600160035f8781526020019081526020015f205f6101000a81548160ff0219169083151502179055505050505050565b6001805461030c90610d81565b80601f016020809104026020016040519081016040528092919081815260200182805461033890610d81565b80156103835780601f1061035a57610100808354040283529160200191610383565b820191905f5260205f20905b81548152906001019060200180831161036657829003601f168201915b505050505081565b606060025f8381526020019081526020015f20805480602002602001604051908101604052809291908181526020015f905b82821015610520578382905f5260205f2090600402016040518060800160405290815f82015481526020016001820154815260200160028201805461040190610d81565b80601f016020809104026020016040519081016040528092919081815260200182805461042d90610d81565b80156104785780601f1061044f57610100808354040283529160200191610478565b820191905f5260205f20905b81548152906001019060200180831161045b57829003601f168201915b5050505050815260200160038201805461049190610d81565b80601f01602080910402602001604051908101604052809291908181526020018280546104bd90610d81565b80156105085780601f106104df57610100808354040283529160200191610508565b820191905f5260205f20905b8154815290600101906020018083116104eb57829003601f168201915b505050505081525050815260200190600101906103bd565b505050509050919050565b5f5481565b6002602052815f5260405f208181548110610549575f80fd5b905f5260205f2090600402015f9150915050805f01549080600101549080600201805461057590610d81565b80601f01602080910402602001604051908101604052809291908181526020018280546105a190610d81565b80156105ec5780601f106105c3576101008083540402835291602001916105ec565b820191905f5260205f20905b8154815290600101906020018083116105cf57829003601f168201915b50505050509080600301805461060190610d81565b80601f016020809104026020016040519081016040528092919081815260200182805461062d90610d81565b80156106785780601f1061064f57610100808354040283529160200191610678565b820191905f5260205f20905b81548152906001019060200180831161065b57829003601f168201915b5050505050905084565b6003602052805f5260405f205f915054906101000a900460ff1681565b5f604051905090565b5f5ffd5b5f5ffd5b5f819050919050565b6106c2816106b0565b81146106cc575f5ffd5b50565b5f813590506106dd816106b9565b92915050565b5f5ffd5b5f5ffd5b5f601f19601f8301169050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52604160045260245ffd5b610731826106eb565b810181811067ffffffffffffffff821117156107505761074f6106fb565b5b80604052505050565b5f61076261069f565b905061076e8282610728565b919050565b5f67ffffffffffffffff82111561078d5761078c6106fb565b5b610796826106eb565b9050602081019050919050565b828183375f83830152505050565b5f6107c36107be84610773565b610759565b9050828152602081018484840111156107df576107de6106e7565b5b6107ea8482856107a3565b509392505050565b5f82601f830112610806576108056106e3565b5b81356108168482602086016107b1565b91505092915050565b5f5f5f5f60808587031215610837576108366106a8565b5b5f610844878288016106cf565b9450506020610855878288016106cf565b935050604085013567ffffffffffffffff811115610876576108756106ac565b5b610882878288016107f2565b925050606085013567ffffffffffffffff8111156108a3576108a26106ac565b5b6108af878288016107f2565b91505092959194509250565b5f81519050919050565b5f82825260208201905092915050565b8281835e5f83830152505050565b5f6108ed826108bb565b6108f781856108c5565b93506109078185602086016108d5565b610910816106eb565b840191505092915050565b5f6020820190508181035f83015261093381846108e3565b905092915050565b5f602082840312156109505761094f6106a8565b5b5f61095d848285016106cf565b91505092915050565b5f81519050919050565b5f82825260208201905092915050565b5f819050602082019050919050565b610998816106b0565b82525050565b5f82825260208201905092915050565b5f6109b8826108bb565b6109c2818561099e565b93506109d28185602086016108d5565b6109db816106eb565b840191505092915050565b5f608083015f8301516109fb5f86018261098f565b506020830151610a0e602086018261098f565b5060408301518482036040860152610a2682826109ae565b91505060608301518482036060860152610a4082826109ae565b9150508091505092915050565b5f610a5883836109e6565b905092915050565b5f602082019050919050565b5f610a7682610966565b610a808185610970565b935083602082028501610a9285610980565b805f5b85811015610acd5784840389528151610aae8582610a4d565b9450610ab983610a60565b925060208a01995050600181019050610a95565b50829750879550505050505092915050565b5f6020820190508181035f830152610af78184610a6c565b905092915050565b610b08816106b0565b82525050565b5f602082019050610b215f830184610aff565b92915050565b5f819050919050565b610b3981610b27565b8114610b43575f5ffd5b50565b5f81359050610b5481610b30565b92915050565b5f5f60408385031215610b7057610b6f6106a8565b5b5f610b7d858286016106cf565b9250506020610b8e85828601610b46565b9150509250929050565b5f608082019050610bab5f830187610aff565b610bb86020830186610aff565b8181036040830152610bca81856108e3565b90508181036060830152610bde81846108e3565b905095945050505050565b5f8115159050919050565b610bfd81610be9565b82525050565b5f602082019050610c165f830184610bf4565b92915050565b7f456c6569746f72206a6120766f746f752e0000000000000000000000000000005f82015250565b5f610c506011836108c5565b9150610c5b82610c1c565b602082019050919050565b5f6020820190508181035f830152610c7d81610c44565b9050919050565b7f4861736820616e746572696f722065206f6272696761746f72696f2e000000005f82015250565b5f610cb8601c836108c5565b9150610cc382610c84565b602082019050919050565b5f6020820190508181035f830152610ce581610cac565b9050919050565b7f4e756d65726f20646f20626c6f636f2065206f6272696761746f72696f2e00005f82015250565b5f610d20601e836108c5565b9150610d2b82610cec565b602082019050919050565b5f6020820190508181035f830152610d4d81610d14565b9050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52602260045260245ffd5b5f6002820490506001821680610d9857607f821691505b602082108103610dab57610daa610d54565b5b50919050565b5f819050815f5260205f209050919050565b5f6020601f8301049050919050565b5f82821b905092915050565b5f60088302610e0d7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff82610dd2565b610e178683610dd2565b95508019841693508086168417925050509392505050565b5f819050919050565b5f610e52610e4d610e4884610b27565b610e2f565b610b27565b9050919050565b5f819050919050565b610e6b83610e38565b610e7f610e7782610e59565b848454610dde565b825550505050565b5f5f905090565b610e96610e87565b610ea1818484610e62565b505050565b5b81811015610ec457610eb95f82610e8e565b600181019050610ea7565b5050565b601f821115610f0957610eda81610db1565b610ee384610dc3565b81016020851015610ef2578190505b610f06610efe85610dc3565b830182610ea6565b50505b505050565b5f82821c905092915050565b5f610f295f1984600802610f0e565b1980831691505092915050565b5f610f418383610f1a565b9150826002028217905092915050565b610f5a826108bb565b67ffffffffffffffff811115610f7357610f726106fb565b5b610f7d8254610d81565b610f88828285610ec8565b5f60209050601f831160018114610fb9575f8415610fa7578287015190505b610fb18582610f36565b865550611018565b601f198416610fc786610db1565b5f5b82811015610fee57848901518255600182019150602085019450602081019050610fc9565b8683101561100b5784890151611007601f891682610f1a565b8355505b6001600288020188555050505b50505050505056fea2646970667358221220dfed3dd2c23a0d06b54a42b7b4c8d395acd8dc735d4afd0b118fa812b9c28ef864736f6c634300081b0033";
            
           
            var abi = @"[
                {
                    ""inputs"": [
                        {
                            ""internalType"": ""bytes32"",
                            ""name"": ""id"",
                            ""type"": ""bytes32""
                        },
                        {
                            ""internalType"": ""string"",
                            ""name"": ""nome"",
                            ""type"": ""string""
                        }
                    ],
                    ""stateMutability"": ""nonpayable"",
                    ""type"": ""constructor""
                },
                {
                    ""constant"": true,
                    ""inputs"": [],
                    ""name"": ""id"",
                    ""outputs"": [
                        {
                            ""name"": """",
                            ""type"": ""bytes32""
                        }
                    ],
                    ""payable"": false,
                    ""stateMutability"": ""view"",
                    ""type"": ""function""
                },
                {
                    ""constant"": true,
                    ""inputs"": [],
                    ""name"": ""nome"",
                    ""outputs"": [
                        {
                            ""name"": """",
                            ""type"": ""string""
                        }
                    ],
                    ""payable"": false,
                    ""stateMutability"": ""view"",
                    ""type"": ""function""
                },
                {
                    ""constant"": false,
                    ""inputs"": [
                        {
                            ""name"": ""eleitorId"",
                            ""type"": ""bytes32""
                        },
                        {
                            ""name"": ""opcaoVotoId"",
                            ""type"": ""bytes32""
                        },
                        {
                            ""name"": ""hashAnterior"",
                            ""type"": ""string""
                        },
                        {
                            ""name"": ""numeroBloco"",
                            ""type"": ""string""
                        }
                    ],
                    ""name"": ""votar"",
                    ""outputs"": [],
                    ""payable"": false,
                    ""stateMutability"": ""nonpayable"",
                    ""type"": ""function""
                },
                {
                    ""constant"": true,
                    ""inputs"": [
                        {
                            ""name"": ""eleitorId"",
                            ""type"": ""bytes32""
                        }
                    ],
                    ""name"": ""consultarVotos"",
                    ""outputs"": [
                        {
                            ""components"": [
                                {
                                    ""name"": ""eleitorId"",
                                    ""type"": ""bytes32""
                                },
                                {
                                    ""name"": ""opcaoVotoId"",
                                    ""type"": ""bytes32""
                                },
                                {
                                    ""name"": ""hashAnterior"",
                                    ""type"": ""string""
                                },
                                {
                                    ""name"": ""numeroBloco"",
                                    ""type"": ""string""
                                }
                            ],
                            ""name"": """",
                            ""type"": ""tuple[]""
                        }
                    ],
                    ""payable"": false,
                    ""stateMutability"": ""view"",
                    ""type"": ""function""
                }
            ]";
            var gas = new HexBigInteger(120000); // Gas limit
            var value = new HexBigInteger(0); // Value
            var cancellationToken = new CancellationTokenSource().Token;

            var deploymentHandler = _web3.Eth.GetContractDeploymentHandler<EleicaoDeploymentDTO>();

            // Converta o Guid para uma string hexadecimal de 32 bytes
            var idHex = eleicaoId.ToString("N").PadLeft(32, '0');
            var deployment = new EleicaoDeploymentDTO
            {
                Id = idHex,
                Nome = nomeEleicao,
                ByteCode = bytecode
            };


            // Crie um CancellationTokenSource
            var cancellationTokenSource = new CancellationTokenSource();

            var transactionReceipts = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deployment, cancellationTokenSource.Token);
            return transactionReceipts;
        }

        public async Task TestInfuraConnectionAsync()
        {
            var response = await client.PostAsync(_infuraUrl, new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"web3_clientVersion\",\"params\":[],\"id\":1}"));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Falha ao conectar ao Infura: {response.StatusCode} - {response.ReasonPhrase}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Conexão com Infura bem-sucedida: {responseString}");
        }
        public async Task<decimal> GetAccountBalanceAsync(string accountAddress)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(accountAddress);
            return Web3.Convert.FromWei(balance.Value);
        }

        #endregion SMART CONTRACT
    }
}
