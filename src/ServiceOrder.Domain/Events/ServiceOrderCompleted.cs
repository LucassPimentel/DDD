namespace ServiceOrder.Domain.Events
{
    public class ServiceOrderCompleted
    {
        public int ServiceOrderId { get; set; }
        public string TechnicianName { get; set; }
        public string TechnicianEmail { get; set; }
        public string ServiceOrderCompletedAt { get; set; }
    }
}
