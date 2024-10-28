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
        private short employeePrivateCode;

        private const short key = 9977;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public short EmployeePrivateCode { get => employeePrivateCode; set => employeePrivateCode = value; }

        public Employee() { }
        public Employee(string _name, string _surname, short _code)
        {
            this.name = _name;
            this.surname = _surname;
            SetEncryptedCode(_code);
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
    }
}
