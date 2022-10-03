using MovieRatingExample.Core.Models;

namespace MovieRatingExample.Domain.IRepositories
{
    public interface IReviewRepository
    {
        BEReview[] GetAll();
    }
}
