namespace SocialNetwork
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Models;
    using Utils;

    public class Startup
    {
        private const int TotalUsers = 50;
        private const int TotalPictures = 100;
        private const int TotalAlbumsPerUser = 3;
        private const int TotalTags = 10;
        private const int MinFriends = 5;
        private const int MaxFriends = 10;
        private const int MaxPicturesInAlbum = 5;
        private const int MaxTagInAlbum = 5;

        private static Random random = new Random();

        public static void Main()
        {
            using (var context = new SocialNetworkDbContext())
            {
                // Initialize database 
                InitializeDatabase(context);

                // Seed data
                //InitialSeed(context);           // Modified for Task 3
                //SeedAlbums(context);            // Task 3
                //SeedPictures(context);          // Task 3
                //SeedPicturesToAlbums(context);  // Taks 3
                //SeedTags(context);              // Task 4
                //SeedTagsToAlbums(context);      // Task 4

                // Queries
                //PrintUsersWithFriendsAndStatuses(context);      // Task 2
                //PrintActiveUsersWithoreThan5Friends(context);   // Task 2
                //PrintAlbumsWithOwnersAndPictures(context);      // Task 3
                //PrintPicturesInMoreThan2Albums(context);        // Task 3
                //PrintUserAlbum(context);                        // Task 3
                //PrintAlbumsByTag(context);                      // Task 4
                //PrintUsersWithAlbumsWithMoreThan3Tags(context); // Task 4




            }
        }

        private static void PrintUsersWithAlbumsWithMoreThan3Tags(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums with more than 3 tags:");

            var albumData = context
                .Albums
                .Where(a => a.Tags.Count > 3)
                .OrderByDescending(a => a.User.Albums.Count)
                .ThenByDescending(a => a.Tags.Count)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    Owner = a.User.Username,
                    Title = a.Name,
                    Tags = a.Tags.Select(at => at.Tag.Name)
                })
                .ToList();

            var builder = new StringBuilder();

            foreach (var album in albumData)
            {
                builder
                    .AppendLine($"{album.Title} - {album.Owner}:")
                    .AppendLine($"  {string.Join(", ", album.Tags)}");
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintAlbumsByTag(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums by tag:");

            var tagId = context
                        .Tags
                        .Where(t => t.Albums.Any())
                        .Select(t => t.Id)
                        .FirstOrDefault();

            var albumsData = context
                .Albums
                .OrderByDescending(a => a.Tags.Count)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Name,
                    Owner = a.User.Username
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var album in albumsData)
            {
                builder.AppendLine($"{album.Name} - {album.Owner}");
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintUserAlbum(SocialNetworkDbContext context)
        {
            Console.WriteLine("User albums with pictures (title & path):");

            var userId = context
                        .Users
                        .Where(u => u.Albums.Any())
                        //.Where(u => u.Albums.Any(a => a.IsPublic))
                        //.Where(u => u.Albums.Any(a => !a.IsPublic))
                        .Select(u => u.Id)
                        .FirstOrDefault();

            var albumData = context
                .Albums
                .Where(a => a.User.Id == userId)
                .Select(a => new
                {
                    Owner = a.User.Username,
                    a.IsPublic,
                    a.Name,
                    Pictures = a.Pictures.Select(pa => new
                    {
                        pa.Picture.Title,
                        pa.Picture.Path
                    })
                })
                .OrderBy(a => a.Name)
                .ToList();

            var builder = new StringBuilder();
            foreach (var album in albumData)
            {
                builder.AppendLine($"{album.Name} - {album.Owner}:");

                if (album.IsPublic)
                {
                    if (album.Pictures.Any())
                    {
                        foreach (var picture in album.Pictures)
                        {
                            builder.AppendLine($"  {picture.Title} - {picture.Path}");
                        }
                    }
                    else
                    {
                        builder.AppendLine("  No pictures in album.");
                    }
                }
                else
                {
                    builder.AppendLine("  Private content!");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintPicturesInMoreThan2Albums(SocialNetworkDbContext context)
        {
            Console.WriteLine("Pictures appearing in more than 2 albums with album title & owner:");

            var picturesData = context
                .Pictures
                .Where(p => p.Albums.Select(pa => pa.Album).Count() > 2)
                .OrderByDescending(p => p.Albums.Count)
                .ThenBy(p => p.Title)
                .Select(p => new
                {
                    p.Title,
                    Albums = p.Albums.Select(pa => new
                    {
                        Title = pa.Album.Name,
                        Owner = pa.Album.User.Username
                    })
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var picture in picturesData)
            {
                builder.AppendLine($"{picture.Title}");
                foreach (var album in picture.Albums)
                {
                    builder.AppendLine($"  {album.Title} - {album.Owner}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintAlbumsWithOwnersAndPictures(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums with owners and pictures count:");

            var albumsData = context
                .Albums
                .Select(a => new
                {
                    a.Name,
                    Owner = a.User.Username,
                    Pictures = a.Pictures.Select(pa => pa.Picture).Count()
                })
                .OrderByDescending(a => a.Pictures)
                .ThenBy(a => a.Owner)
                .ToList();

            var builder = new StringBuilder();
            foreach (var album in albumsData)
            {
                builder.AppendLine($"{album.Name} - Owner {album.Owner} - Pictures {album.Pictures}");
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintActiveUsersWithoreThan5Friends(SocialNetworkDbContext context)
        {
            Console.WriteLine("Active users with more than 5 friends:");

            var usersData = context
                .Users
                .Where(u => u.IsDeleted == false)
                .Where(u => u.FriendshipsMade.Count > 5)
                .OrderBy(u => u.RegisteredOn)
                .ThenByDescending(u => u.FriendshipsMade.Count)
                .Select(u => new
                {
                    u.Username,
                    Friends = u.FriendshipsMade.Count,
                    Activity = DateTime.Now.Subtract(u.RegisteredOn)
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var user in usersData)
            {
                builder
                    .AppendLine($"{user.Username} - Friends {user.Friends} - Network activity {user.Activity.Days} days");
            }

            Console.WriteLine(builder.ToString());
        }

        private static void PrintUsersWithFriendsAndStatuses(SocialNetworkDbContext context)
        {
            Console.WriteLine("Users with number of friends and status of each friend:");

            var usersData = context
                .Users
                .Select(u => new
                {
                    u.Username,
                    Friends = u.FriendshipsMade.Count,
                    FriendsStatus = u.FriendshipsMade.Select(f => f.Friend.IsDeleted)
                })
                .OrderByDescending(u => u.Friends)
                .ThenBy(u => u.Username)
                .ToList();

            var builder = new StringBuilder();
            foreach (var user in usersData)
            {
                builder.AppendLine($"{user.Username} - Friends {user.Friends}");
                foreach (var friendStatus in user.FriendsStatus)
                {
                    if (friendStatus)
                    {
                        builder.AppendLine("  Active");
                    }
                    else
                    {
                        builder.AppendLine("  Inactive");
                    }
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private static void SeedTagsToAlbums(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Tags to Albums...");

            var albums = context.Albums.ToList();
            var tagIds = context.Tags.Select(t => t.Id).ToList();

            for (int i = 0; i < albums.Count; i++)
            {
                var currentAlbum = albums[i];
                var tagsCount = random.Next(0, MaxTagInAlbum);

                for (int j = 0; j < tagsCount; j++)
                {
                    try
                    {
                        currentAlbum.Tags.Add(new AlbumTag
                        {
                            TagId = tagIds[random.Next(0, tagIds.Count)]
                        });

                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private static void SeedTags(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Tags...");

            for (int i = 0; i < TotalTags; i++)
            {
                context.Tags.Add(new Tag
                {
                    Name = TagTransformer.Transform($"Tag  {i}")
                });
            }

            // Testing invalid tags
            //var invalidTags = new[] { "myCat", "#no wake up", "aaaaaaaaaaaaaaaaaaXCutThisEnd", "me and my bff doing selfie" };

            //foreach (var invalidTag in invalidTags)
            //{
            //    context.Tags.Add(new Tag { Name = TagTransformer.Transform(invalidTag) });
            //}

            context.SaveChanges();
        }

        private static void SeedPicturesToAlbums(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Pictures to Albums...");

            var albums = context.Albums.ToList();
            var pictureIds = context.Pictures.Select(p => p.Id).ToList();

            for (int i = 0; i < albums.Count; i++)
            {
                var currentAlbum = albums[i];
                var picturesCount = random.Next(0, MaxPicturesInAlbum);

                for (int j = 0; j < picturesCount; j++)
                {
                    try
                    {
                        currentAlbum.Pictures.Add(new PictureAlbum
                        {
                            PictureId = pictureIds[random.Next(0, pictureIds.Count)]
                        });

                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private static void SeedAlbums(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Albums...");

            var userIds = context.Users.Select(u => u.Id).ToList();

            for (int i = 0; i < userIds.Count; i++)
            {
                var currentUserId = userIds[i];
                var userAlbums = random.Next(0, TotalAlbumsPerUser);

                for (int j = 0; j < userAlbums; j++)
                {
                    var isPublic = false;
                    if (j % 3 == 0)
                    {
                        isPublic = true;
                    }

                    context.Albums.Add(new Album
                    {
                        Name = $"Album {j}/{userIds[i]}",
                        BackgroundColor = "default",
                        UserId = currentUserId,
                        IsPublic = isPublic
                    });

                    context.SaveChanges();
                }
            }
        }

        private static void SeedPictures(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Pictures...");

            for (int i = 0; i < TotalPictures; i++)
            {
                context.Pictures.Add(new Picture
                {
                    Title = $"PictureTitle{i}",
                    Caption = $"Caption{i}",
                    Path = $"Path{i}"
                });

                context.SaveChanges();
            }
        }

        private static void InitialSeed(SocialNetworkDbContext context)
        {
            SeedUsers(context); // with profile pictures
            SeedFriendships(context);

            Console.WriteLine("Database ready!");
        }

        private static void SeedFriendships(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Friendships...");

            var users = context.Users.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                var usersFriends = random.Next(MinFriends, MaxFriends);
                var currentUser = users[i];

                for (int j = 0; j < usersFriends; j++)
                {
                    var friend = users[random.Next(0, users.Count)];

                    if (friend.Id != currentUser.Id)
                    {
                        var friendshipMade = new UserFriend
                        {
                            UserId = currentUser.Id,
                            FriendId = friend.Id
                        };

                        var friendshipAccepted = new UserFriend
                        {
                            UserId = friend.Id,
                            FriendId = currentUser.Id
                        };

                        try
                        {
                            currentUser.FriendshipsMade.Add(friendshipMade);
                            currentUser.FriendshipsAccepted.Add(friendshipAccepted);

                            context.SaveChanges();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private static void SeedUsers(SocialNetworkDbContext context)
        {
            Console.WriteLine("Seeding Users...");

            for (int i = 0; i < TotalUsers; i++)
            {
                var isDeleted = false;
                if (i % 10 == 0)
                {
                    isDeleted = true;
                }

                context.Users.Add(new User
                {
                    Username = $"User#{i}",
                    Password = $"PASS!word@{i}",
                    Email = $"user{i * random.Next(0, TotalUsers)}@gmail.com",
                    Age = random.Next(1, 121),
                    //ProfilePicture = new byte[] { 1, 0, 1, 1, 0 }, // Disable for Task 3
                    RegisteredOn = DateTime.Now.AddDays(-i * 10 + TotalUsers),
                    IsDeleted = isDeleted
                });
            }

            context.SaveChanges();
        }

        private static void InitializeDatabase(SocialNetworkDbContext context)
        {
            Console.WriteLine("Initializing database...");
            context.Database.Migrate();
        }
    }
}
