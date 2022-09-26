using MovieRatingExample.Core.Model;
using MovieRatingExample.Core.Repositories;
using MovieRatingExample.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingExample.Application
{
    public class ReviewService : IReviewService
    {
        private IReviewRepository Repository;

        public ReviewService(IReviewRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentException("Missing repository");
            }
            Repository = repository;
        }

        public double GetAverageRateFromReviewer(int reviewer)
        {
            int count = 0;
            int score = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Reviewer == reviewer)
                {
                    count++;
                    score += review.Grade;
                }
            }
            return score > 0 ? score / count : 0;
        }

        public double GetAverageRateOfMovie(int movie)
        {
            int count = 0;
            int score = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Movie == movie)
                {
                    count++;
                    score += review.Grade;
                }
            }
            return score > 0 ? score / count : 0;
        }

        public List<int> GetMostProductiveReviewers()
        {
            throw new NotImplementedException();
        }

        public List<int> GetMoviesWithHighestNumberOfTopRates()
        {
            var ids = new Dictionary<int, int>();

            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Grade == 5)
                {
                    if (!ids.ContainsKey(review.Movie))
                        ids.Add(review.Movie, 1);
                    else
                    {
                        ids[review.Movie]++;
                    }
                }
            }

            //var collection = new Dictionary<int, int>();
            int highestNumber = 0;
            foreach (var entry in ids)
            {
                var foundAmount = entry.Value;

                if (foundAmount > highestNumber)
                    highestNumber = foundAmount;

            }

            var result = new List<int>();
            foreach (var entry in ids)
            {
                var movieId = entry.Key;
                var foundAmount = entry.Value;

                if (foundAmount == highestNumber)
                    result.Add(movieId);
            }

            return result;
        }

        public int GetNumberOfRates(int movie, int rate)
        {
            int count = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Movie == movie && review.Grade == rate)
                    count++;
            }
            return count;
        }

        public int GetNumberOfRatesByReviewer(int reviewer, int rate)
        {
            int count = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Reviewer == reviewer && review.Grade == rate)
                    count++;
            }
            return count;
        }

        public int GetNumberOfReviews(int movie)
        {
            int count = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Movie == movie)
                    count++;
            }
            return count;
        }

        public int GetNumberOfReviewsFromReviewer(int reviewer)
        {
            int count = 0;
            foreach (BEReview review in Repository.GetAll())
            {
                if (review.Reviewer == reviewer)
                    count++;
            }
            return count;
        }

        public List<int> GetReviewersByMovie(int movie)
        {
            throw new NotImplementedException();
        }

        public List<int> GetTopMoviesByReviewer(int reviewer)
        {
            throw new NotImplementedException();
        }

        public List<int> GetTopRatedMovies(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
