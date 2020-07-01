using NUnit.Framework;
using Project;

namespace UserTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToString_CorrectUser_ReturnsEmail()
        {
            string userEmail = "a@a";
            

            var user = new User(userEmail);

            var result = user.ToString();

            Assert.AreEqual(user.Email, result);
        }
    }
}