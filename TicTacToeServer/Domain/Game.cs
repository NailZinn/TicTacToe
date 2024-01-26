﻿namespace Domain;

public class Game
{
    public int Id { get; set; }
    
    public User Player1 { get; set; }
    public User? Player2 { get; set; }
    public List<User> Others { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public GameStatus Status { get; set; }
    public long MaxRating { get; set; }
}