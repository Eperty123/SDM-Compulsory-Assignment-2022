using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingExample.Core.Model
{
    public class BEReview
    {
        public int Reviewer { get; set; }
        public int Movie { get; set; }
        public int Grade { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
