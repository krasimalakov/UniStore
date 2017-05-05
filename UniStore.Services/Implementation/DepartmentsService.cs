namespace UniStore.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models.BindingModels.Departments;
    using Models.EntityModels;
    using Models.ViewModels.Department;

    public class DepartmentsService : BaseService, IDepartmentsService
    {
        public DepartmentsService(IUniStoreContext context)
            : base(context)
        {
        }

        public IEnumerable<DepartmentVM> GetDepartmentVMs()
        {
            var departments = this.Context.Departments.All().ToList();
            return Mapper.Map<IEnumerable<DepartmentVM>>(departments);
        }

        public DepartmentVM GetDepartmentVM(int id)
        {
            var department = this.Context.Departments.Find(id);
            return Mapper.Map<DepartmentVM>(department);
        }

        public bool IsExistDepartmentWithName(string name)
        {
            return this.Context.Departments.All()
                .Any(d => string.Equals(d.Name, name));
        }

        public bool IsExistOtherDepartmentWithName(int id, string name)
        {
            return this.Context.Departments.All()
                .Any(d => string.Equals(d.Name, name) && d.Id != id);
        }

        public Department GetDepartmentById(int id)
        {
            return this.Context.Departments.Find(id);
        }

        public void CreateDepartment(AddDepartmentBM departmentBM)
        {
            var department = Mapper.Map<Department>(departmentBM);
            this.Context.Departments.Add(department);
            this.Context.SaveChanges();
        }

        public void RemoveDepartment(int id)
        {
            this.Context.Departments.Remove(id);
            this.Context.SaveChanges();
        }

        public void UpdateDepartment(EditDepartmentBM departmentBM)
        {
            var department = this.Context.Departments.Find(departmentBM.Id);
            if (department == null)
            {
                return;
            }

            department.Name = departmentBM.Name;
            this.Context.SaveChanges();
        }

    }
}