namespace Appointment.Service
{
    public interface IAppointmentService
    {
        Task<string> Create();
        Task<string> Update();
        Task<string> Delete(string query);
        Task<string> Get(string query);

        Task<string> GetAll(string query);
    }
}
