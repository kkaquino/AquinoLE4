using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<UserModel?> Authenticate(string username, string password)
        {
            var users = await _db.LoadData<UserModel, dynamic>(
                "dbo.spUsers_Authenticate",
                new { username, password },
                connectionStringName,
                true);
            return users?.FirstOrDefault();
        }

        public void Register(string username, string firstName, string lastName, string password)
        {
            _db.SaveData<dynamic>(
                "dbo.spUsers_Register",
                new { userName = username, firstName, lastName, password },
                connectionStringName,
                true);
        }


        public void AddPost(PostModel post)
        {
            _db.SaveData(
                "dbo.spPosts_Insert",
                new
                {
                    userId = post.UserId,
                    title = post.Title,
                    body = post.Body,
                    dateCreated = post.DateCreated
                },
                connectionStringName,
                true);
        }

        public List<ListPostModel> ListPosts()
        {
            var postsTask = _db.LoadData<ListPostModel, dynamic>("dbo.spPosts_List", new { }, connectionStringName, true);
            return postsTask.Result.ToList();
        }

        public ListPostModel? ShowPostDetails(int id)
        {
            var postTask = _db.LoadData<ListPostModel, dynamic>("dbo.spPosts_Detail", new { id }, connectionStringName, true);
            return postTask.Result.FirstOrDefault();
        }

        public static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a Post ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Post ID.");
                return;
            }

            var post = db.ShowPostDetails(id);
            if (post == null)
            {
                Console.WriteLine("Post not found.");
                return;
            }

            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");
            Console.WriteLine();
            Console.WriteLine(post.Body);
            Console.WriteLine(post.DateCreated.ToString("MM dd yyyy"));
        }
    }
}
