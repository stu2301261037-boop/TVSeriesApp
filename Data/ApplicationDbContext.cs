using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TVSeriesApp.Models;

namespace TVSeriesApp.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Series> Series { get; set; }
		public DbSet<Actor> Actors { get; set; }
		public DbSet<Episode> Episodes { get; set; }
		public DbSet<SeriesActor> SeriesActors { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Конфигуриране на many-to-many връзка
			builder.Entity<SeriesActor>()
				.HasKey(sa => new { sa.SeriesId, sa.ActorId });

			builder.Entity<SeriesActor>()
				.HasOne(sa => sa.Series)
				.WithMany()
				.HasForeignKey(sa => sa.SeriesId);

			builder.Entity<SeriesActor>()
				.HasOne(sa => sa.Actor)
				.WithMany(a => a.SeriesActors)
				.HasForeignKey(sa => sa.ActorId);
		}
	}
}