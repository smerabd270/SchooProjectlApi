using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public interface IGradeService
    {
        Task<Grade> AddGradeAsync(Grade grade);
        Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId);
    }
}
