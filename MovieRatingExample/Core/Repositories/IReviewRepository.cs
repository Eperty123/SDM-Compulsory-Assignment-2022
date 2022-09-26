using MovieRatingExample.Core.Model;

namespace MovieRatingExample.Core.Repositories
{
    public interface IReviewRepository
    {
        BEReview[] GetAll();
    }
}
