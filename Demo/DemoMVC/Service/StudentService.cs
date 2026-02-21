using Demo.DemoMVC.IService;
using Demo.DemoMVC.Model;
using System;
using static Demo.DemoMVC.Service.StudentService;

namespace Demo.DemoMVC.Service
{

    public class StudentService: IStudentService
    {
        public Task<StudentModel> CreateAsync(StudentModel dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StudentModel?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, StudentModel dto)
        {
            throw new NotImplementedException();
        }

    }
}
