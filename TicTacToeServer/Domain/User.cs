using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
    public Game? AsOwner { get; set; }
    public Game? AsPlayer { get; set; }
    public Game? AsWatcher { get; set; }

    public Game? ActiveGame => AsOwner ?? AsPlayer;
    public bool HasJoinedGame => (ActiveGame ?? AsWatcher) is not null;
}
