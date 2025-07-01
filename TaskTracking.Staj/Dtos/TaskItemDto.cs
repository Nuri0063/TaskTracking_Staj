namespace TaskTracking.Staj.Dtos
{
    public class TaskItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Priority { get; set; }
    }
}
