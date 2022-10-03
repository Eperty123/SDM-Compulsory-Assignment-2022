using MovieRatingExample.Core.Models;
using MovieRatingExample.Domain.IRepositories;

namespace MovieRatingExample.Infrastructure.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        readonly List<BEReview> reviews;

        public ReviewRepository(List<BEReview> reviews)
        {
            this.reviews = reviews;
        }

        public BEReview[] GetAll()
        {
            return reviews.ToArray();
        }
    }
}
