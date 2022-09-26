using Moq;
using MovieRatingExample.Application;
using MovieRatingExample.Core.Model;
using MovieRatingExample.Core.Repositories;
using MovieRatingExample.Core.Service;

namespace XUnitTestProject
{
    public class ReviewServiceTest
    {
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
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(3, 0)]
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
        [InlineData(1, 3)]
        [InlineData(2, 3)]
        [InlineData(3, 0)]
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
        [InlineData(1, 4, 2)]
        [InlineData(1, 5, 1)]
        [InlineData(1, 3, 0)]
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
        [InlineData(1, 3)]
        [InlineData(2, 1)]
        [InlineData(3, 0)]
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
        [InlineData(1, 3)]
        [InlineData(2, 5)]
        [InlineData(3, 0)]
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
        [InlineData(1, 2, 2)]
        [InlineData(2, 5, 1)]
        [InlineData(3, 4, 0)]
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

        [Fact]
        public void GetMoviesWithHighestNumberOfTopRates()
        {
            // Arrange
            BEReview[] fakeRepo = new BEReview[]
            {
                new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 1, Grade=5, ReviewDate = new DateTime()},
                new BEReview() {Reviewer = 1, Movie = 2, Grade=5, ReviewDate = new DateTime()},
            };

            Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

            IReviewService service = new ReviewService(mockRepository.Object);

            // Act
            List<int> result = service.GetMoviesWithHighestNumberOfTopRates();

            // Assert
            Assert.Equal(1, result.Count);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }
    }
}
