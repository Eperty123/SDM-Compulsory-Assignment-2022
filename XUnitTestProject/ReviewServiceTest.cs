using Moq;
using MovieRatingExample.Core.IServices;
using MovieRatingExample.Core.Models;
using MovieRatingExample.Domain.IRepositories;
using MovieRatingExample.Domain.Services;
using Xunit.Abstractions;

namespace XUnitTestProject
{
    public class ReviewServiceTest
    {
        static ITestOutputHelper console;
        public ReviewServiceTest(ITestOutputHelper _console)
        {
            console = _console;
        }

        #region MemberData TestCases

        static IEnumerable<object[]> GetMoviesWithHighestNumberOfTopRates_TestCases()
        {
            // No highest top rates => empty list
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                },
                new List<int>(),
            };

            // One highest top rates => list(1)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 2, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                },
                new List<int>() {1},
            };

            // Two highest top rates => list(2)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                },
                new List<int>(){1,3},
            };
        }

        static IEnumerable<object[]> GetMostProductiveReviewers_TestCases()
        {
            // No reviwers => empty list
            yield return new object[]
            {
                new BEReview[]
                {
                },
                new List<int>(),
            };

            // One productive reviwer => list(1)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 2, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                },
                new List<int>() {1},
            };

            // Two productive reviwers => list(2)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 2, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 3, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 3, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                },
                new List<int>(){1,3},
            };
        }

        static IEnumerable<object[]> GetTopRatedMovies_TestCases()
        {
            // No reviwers => empty list
            yield return new object[]
            {
                new BEReview[]
                {
                },
                1,
                new List<int>(),
            };

            // One top rated movie => list(1)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 2, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                },
                3,
                new List<int>() {1, 2},
            };

            // Two top rated movies => list(2)
            yield return new object[]
            {
                new BEReview[]
                {
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 2, Movie = 2, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 3, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                    new BEReview() {Reviewer = 3, Movie = 3, Grade=5, ReviewDate = new DateTime()},
                },
                5,
                new List<int>(){1, 2, 3},
            };
        }

        #endregion

        #region Helper Methods
        static bool IsListIdentical(List<int> expectedResultList, List<int> resultList)
        {
            if (expectedResultList.Count != resultList.Count)
                return false;

            for (int i = 0; i < expectedResultList.Count; i++)
            {
                var expected = expectedResultList[i];
                var result = resultList[i];

                if (expected != result)
                    return false;

            }

            return true;
        }
        #endregion

        [Fact]
        public void CreateReviewServiceWithRepository()
        {
            // Arrange
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            IReviewRepository repository = mockRepository.Object;

            // Act
            IReviewService service = new ReviewService(repository);

            // Assert
            Assert.NotNull(service);
            Assert.True(service is ReviewService);
        }

        [Fact]
        public void CreateReviewServiceWithNoRepositoryExceptArgumentException()
        {
            // Arrange
            IReviewService service = null;

            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => service = new ReviewService(null));

            Assert.Equal("Missing repository", ex.Message);
            Assert.Null(service);
        }

        [Theory]
        [InlineData(1, 2)] // Reviewer 1 with 2 reviews
        [InlineData(2, 1)] // Reviewer 2 with one review
        [InlineData(3, 0)] // Non-existent reviewer 3 with no reviews
        public void GetNumberOfReviewsFromReviewer(int reviewer, int expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=3, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 2, Movie = 1, Grade=3, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=3, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            int result = service.GetNumberOfReviewsFromReviewer(reviewer);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 3)] // Reviewer 1 with multiple reviews
        [InlineData(2, 3)] // Reviewer 2 with one review
        [InlineData(3, 0)] // Non-existent reviewer 3 with no reviews
        public void GetAverageRateFromReviewer(int reviewer, double expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=1, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 2, Movie = 1, Grade=3, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            double result = service.GetAverageRateFromReviewer(reviewer);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 4, 2)] // Reviewer 1 who gave a grade of 4 multiple times
        [InlineData(1, 5, 1)] // Reviewer 1 who gave a grade of 5 once
        [InlineData(1, 3, 0)] // Reviewer 2 who gave a grade of 3 zero times
        public void GetNumberOfRatesByReviewer(int reviewer, int rate, int expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            int result = service.GetNumberOfRatesByReviewer(reviewer, rate);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 3)] // Movie with id 1 with 3 reviews
        [InlineData(2, 1)] // Movie with id 2 with  1 review
        [InlineData(3, 0)] // Movie with id 3 with no reviews
        public void GetNumberOfReviews(int movie, int expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            int result = service.GetNumberOfReviews(movie);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 3)] // Movie with id 1 of average rate of 3 from multiple reviews
        [InlineData(2, 5)] // Movie with id 2 of average rate of 5 from one review
        [InlineData(3, 0)] // Movie with id 3 of average rate of 0 from no reviews
        public void GetAverageRateOfMovie(int movie, double expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            double result = service.GetAverageRateOfMovie(movie);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 2)] // Movie with id 1 with a rate of 2 with multiple reviews
        [InlineData(2, 5, 1)] // Movie with id 2 with a rate of 5 with one review
        [InlineData(3, 4, 0)] // Movie with id 3 with a rate of 4 with no reviews
        public void GetNumberOfRates(int movie, int rate, int expectedResult)
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            int result = service.GetNumberOfRates(movie, rate);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        // TODO: Create MemberData Test class method that returns an enumerable of object.
        [Theory]
        [MemberData(nameof(GetMoviesWithHighestNumberOfTopRates_TestCases))]
        public void GetMoviesWithHighestNumberOfTopRates(BEReview[] fakeRepo, List<int> expectedResult)
        {
            // Arrange
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetMoviesWithHighestNumberOfTopRates();

            // Assert
            Assert.True(IsListIdentical(expectedResult, result));
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetMostProductiveReviewers_TestCases))]
        public void GetMostProductiveReviewers(BEReview[] fakeRepo, List<int> expectedResult)
        {
            // Arrange
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetMostProductiveReviewers();

            // Assert
            Assert.True(IsListIdentical(expectedResult, result));
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetTopRatedMovies_TestCases))]
        public void GetTopRatedMovies(BEReview[] fakeRepo, int amount, List<int> expectedResult)
        {
            // Arrange
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetTopRatedMovies(amount);

            // Assert
            Assert.True(IsListIdentical(expectedResult, result));
            mockRepository.Verify(r => r.GetAll(), Times.AtLeastOnce);
        }

        [Fact]
        public void GetTopRatedMoviesInvalidInput()
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            int amount = -1;
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act + assert
            var ex = Assert.Throws<ArgumentException>(() => service.GetTopRatedMovies(amount));

            Assert.Equal("Amount must be 1 or bigger", ex.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Never);
        }

        [Fact]
        public void GetTopMoviesByReviewer()
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 3, Grade=5, ReviewDate = new DateTime(2019,05,08)},
                new BEReview() {Reviewer = 1, Movie = 4, Grade=5, ReviewDate = new DateTime(2019,05,09) },
            };

            List<int> expectedResult = new List<int>
            {
                3,
                4,
                2,
                1
            };

            int reviewerId = 1;
            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetTopMoviesByReviewer(reviewerId);

            // Assert
            Assert.True(IsListIdentical(expectedResult, result));
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetReviewersByMovie()
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=2, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 2, Movie = 1, Grade=4, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 3, Movie = 1, Grade=5, ReviewDate = new DateTime(2019,05,08)},
                new BEReview() {Reviewer = 4, Movie = 1, Grade=5, ReviewDate = new DateTime(2019,05,09) },
            };

            List<int> expectedResult = new List<int>
            {
                3,
                4,
                2,
                1
            };

            int movieId = 1;

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetReviewersByMovie(movieId);

            // Assert
            Assert.True(IsListIdentical(expectedResult, result));
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }
    }
}
