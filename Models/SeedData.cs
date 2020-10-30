using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
            {
                if (!context.Movies.Any())
                {
                    //if contains any data --- then we return --- we do not seed
                    context.Movies.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "PG",

                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "GP"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "GP"
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M,
                        Rating = "R"
                    },
                    new Movie
                    {
                        Title = "Inglorious Basterds",
                        ReleaseDate = DateTime.Parse("2009-08-21"),
                        Genre = "War",
                        Price = 5.00M,
                        Rating = "R",
                    },
                    new Movie
                    {
                        Title = "Mama",
                        ReleaseDate = DateTime.Parse("2013-01-18"),
                        Genre = "Horror",
                        Price = 4.99M,
                        Rating = "PG"
                    },
                    new Movie
                    {
                        Title = "Romeo + Juliet",
                        ReleaseDate = DateTime.Parse("1996-11-01"),
                        Genre = "Romance",
                        Price = 6.29M,
                        Rating = "PG"
                    },
                    new Movie
                    {
                        Title = "Jigsaw",
                        ReleaseDate = DateTime.Parse("2017-10-27"),
                        Genre = "Mystery",
                        Price = 9.50M,
                        Rating = "R"
                    }
                    );
                }

                if (!context.Directors.Any())
                {
                    context.Directors.AddRange(
                        new Director
                        {
                            Name = "Larz Thimoty Pal-ing"
                        },
                        new Director
                        {
                            Name = "Michelle Espeleta"
                        },
                        new Director
                        {
                            Name = "Kyla Samson"
                        },
                        new Director
                        {
                            Name = "Vince \"The Terrible\" Repia"
                        }
                        );
                }
                    

                    
                    context.SaveChanges();
                
            }
        }
    }
}
