using MovieRatingExample.Core.Models;
using MovieRatingExample.Domain.IRepositories;
using MovieRatingExample.Infrastructure.Data.Repositories;
using MovieRatingExample.Infrastructure.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestProject
{
    public class JsonReviewRepositoryTest
    {
        [Fact]
        public void JsonReviewReadNotNullTest()
        {
            var reviews = File.ReadAllText("Resources/ratings.json").FromJson<BEReview[]>();
            Assert.NotNull(reviews);
        }

        [Theory]
        //[InlineData("ratings.json")]
        [InlineData("ratings_small.json")]
        public void JsonReviewReadParseTest(string jsonFileName)
        {
            var reviews = File.ReadAllText($"Resources/{jsonFileName}").FromJson<BEReview[]>();
            IReviewRepository repository = new ReviewRepository(reviews.ToList());

            Assert.NotNull(repository.GetAll());
        }
    }
}
