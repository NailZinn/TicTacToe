using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
    public Game? AsOwner { get; set; }
    
    public int? AsPlayerId { get; set; }
    public Game? AsPlayer { get; set; }
    
    public int? AsWatcherId { get; set; }
    public Game? AsWatcher { get; set; }

    public bool HasActiveGame => AsOwner is not null || AsPlayer is not null || AsWatcher is not null;
}
