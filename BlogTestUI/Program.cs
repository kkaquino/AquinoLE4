using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BlogTestUI
{
    internal class Program
    {
        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            return new SqlData(dbAccess);
        }

        static async Task Main(string[] args)
        {
            var db = GetConnection();

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                var user = await Login(db);
                if (user == null)
                {
                    Console.WriteLine("Invalid credentials. Press Enter to exit.");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine($"Welcome, {user.UserName}");
                RunAppFeatures(db, user);
            }
            else if (choice == "2")
            {
                Register(db);
                Console.WriteLine("Registration complete. Press Enter to exit.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid choice. Press Enter to exit.");
                Console.ReadLine();
                return;
            }
        }

        static async Task<UserModel?> Login(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty;

            return await db.Authenticate(username, password);
        }

        static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter new password: ");
            string password = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter new first name: ");
            string firstName = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter new last name: ");
            string lastName = Console.ReadLine() ?? string.Empty;

            db.Register(username, firstName, lastName, password);
            Console.WriteLine("Registration successful.");
        }

        static void RunAppFeatures(SqlData db, UserModel user)
        {
            AddPost(db, user);
            ListPosts(db);
            SqlData.ShowPostDetails(db);
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void AddPost(SqlData db, UserModel user)
        {
            Console.Write("Title: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Body: ");
            string body = Console.ReadLine() ?? string.Empty;

            var post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            db.AddPost(post);
            Console.WriteLine("Post added successfully.");
        }

        static void ListPosts(SqlData db)
        {
            var posts = db.ListPosts();
            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Id}. {post.Title} by {post.UserName} [{post.DateCreated:yyyy-MM-dd}]");
                Console.WriteLine($"{(post.Body.Length > 20 ? post.Body.Substring(0, 20) + "..." : post.Body)}");
                Console.WriteLine();
            }
        }

    }
}
