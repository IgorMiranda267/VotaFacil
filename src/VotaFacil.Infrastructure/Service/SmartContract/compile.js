const path = require('path');
const fs = require('fs');
const solc = require('solc');

// Caminho para o contrato
const contractPath = path.resolve(__dirname, 'Eleicao.sol');
const source = fs.readFileSync(contractPath, 'utf8');

// Configuração do input para o compilador Solidity
const input = {
    language: 'Solidity',
    sources: {
        'Eleicao.sol': {
            content: source,
        },
    },
    settings: {
        outputSelection: {
            '*': {
                '*': ['abi', 'evm.bytecode'],
            },
        },
    },
};

// Compilação do contrato
const output = JSON.parse(solc.compile(JSON.stringify(input)));

// Verificação de erros de compilação
if (output.errors) {
    output.errors.forEach(err => {
        console.error(err.formattedMessage);
    });
}

// Acessando o bytecode do contrato compilado
const bytecode = output.contracts['Eleicao.sol'].Eleicao.evm.bytecode.object;
console.log('Bytecode:', bytecode);

// Acessando a ABI do contrato compilado
const abi = output.contracts['Eleicao.sol'].Eleicao.abi;
console.log('ABI:', JSON.stringify(abi, null, 2));