namespace DDD.ServiceOrder.Api.Application.ViewModels
{
    public class TechnicianViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<ServiceOrderViewModel> ServiceOrders { get; set; }
    }
}
