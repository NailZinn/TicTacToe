namespace TicTacToeServer.Dto;

public record StartGameMessage(char PlayerSymbol, bool PlayerTurn, List<string> GameMessages);