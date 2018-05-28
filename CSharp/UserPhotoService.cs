using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTest.CSharp
{
    public class User
    {
        public string Id { set; get; }
        public string Photo { set; get; }
    }

    public class UserPhoto
    {
        public string Url { set; get; }
    }

    public class UserPhotoService
    {
        /// <summary>
        /// This function is meant to retrieve a list of all users from a server (assume request.get is an actual function)
        /// After retrieving the users, it retrieves the picture for each user (in a non-blocking manner) 
        /// and patches the original user object with the newly retrieved photo URL.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsers()
        {
            var users = await Request.Get<List<User>>("/users");
            var photoTasks = new List<Task>();

            #region Populate photographs
            foreach(var user in users)
            {
                photoTasks.Add(GetUserPhoto(user));
            }
            #endregion

            await Task.WhenAll(photoTasks);
            return users;
        }

        /// <summary>
        /// This function retrieves the picture for a given user
        /// </summary>
        /// <param name="theUser">the given user</param>
        /// <returns></returns>
        public async Task GetUserPhoto(User theUser)
        {
            var thePhoto = await Request.Get<UserPhoto>($"/users/{theUser.Id}/photo");
            theUser.Photo = thePhoto.Url;
        }
    }

    /// <summary>
    /// Send in your wishes. I am the benevolent one! 
    /// </summary>
    public static class Request
    {
        /// <summary>
        /// Makes an HTTP GET request to the specified URL and deserialize the response into an object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> Get<T>(string url) where T : class
        {
            throw new NotImplementedException("Implementation not required");
        }
    }
}
