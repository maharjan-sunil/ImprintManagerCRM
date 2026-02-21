using Demo.DemoMVC.Model;

namespace Demo.DemoMVC.IService
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentModel>> GetAllAsync();
        Task<StudentModel?> GetByIdAsync(int id);
        Task<StudentModel> CreateAsync(StudentModel dto);
        Task<bool> UpdateAsync(int id, StudentModel dto);
        Task<bool> DeleteAsync(int id);
    }
}
