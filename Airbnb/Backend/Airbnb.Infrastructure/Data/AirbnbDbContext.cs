using Airbnb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Infrastructure.Data;

public class AirbnbDbContext(DbContextOptions<AirbnbDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<ListingImage> ListingImages => Set<ListingImage>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<Listing>()
            .HasOne(x => x.Host)
            .WithMany(x => x.Listings)
            .HasForeignKey(x => x.HostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Guest)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Listing)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Favorite>()
            .HasIndex(x => new { x.UserId, x.ListingId })
            .IsUnique();

        modelBuilder.Entity<Favorite>()
            .HasOne(x => x.User)
            .WithMany(x => x.Favorites)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(x => x.Listing)
            .WithMany(x => x.Favorites)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(x => x.Listing)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ListingImage>()
            .HasOne(x => x.Listing)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Listing>().Property(x => x.PricePerNight).HasPrecision(18, 2);
        modelBuilder.Entity<Booking>().Property(x => x.TotalPrice).HasPrecision(18, 2);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, FirstName = "Demo", LastName = "Host", Email = "host@airbnb.local",
                PasswordHash = passwordHash, Role = Airbnb.Domain.Enums.UserRole.Host,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new User
            {
                Id = 2, FirstName = "Demo", LastName = "Guest", Email = "guest@airbnb.local",
                PasswordHash = passwordHash, Role = Airbnb.Domain.Enums.UserRole.Guest,
                CreatedAt = new DateTime(2026, 1, 1)
            });

        modelBuilder.Entity<Listing>().HasData(
            new Listing
            {
                Id = 1, HostId = 1, Title = "Modern Alpine Apartment",
                Description = "A bright and comfortable apartment with mountain views.",
                PropertyType = Airbnb.Domain.Enums.PropertyType.Apartment,
                City = "Munich", Country = "Germany", Address = "Demo Street 1",
                PricePerNight = 145, MaxGuests = 4, Bedrooms = 2, Beds = 2, Bathrooms = 1,
                IsActive = true, CreatedAt = new DateTime(2026, 1, 1)
            },
            new Listing
            {
                Id = 2, HostId = 1, Title = "Cozy City House",
                Description = "A stylish house close to restaurants and public transport.",
                PropertyType = Airbnb.Domain.Enums.PropertyType.House,
                City = "Berlin", Country = "Germany", Address = "Demo Avenue 2",
                PricePerNight = 180, MaxGuests = 5, Bedrooms = 3, Beds = 4, Bathrooms = 2,
                IsActive = true, CreatedAt = new DateTime(2026, 1, 2)
            });

        modelBuilder.Entity<ListingImage>().HasData(
            new ListingImage { Id = 1, ListingId = 1, ImageUrl = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?auto=format&fit=crop&w=1200&q=80", IsPrimary = true },
            new ListingImage { Id = 2, ListingId = 2, ImageUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?auto=format&fit=crop&w=1200&q=80", IsPrimary = true });
    }
}
