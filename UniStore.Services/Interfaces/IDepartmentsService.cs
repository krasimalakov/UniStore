namespace UniStore.Services.Interfaces
{
    using System.Collections.Generic;
    using Models.BindingModels.Departments;
    using Models.EntityModels;
    using Models.ViewModels.Department;

    public interface IDepartmentsService
    {
        IEnumerable<DepartmentVM> GetDepartmentVMs();

        DepartmentVM GetDepartmentVM(int id);

        bool IsExistDepartmentWithName(string name);

        bool IsExistOtherDepartmentWithName(int id, string name);

        Department GetDepartmentById(int id);

        void CreateDepartment(AddDepartmentBM departmentBM);

        void RemoveDepartment(int id);

        void UpdateDepartment(EditDepartmentBM departmentBM);
    }
}