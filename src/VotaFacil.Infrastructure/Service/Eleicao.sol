// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract Eleicao {
    bytes32 public id;
    string public nome;

    struct Voto {
        bytes32 eleitorId;
        bytes32 opcaoVotoId;
        string hashAnterior;
        string numeroBloco;
    }

    mapping(bytes32 => Voto[]) public votos;
    mapping(bytes32 => bool) public jaVotou;

    function votar(bytes32 eleitorId, bytes32 opcaoVotoId, string memory hashAnterior, string memory numeroBloco) public {
        require(!jaVotou[eleitorId], "Eleitor já votou.");
        require(bytes(hashAnterior).length > 0, "Hash anterior é obrigatório.");
        require(bytes(numeroBloco).length > 0, "Número do bloco é obrigatório.");
        
        Voto memory novoVoto = Voto(eleitorId, opcaoVotoId, hashAnterior, numeroBloco);
        votos[eleitorId].push(novoVoto);
        jaVotou[eleitorId] = true;
    }

    function consultarVotos(bytes32 eleitorId) public view returns (Voto[] memory) {
        return votos[eleitorId];
    }
}