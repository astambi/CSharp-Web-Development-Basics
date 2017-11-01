namespace SocialNetwork.Core
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SocialNetwork.Data;
    using SocialNetwork.Models;
    using SocialNetwork.Utils;

    public class Engine
    {
        private const int TotalUsers = 50;
        private const int TotalPictures = 50;
        private const int TotalAlbumsPerUser = 5;
        private const int TotalTags = 50;
        private const int MinFriends = 5;
        private const int MaxFriends = 10;
        private const int MaxPicturesInAlbum = 10;
        private const int MaxTagInAlbum = 5;
        private const int MaxSharedAlbumsPerUser = 5;

        private static Random random = new Random();

        public void Run()
        {
            // Initialize database 
            InitializeDatabase();

            // Seed data
            SeedUsers();                          // Profile pictures removed in Task 3
            SeedFriendships();                    // Task 2
            SeedAlbums();                         // Task 3
            SeedPictures();                       // Task 3
            SeedPicturesToAlbums();               // Taks 3
            SeedTags();                           // Task 4
            SeedTagsToAlbums();                   // Task 4
            SeedUsersToSharedAlbums();            // Task 5 & Modified for Task 6 UserRoles

            Console.WriteLine("Seeding done! Running queries...");

            // Queries
            using (var context = new SocialNetworkDbContext())
            {
                PrintUsersWithFriendsAndStatuses(context);        // Task 2
                PrintActiveUsersWithoreThan5Friends(context);     // Task 2

                PrintAlbumsWithOwnersAndPictures(context);        // Task 3
                PrintPicturesInMoreThan2Albums(context);          // Task 3
                PrintUserAlbum(context);                          // Task 3

                PrintAlbumsByTag(context);                        // Task 4
                PrintUsersWithAlbumsWithMoreThan3Tags(context);   // Task 4

                PrintUsersWithOthersInSharedAlbums(context);      // Task 5
                PrintAlbumsSharedWithMoreThan2People(context);    // Task 5
                PrintAlbumsSharedWithUser(context);               // Task 5

                PrintAlbumsWithUsersAndRoles(context);            // Task 6
                PrintAlbumsByUser(context);                       // Task 6
                PrintUsersThatAreViewersOfAtLeast1Album(context); // Task 6
            }
        }

        private void PrintUsersThatAreViewersOfAtLeast1Album(SocialNetworkDbContext context)
        {
            Console.WriteLine("Users viewing at least 1 public album:");

            var usersData = context
                .Users
                .Select(u => new
                {
                    Name = u.Username,
                    PublicAlbums = u.SharedAlbums
                                    .Where(sa => sa.Album.IsPublic)
                                    .Select(sa => sa.UserRole == UserRole.Viewer)
                                    .Count()
                })
                .Where(u => u.PublicAlbums > 0)
                .ToList();

            var builder = new StringBuilder();
            foreach (var user in usersData)
            {
                builder.AppendLine($"{user.Name} - Viewing Public Albums {user.PublicAlbums}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintAlbumsByUser(SocialNetworkDbContext context)
        {
            Console.WriteLine("User albums in different roles:");

            var userId = context
                .Users
                .Where(u => u.SharedAlbums.Any(sa => sa.UserRole == UserRole.Owner))
                .Select(u => u.Id)
                .FirstOrDefault();

            var albumsData = context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    OwnerCount = u.SharedAlbums
                                  .Where(sa => sa.UserRole == UserRole.Owner)
                                  .Count(),
                    ViewerCount = u.SharedAlbums
                                  .Where(sa => sa.UserRole == UserRole.Viewer)
                                  .Count()
                })
                .FirstOrDefault();

            Console.WriteLine($"Owner {albumsData.OwnerCount} - Viewer {albumsData.ViewerCount} {Environment.NewLine}");
        }

        private void PrintAlbumsWithUsersAndRoles(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums with users and roles:");

            var albumsData = context
                .Albums
                .Where(a => a.SharedAlbums.Any())
                .Select(a => new
                {
                    Name = a.Name,
                    Owner = a.SharedAlbums
                            .Where(sa => sa.UserRole == UserRole.Owner)
                            .Select(sa => sa.User.Username)
                            .FirstOrDefault(),
                    Viewers = a.SharedAlbums
                             .Where(sa => sa.UserRole == UserRole.Viewer)
                             .Count(),
                    Users = a.SharedAlbums.Select(sa => new
                    {
                        User = sa.User.Username,
                        UserRole = sa.UserRole
                    })
                })
                .OrderBy(a => a.Owner)
                .ThenByDescending(a => a.Viewers)
                .ToList();

            var builder = new StringBuilder();
            foreach (var album in albumsData)
            {
                builder.AppendLine($"{album.Name}:");

                foreach (var user in album.Users)
                {
                    builder.AppendLine($"  {user.User} - {user.UserRole}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintAlbumsSharedWithUser(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums shared with user with pictures count:");

            var userId = context
                        .Users
                        .Where(u => u.SharedAlbums.Any())
                        .Select(u => u.Id)
                        .FirstOrDefault();

            var albumsData = context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    SharedAlbums = u.SharedAlbums.Select(sa => new
                    {
                        Name = sa.Album.Name,
                        Pictures = sa.Album.Pictures.Count
                    })
                    .OrderByDescending(a => a.Pictures)
                    .ThenBy(a => a.Name)
                    .ToList()
                })
                .FirstOrDefault();

            var builder = new StringBuilder();
            foreach (var album in albumsData.SharedAlbums)
            {
                builder.AppendLine($"{album.Name} - Pictures {album.Pictures}");
            }

            Console.WriteLine(builder.ToString());

        }

        private void PrintAlbumsSharedWithMoreThan2People(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums shared with more than 2 users:");

            var albumsData = context
                .Albums
                .Where(a => a.SharedAlbums.Count > 2)
                .Select(a => new
                {
                    a.Name,
                    a.IsPublic,
                    People = a.SharedAlbums.Select(sa => sa.User).Count()
                })
                .OrderByDescending(a => a.People)
                .ThenBy(a => a.Name)
                .ToList();


            var builder = new StringBuilder();
            foreach (var album in albumsData)
            {
                var isPublic = album.IsPublic ? "Public" : "Private";

                builder.AppendLine($"{album.Name} - Shared with {album.People} users - {isPublic}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintUsersWithOthersInSharedAlbums(SocialNetworkDbContext context)
        {
            Console.WriteLine("Users with albums shared with friends:");

            var usersData = context
                .Users
                .Where(u => u.SharedAlbums.Any())
                .Where(u => u.FriendshipsMade.Any())
                .Select(u => new
                {
                    Name = u.Username,
                    Friends = u.FriendshipsMade.Select(f => f.Friend.Username),
                    SharedAlbums = u.SharedAlbums.Select(sa => new
                    {
                        Name = sa.Album.Name,
                        SharedWith = sa.Album.SharedAlbums.Select(a => a.User.Username)
                    })
                })
                .OrderBy(u => u.Name)
                .ToList();

            var builder = new StringBuilder();
            foreach (var user in usersData)
            {
                builder.AppendLine($"{user.Name}:");

                foreach (var friend in user.Friends)
                {
                    var albumsSharedWithFriend = user
                        .SharedAlbums
                        .Where(sa => sa.SharedWith.Contains(friend));

                    if (albumsSharedWithFriend.Any())
                    {
                        builder.AppendLine($"  Shared with {friend}:");
                        foreach (var album in albumsSharedWithFriend)
                        {
                            builder.AppendLine($"    {album.Name}");
                        }
                    }
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintUsersWithAlbumsWithMoreThan3Tags(SocialNetworkDbContext context)
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
                    .AppendLine($"{album.Title} - Owner {album.Owner}:")
                    .AppendLine($"  {string.Join(", ", album.Tags)}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintAlbumsByTag(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums by tag:");

            var tagId = context
                        .Tags
                        .Where(t => t.Albums.Any())
                        .Select(t => t.Id)
                        .FirstOrDefault();

            var albumsData = context
                .Albums
                .Where(a => a.Tags.Any(t => t.TagId == tagId))
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
                builder.AppendLine($"{album.Name} - Owner {album.Owner}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintUserAlbum(SocialNetworkDbContext context)
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
                builder.AppendLine($"{album.Name} - Owner {album.Owner}:");

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

        private void PrintPicturesInMoreThan2Albums(SocialNetworkDbContext context)
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
                    builder.AppendLine($"  {album.Title} - Owner {album.Owner}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintAlbumsWithOwnersAndPictures(SocialNetworkDbContext context)
        {
            Console.WriteLine("Albums with owners and pictures:");

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

        private void PrintActiveUsersWithoreThan5Friends(SocialNetworkDbContext context)
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

        private void PrintUsersWithFriendsAndStatuses(SocialNetworkDbContext context)
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

        private void SeedUsersToSharedAlbums()
        {
            Console.WriteLine("Seeding Users to SharedAlbums...");

            // User navigation properties in Albums are not deleted on migration to SharesAlbums (UserAlbums)
            // Existing data in Albums is transfered to SharedAlbums

            using (var context = new SocialNetworkDbContext())
            {
                var albums = context.Albums.ToList();
                var usersWithFriends = context
                    .Users
                    .Where(u => u.FriendshipsMade.Any())
                    .Select(u => new
                    {
                        User = u,
                        Friends = u.FriendshipsMade.Select(f => f.Friend).ToList(),
                        Albums = u.Albums.Select(a => a.Id).ToList()
                    })
                    .ToList();

                for (int i = 0; i < usersWithFriends.Count; i++)
                {
                    var currentUserData = usersWithFriends[i];

                    var currentUser = currentUserData.User;
                    var userFriends = currentUserData.Friends;
                    var userAlbumsIds = currentUserData.Albums;

                    for (int j = 0; j < userAlbumsIds.Count; j++)
                    {
                        var currentAlbumId = userAlbumsIds[j];

                        // Add current user to shared album
                        try
                        {
                            currentUser.SharedAlbums.Add(new UserAlbum
                            {
                                AlbumId = currentAlbumId,
                                UserRole = UserRole.Owner // Task 6 UserRoles
                            });

                            context.SaveChanges();
                        }
                        catch (Exception)
                        {
                        }

                        // Add user friends to shared album
                        var friendsInCurrentAlbum = random.Next(1, userFriends.Count);

                        for (int k = 0; k < friendsInCurrentAlbum; k++)
                        {
                            try
                            {
                                var currentFriend = userFriends[k];
                                currentFriend.SharedAlbums.Add(new UserAlbum
                                {
                                    AlbumId = currentAlbumId
                                });

                                context.SaveChanges();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        private void SeedTagsToAlbums()
        {
            Console.WriteLine("Seeding Tags to Albums...");

            using (var context = new SocialNetworkDbContext())
            {
                var albums = context.Albums.ToList();
                var tagIds = context.Tags.Select(t => t.Id).ToList();

                for (int i = 0; i < albums.Count; i++)
                {
                    var currentAlbum = albums[i];
                    var tagsCount = random.Next(2, MaxTagInAlbum);

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
        }

        private void SeedTags()
        {
            Console.WriteLine("Seeding Tags...");

            using (var context = new SocialNetworkDbContext())
            {
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
        }

        private void SeedPicturesToAlbums()
        {
            Console.WriteLine("Seeding Pictures to Albums...");

            using (var context = new SocialNetworkDbContext())
            {
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
        }

        private void SeedAlbums()
        {
            Console.WriteLine("Seeding Albums...");

            using (var context = new SocialNetworkDbContext())
            {
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
        }

        private void SeedPictures()
        {
            Console.WriteLine("Seeding Pictures...");

            using (var context = new SocialNetworkDbContext())
            {
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
        }

        private void SeedFriendships()
        {
            Console.WriteLine("Seeding Friendships...");

            using (var context = new SocialNetworkDbContext())
            {
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
        }

        private void SeedUsers()
        {
            Console.WriteLine("Seeding Users...");

            using (var context = new SocialNetworkDbContext())
            {
                for (int i = 0; i < TotalUsers; i++)
                {
                    var isDeleted = false;
                    if (i % 10 == 0)
                    {
                        isDeleted = true;
                    }

                    var user = new User
                    {
                        Username = $"User#{i}",
                        Password = $"PASS!word@{i}",
                        Email = $"user{i * random.Next(0, TotalUsers)}@gmail.com",
                        Age = random.Next(1, 121),
                        //ProfilePicture = new byte[] { 1, 0, 1, 1, 0 }, // Disable for Task 3
                        RegisteredOn = DateTime.Now.AddDays(-i * 10 + TotalUsers),
                        IsDeleted = isDeleted
                    };

                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        private void InitializeDatabase()
        {
            Console.WriteLine("Initializing database...");

            using (var context = new SocialNetworkDbContext())
            {
                context.Database.EnsureDeleted(); // for testing purposes only

                context.Database.Migrate();

                Console.WriteLine("Database ready!");
            }
        }
    }
}
