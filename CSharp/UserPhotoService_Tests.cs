using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleTest.CSharp
{
    [TestClass]
    public class UserPhotoService_Tests
    {
        const string url1 = "http://the.first.photo";
        const string url2 = "http://the.second.photo";
        const string url3 = "http://the.third.photo";
        const string url4 = "http://the.fourth.photo";
        private readonly IDictionary<string, string> userPhotos = new Dictionary<string, string>{
            { "1", url1 },
            { "2", url2 },
            { "3", url3 },
            { "4", url4 },
        };
        private readonly List<User> fakeUsers = new String[] { "1", "2", "3", "4" }
       .Select(id => { return new User { Id = id }; }).ToList();

        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod]
        public async Task GetUsersWithPhotos_Test()
        {
            // Arrange
            // Return the list of dummy users
            var mockRequestService = new Mock<IHttpService>();
            mockRequestService
                .Setup(m => m.Get<List<User>>("/users"))
                .Returns(Task.FromResult(fakeUsers));

            // Return the user's photo using the user id to cross-reference the dictionary
            mockRequestService
                .Setup(m => m.Get<UserPhoto>(It.IsAny<string>()))
                .Returns((string url) =>
                {
                    string userId = new string(url.Where(x => char.IsDigit(x)).ToArray()); // expected URL format is $"/users/{user.Id}/photo"
                    return Task.FromResult(new UserPhoto
                    {
                        Url = userPhotos[userId]
                    });
                });

            var userPhotoService = new UserPhotoService(mockRequestService.Object);

            //Act
            var users = await userPhotoService.GetUsers();

            //Assert to confirm that the photo of each user has been populated successfully
            Assert.AreEqual(users, fakeUsers);
            CollectionAssert.AreEqual(users.Select(x => x.Photo).ToList(), userPhotos.Select(x => x.Value).ToList());
        }
    }
}
