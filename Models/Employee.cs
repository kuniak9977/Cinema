using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class Employee
    {
        private string name;
        private string surname;
        private Occupation role;
        private short employeePrivateCode;

        private string normalizedName;

        private List<Employee> listOfEmployees;

        private const short key = 9977;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string NormalizedName { get => normalizedName; set => normalizedName = value; }
        public short EmployeePrivateCode { get => employeePrivateCode; set => employeePrivateCode = value; }
        public List<Employee> ListOfEmployees { get => listOfEmployees; set => listOfEmployees = value; }
        public Occupation Role { get => role; set => role = value; }

        public Employee() { }
        public Employee(string _name, string _surname, short _code, int _role)
        {
            this.name = _name;
            this.surname = _surname;
            this.role = ConvertFromInt(_role);
            this.normalizedName = NormalizeName(_name, _surname);
            this.listOfEmployees = new List<Employee>();
            SetEncryptedCode(_code);
        }

        public void AddToList(Employee _emp)
        {
            listOfEmployees.Add(_emp);
        }

        public string NormalizeName(string _name, string _surname)
        {
            return $"{_name} {_surname}".ToUpperInvariant();
        }

        public override string ToString()
        {
            return $"{name} {surname} - {role}";
        }

        private static Occupation ConvertFromInt(int _role)
        {
            if (Enum.IsDefined(typeof(Occupation), _role))
                return (Occupation)_role;
            else
                throw new ArgumentOutOfRangeException(nameof(_role), "Niepoprawna wartość _role");
        }

        public short GetDecryptedCode()
        {
            return (short)(employeePrivateCode ^ key);
        }

        public void SetEncryptedCode(short _code)
        {
            this.employeePrivateCode = (short)(_code ^ key);
        }
        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   employeePrivateCode == employee.employeePrivateCode;
        }

        public override int GetHashCode()
        {
            return employeePrivateCode.GetHashCode();
        }

        public static void SortByRole(List<Employee> _employees)
        {
            _employees.Sort((e1,e2) => e1.Role.CompareTo(e2.Role));
        }

        public enum Occupation
        {
            Nieprzydzielony = 5,
            Dyrektor = 0,
            Kierwonik = 1,
            Menedżer = 2,
            Specjalista = 3,
            Pracownik = 4,
        }
    }
}
