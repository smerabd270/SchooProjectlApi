using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public interface IAssignmentService
    {
        Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(int courseId);
        Task<Assignment> CreateAssignmentAsync(Assignment assignment);
    }
}
