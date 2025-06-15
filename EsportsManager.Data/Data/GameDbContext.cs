using EsportsManager.EsportsManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class GameDbContext : DbContext
{
    public DbSet<GameState> GameStates { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<GameProfile> GameProfiles { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<PlayerStats> PlayerStats { get; set; }
    public DbSet<Trait_Player> Traits_Players { get; set; }
    public DbSet<PlayerTrait> PlayerTraits { get; set; }
    public DbSet<Trait_Team> Traits_Teams { get; set; }
    public DbSet<TeamTrait> TeamTraits { get; set; }
    public DbSet<Trait_Organization> Traits_Organization { get; set; }
    public DbSet<OrganizationTrait> OrganizationTraits { get; set; }
    public DbSet<VRSRanking> VRSRankings { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Sponsorship> Sponsors { get; set; }
    public DbSet<Event> Events { get; set; }


    private readonly string _dbPath;

    public GameDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //GameState integration
        modelBuilder.Entity<GameState>()
            .HasKey(gs => gs.Id);
        //modelBuilder.Entity<GameState>()
        //    .HasOne(gs => gs.UserTeam)
        //    .WithMany(c=>c.GameStateReferences)
        //    .HasForeignKey(gs => gs.UserTeamId)
        //    .IsRequired(false)
        //    .OnDelete(DeleteBehavior.Cascade);
        //modelBuilder.Entity<GameState>()
        //    .HasOne(gs => gs.UserOrg)
        //    .WithMany(c=>c.GameStateReferences)
        //    .HasForeignKey(gs => gs.UserOrgId)
        //    .IsRequired(false)
        //    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GameState>(entity =>
        {
            // Explicit column configuration
            entity.Property(e => e.UserTeamId)
                .HasColumnType("TEXT")  // SQLite text type for GUIDs
                .IsRequired(false); // Makes it nullable

            entity.Property(e => e.UserOrgId)
                .HasColumnType("TEXT")
                .IsRequired(false);
        });

        modelBuilder.Entity<Event>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Events)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Organization>()
            .HasOne(c=>c.GameState)
            .WithMany(gs => gs.Organizations)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Game>()
            .HasOne(c=>c.GameState)
            .WithMany(gs => gs.AllGames)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Team>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Teams)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Player>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Players)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Coach>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Coaches)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Manager>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Managers)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Map>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Maps)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Sponsorship>()
            .HasOne(c => c.GameState)
            .WithMany(gs => gs.Sponsors)
            .HasForeignKey(c => c.GameStateId)
            .OnDelete(DeleteBehavior.Cascade);


        // TPH for Matches
        modelBuilder.Entity<Match>()
            .HasDiscriminator<string>("MatchType")
            .HasValue<Match_CS>("CS")
            .HasValue<Match_LoL>("LoL");

        // Series - Match (1-to-many)
        modelBuilder.Entity<Series>()
            .HasMany(s => s.Matches)
            .WithOne(m => m.Series)
            .HasForeignKey(m => m.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        // Match - WinnerTeam (many-to-one)
        modelBuilder.Entity<Match>()
            .HasOne(m => m.WinnerTeam)
            .WithMany()
            .HasForeignKey(m => m.WinnerTeamId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // CS Match - CSMap (1-to-many)
        modelBuilder.Entity<Match_CS>()
            .HasOne(m => m.Map)
            .WithMany(cs => cs.Matches)
            .HasForeignKey(m => m.MapId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PlayerStats_CS>()
            .HasOne(m=>m.Match)
            .WithMany(p => p.PlayerStats)
            .HasForeignKey(m => m.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PlayerStats_CS>()
            .HasOne(m => m.Player)
            .WithMany(p => p.PlayerStats_CS)
            .HasForeignKey(m => m.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);



        // 🧠 Configure TPH inheritance for GameProfile
        modelBuilder.Entity<GameProfile>()
            .HasDiscriminator<string>("ProfileType")
            .HasValue<GameProfile_CS>("CS")
            .HasValue<GameProfile_LoL>("LoL")
            .HasValue<GameProfile_SC>("SC");

        // 🧠 Team - Game (many-to-one)
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Game)
            .WithMany()
            .HasForeignKey(t => t.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🧠 Player - GameProfiles (one-to-many)
        modelBuilder.Entity<Player>()
            .HasMany(p => p.GameProfiles)
            .WithOne(gp => gp.Player)
            .HasForeignKey(gp => gp.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🧠 Team - Player (optional many-to-one)
        modelBuilder.Entity<Player>()
            .HasOne(p=> p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(c=>c.TeamId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // 🧠 Team - Coach (optional many-to-one)
        modelBuilder.Entity<Coach>()
            .HasOne(c => c.Team)
            .WithMany(t => t.Coaches)
            .HasForeignKey(c => c.TeamId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // Ensure all strings like names are required and limited in size (optional but good practice)
        modelBuilder.Entity<Team>()
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Game>()
            .Property(g => g.FullName)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Game>()
            .Property(g => g.ShortName)
            .IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<Game>()
            .HasOne(c => c.Developer)
            .WithMany()
            .HasForeignKey(c => c.DeveloperId)
            .IsRequired(true);

        modelBuilder.Entity<Organization>()
            .HasMany(o => o.Teams)
            .WithOne(t => t.Organization)
            .HasForeignKey(t => t.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Organization)
            .WithMany(o => o.Teams)
            .HasForeignKey(t => t.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        modelBuilder.Entity<Organization>()
            .HasMany(o=>o.Managers)
            .WithOne(m => m.Organization)
            .HasForeignKey(m => m.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder.Entity<Tournament>()
            .HasMany(t => t.Series)
            .WithOne(t => t.Tournament)
            .HasForeignKey(t => t.TournamentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        modelBuilder.Entity<Tournament>()
            .HasOne(c => c.Organization)
            .WithMany(o => o.Tournaments)
            .HasForeignKey(c => c.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Tournament>()
            .HasOne(c => c.Winner)
            .WithMany(c => c.TournamentsWon)
            .HasForeignKey(c => c.WinnerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Tournament>()
            .HasMany(c => c.ParticipatingTeams)
            .WithMany(c => c.TournamentsParticipated);
        modelBuilder.Entity<Tournament>()
            .HasOne(c => c.ParentTournament)
            .WithMany(g => g.QualifierTournaments)
            .HasForeignKey(c => c.ParentTournamentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        //Traits
        //PlayerTraits
        modelBuilder.Entity<PlayerTrait>()
            .HasKey(pt => new { pt.PlayerId, pt.TraitId });

        // Configure the many-to-many relationship
        modelBuilder.Entity<PlayerTrait>()
            .HasOne(pt => pt.Player)
            .WithMany(p => p.PlayerTraits)
            .HasForeignKey(pt => pt.PlayerId);

        modelBuilder.Entity<PlayerTrait>()
            .HasOne(pt => pt.Trait)
            .WithMany(t => t.PlayerTraits)
            .HasForeignKey(pt => pt.TraitId);
        //TeamTraits
        modelBuilder.Entity<TeamTrait>()
            .HasKey(pt => new { pt.TeamId, pt.TraitId });

        // Configure the many-to-many relationship
        modelBuilder.Entity<TeamTrait>()
            .HasOne(pt => pt.Team)
            .WithMany(p => p.TeamTraits)
            .HasForeignKey(pt => pt.TeamId);

        modelBuilder.Entity<TeamTrait>()
            .HasOne(pt => pt.Trait)
            .WithMany(t => t.TeamTraits)
            .HasForeignKey(pt => pt.TraitId);
        //OrganizationTraits
        modelBuilder.Entity<OrganizationTrait>()
            .HasKey(pt => new { pt.OrganizationId, pt.TraitId });

        // Configure the many-to-many relationship
        modelBuilder.Entity<OrganizationTrait>()
            .HasOne(pt => pt.Organization)
            .WithMany(p => p.OrganizationTraits)
            .HasForeignKey(pt => pt.OrganizationId);

        modelBuilder.Entity<OrganizationTrait>()
            .HasOne(pt => pt.Trait)
            .WithMany(t => t.OrganizationTraits)
            .HasForeignKey(pt => pt.TraitId);
        //ManagerTraits
        modelBuilder.Entity<ManagerTrait>()
            .HasKey(pt => new { pt.ManagerId, pt.TraitId });

        // Configure the many-to-many relationship
        modelBuilder.Entity<ManagerTrait>()
            .HasOne(pt => pt.Manager)
            .WithMany(p => p.ManagerTraits)
            .HasForeignKey(pt => pt.ManagerId);

        modelBuilder.Entity<ManagerTrait>()
            .HasOne(pt => pt.Trait)
            .WithMany(t => t.ManagerTraits)
            .HasForeignKey(pt => pt.TraitId);

        modelBuilder.Entity<VRSRanking>()
            .HasOne(v => v.Team)
            .WithOne() // Optional: .WithOne(t => t.VRSRanking) if you want back-reference
            .HasForeignKey<VRSRanking>(v => v.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        //Contracts
        modelBuilder.Entity<Contract>()
            .HasDiscriminator<string>("ContractType")
            .HasValue<ContractPlayer>("ContractPlayer")
            .HasValue<ContractSponsor>("ContractSponsor");

        modelBuilder.Entity<ContractPlayer>()
            .HasOne(c => c.Player)
            .WithMany(c=>c.ContractPlayers)
            .HasForeignKey(c => c.PlayerId);
        modelBuilder.Entity<ContractSponsor>()
            .HasOne(c => c.Team)
            .WithMany(c => c.Sponsors)
            .HasForeignKey(c => c.TeamId)
            .IsRequired(false);
        modelBuilder.Entity<ContractSponsor>()
            .HasOne(c => c.Organization)
            .WithMany(c => c.Sponsors)
            .HasForeignKey(c => c.OrganizationId)
            .IsRequired(false);
    }
}