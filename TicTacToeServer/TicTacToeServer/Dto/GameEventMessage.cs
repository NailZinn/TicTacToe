namespace TicTacToeServer.Dto;

public record GameEventMessage(string GameId, int Square, char Symbol);