namespace TaskTracking.Staj.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string?  Description  { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public string? Priority { get; set; }


        public int UserId { get; set; }
        public User? User { get; set; }




    }
}
