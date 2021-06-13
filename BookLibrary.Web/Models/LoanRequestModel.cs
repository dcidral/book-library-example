using System;

namespace BookLibrary.Web.Models
{
    public class LoanRequestModel
    {
        public string User { get; set; }
        public DateTime Borrowed { get; set; }
    }
}
