using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TVSeriesApp.Models;
using TVSeriesApp.Data;

namespace TVSeriesApp.Data
{
	public static class SeedData
	{
		public static async Task Initialize(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			// Миграции
			await context.Database.MigrateAsync();

			// Създаване на роли
			string[] roleNames = { "Admin", "User", "Guest" };
			foreach (var roleName in roleNames)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			// Създаване на администратор
			var adminUser = new IdentityUser
			{
				UserName = "admin@tvseries.com",
				Email = "admin@tvseries.com",
				EmailConfirmed = true
			};

			string adminPassword = "Admin123!";
			var user = await userManager.FindByEmailAsync(adminUser.Email);

			if (user == null)
			{
				var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
         

            // Създаване на тестови потребители
            var testUser = new IdentityUser
			{
				UserName = "user@tvseries.com",
				Email = "user@tvseries.com",
				EmailConfirmed = true
			};

			var existingUser = await userManager.FindByEmailAsync(testUser.Email);
			if (existingUser == null)
			{
				var createUser = await userManager.CreateAsync(testUser, "User123!");
				if (createUser.Succeeded)
				{
					await userManager.AddToRoleAsync(testUser, "User");
				}
			}

            // Добавяне на примерни сериали
            if (!context.Series.Any())
			{
				var series = new List<Series>
				{
					new Series
					{
						Title = "Game of Thrones",
						Description = "Nine noble families fight for control over the lands of Westeros.",
						ReleaseDate = new DateTime(2011, 4, 17),
						Seasons = 8,
						Rating = 9.3m,
						Genre = "Fantasy",
						IsCompleted = true,
                        PosterUrl = "https://m.media-amazon.com/images/M/MV5BYTRiNDQwYzAtMzVlZS00NTI5LWJjYjUtMzkwNTUzMWMxZTllXkEyXkFqcGdeQXVyNDIzMzcwNjc@._V1_FMjpg_UX1000_.jpg"
                    },
					new Series
					{
						Title = "Breaking Bad",
						Description = "A high school chemistry teacher diagnosed with cancer turns to a life of crime.",
						ReleaseDate = new DateTime(2008, 1, 20),
						Seasons = 5,
						Rating = 9.5m,
						Genre = "Crime",
						IsCompleted = true,
                        PosterUrl = "https://mediaproxy.tvtropes.org/width/1200/https://static.tvtropes.org/pmwiki/pub/images/breaking_bad_4.png"
                    },
					new Series
					{
						Title = "Stranger Things",
						Description = "When a young boy vanishes, a small town uncovers a mystery involving secret experiments.",
						ReleaseDate = new DateTime(2016, 7, 15),
						Seasons = 4,
						Rating = 8.7m,
						Genre = "Sci-Fi",
						IsCompleted = false,
                        PosterUrl = "https://i.redd.it/x312wpf8c1zf1.png"
                    },
					new Series
					{
						Title = "Supernatural",
						Description = "Two brothers follow their father's footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons, and gods that roam the earth.",
						ReleaseDate = new DateTime(2016, 11, 4),
						Seasons = 15,
						Rating = 8.4m,
						Genre = "Action",
						IsCompleted = true,
						PosterUrl = "https://i0.wp.com/abacstallion.com/wp-content/uploads/2019/04/Supernatural-Poster.jpg?resize=1068%2C1602&ssl=1"
                    },
					new Series
					{
						Title = "The Mandalorian",
						Description = "The travels of a lone bounty hunter in the outer reaches of the galaxy.",
						ReleaseDate = new DateTime(2019, 11, 12),
						Seasons = 3,
						Rating = 8.8m,
						Genre = "Sci-Fi",
						IsCompleted = false, 
						PosterUrl = "https://m.media-amazon.com/images/I/71YDdgOqzkL._AC_SL1024_.jpg"
                    },
					new Series
					{
						Title = "La Casa De Papel",
						Description = "An unusual group of robbers attempt to carry out the most perfect robbery in Spanish history - stealing 2.4 billion euros from the Royal Mint of Spain.",
						ReleaseDate = new DateTime(2019, 12, 20),
						Seasons = 3,
						Rating = 8.2m,
						Genre = "Crime drama",
						IsCompleted = false,
                        PosterUrl = "https://no1name.blog.bg/photos/107432/original/money%20heist%201.jpg"
                    }
				};

				await context.Series.AddRangeAsync(series);
				await context.SaveChangesAsync();
			}

			// Добавяне на примерни актьори
			if (!context.Actors.Any())
			{
				var actors = new List<Actor>
				{
					new Actor
					{
						FirstName = "Emilia",
						LastName = "Clarke",
						BirthDate = new DateTime(1986, 10, 23),
						Nationality = "British",
						Salary = 500000m,
						IsAwardWinner = true
					},
					new Actor
					{
						FirstName = "Kit",
						LastName = "Harington",
						BirthDate = new DateTime(1986, 12, 26),
						Nationality = "British",
						Salary = 450000m,
						IsAwardWinner = true
					},
					new Actor
					{
						FirstName = "Bryan",
						LastName = "Cranston",
						BirthDate = new DateTime(1956, 3, 7),
						Nationality = "American",
						Salary = 600000m,
						IsAwardWinner = true
					},
					new Actor
					{
						FirstName = "Aaron",
						LastName = "Paul",
						BirthDate = new DateTime(1979, 8, 27),
						Nationality = "American",
						Salary = 400000m,
						IsAwardWinner = true
					},
					new Actor
					{
						FirstName = "Millie Bobby",
						LastName = "Brown",
						BirthDate = new DateTime(2004, 2, 19),
						Nationality = "British",
						Salary = 350000m,
						IsAwardWinner = false
					},
					new Actor
					{
						FirstName = "Claire",
						LastName = "Foy",
						BirthDate = new DateTime(1984, 4, 16),
						Nationality = "British",
						Salary = 300000m,
						IsAwardWinner = true
					}
				};
                // Добавяне на връзки между актьори и сериали
                if (!context.SeriesActors.Any())
                {
                    var got = await context.Series.FirstAsync(s => s.Title == "Game of Thrones");
                    var bb = await context.Series.FirstAsync(s => s.Title == "Breaking Bad");
                    var st = await context.Series.FirstAsync(s => s.Title == "Stranger Things");

                    var emilia = await context.Actors.FirstAsync(a => a.FirstName == "Emilia" && a.LastName == "Clarke");
                    var kit = await context.Actors.FirstAsync(a => a.FirstName == "Kit" && a.LastName == "Harington");
                    var bryan = await context.Actors.FirstAsync(a => a.FirstName == "Bryan" && a.LastName == "Cranston");
                    var aaron = await context.Actors.FirstAsync(a => a.FirstName == "Aaron" && a.LastName == "Paul");
                    var millie = await context.Actors.FirstAsync(a => a.FirstName == "Millie Bobby" && a.LastName == "Brown");

                    var seriesActors = new List<SeriesActor>
    {
        new SeriesActor { SeriesId = got.Id, ActorId = emilia.Id, CharacterName = "Daenerys Targaryen" },
        new SeriesActor { SeriesId = got.Id, ActorId = kit.Id, CharacterName = "Jon Snow" },
        new SeriesActor { SeriesId = bb.Id, ActorId = bryan.Id, CharacterName = "Walter White" },
        new SeriesActor { SeriesId = bb.Id, ActorId = aaron.Id, CharacterName = "Jesse Pinkman" },
        new SeriesActor { SeriesId = st.Id, ActorId = millie.Id, CharacterName = "Eleven" }
    };

                    await context.SeriesActors.AddRangeAsync(seriesActors);
                    await context.SaveChangesAsync();
                }
            }

			// Добавяне на примерни епизоди
			if (!context.Episodes.Any())
			{
				var got = await context.Series.FirstAsync(s => s.Title == "Game of Thrones");
				var bb = await context.Series.FirstAsync(s => s.Title == "Breaking Bad");

				var episodes = new List<Episode>
				{
					new Episode
					{
						Title = "Winter Is Coming",
						SeasonNumber = 1,
						EpisodeNumber = 1,
						AirDate = new DateTime(2011, 4, 17),
						Duration = 62,
						Rating = 8.9m,
						SeriesId = got.Id
					},
					new Episode
					{
						Title = "The Kingsroad",
						SeasonNumber = 1,
						EpisodeNumber = 2,
						AirDate = new DateTime(2011, 4, 24),
						Duration = 56,
						Rating = 8.8m,
						SeriesId = got.Id
					},
					new Episode
					{
						Title = "Pilot",
						SeasonNumber = 1,
						EpisodeNumber = 1,
						AirDate = new DateTime(2008, 1, 20),
						Duration = 58,
						Rating = 8.9m,
						SeriesId = bb.Id
					},
					new Episode
					{
						Title = "Cat's in the Bag...",
						SeasonNumber = 1,
						EpisodeNumber = 2,
						AirDate = new DateTime(2008, 1, 27),
						Duration = 48,
						Rating = 8.7m,
						SeriesId = bb.Id
					}
				};

				await context.Episodes.AddRangeAsync(episodes);
				await context.SaveChangesAsync();
			}
		}
	}
}