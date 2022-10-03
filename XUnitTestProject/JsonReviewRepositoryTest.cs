using MovieRatingExample.Core.Models;
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
    }
}
