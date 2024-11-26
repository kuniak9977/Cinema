namespace Cinema.Models
{
    public class Employee
    {
        private int id;
        private string name;
        private string surname;
        private Occupation role;
        private short employeePrivateCode;
        private string normalizedName;

        private List<int> subordinateIds;

        private static int lastUsedId = 0;

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string NormalizedName { get => normalizedName; set => normalizedName = value; }
        public short EmployeePrivateCode { get => employeePrivateCode; set => employeePrivateCode = value; }
        public Occupation Role { get => role; set => role = value; }
        public int Id { get => id; }
        public List<int> SubordinateIds { get => subordinateIds; set => subordinateIds = value; }

        public Employee()
        {
            this.id = GetNextAvailableId();
            this.subordinateIds = new List<int>();
        }

        public Employee(string _name, string _surname, short _code, int _role)
        {
            this.id = GetNextAvailableId();
            this.name = _name;
            this.surname = _surname;
            this.role = ConvertFromInt(_role);
            this.normalizedName = NormalizeName(_name, _surname);
            this.subordinateIds = new List<int>();
            this.employeePrivateCode = _code;
        }
        public string NormalizeName(string _name, string _surname)
        {
            return $"{_name} {_surname}".ToUpperInvariant();
        }

        public void AddSubordinate(int subordinateId)
        {
            subordinateIds.Add(subordinateId);
        }

        public static Occupation ConvertFromInt(int _role)
        {
            if (Enum.IsDefined(typeof(Occupation), _role))
                return (Occupation)_role;
            else
                throw new ArgumentOutOfRangeException(nameof(_role), "Niepoprawna wartość _role");
        }

        private static int GetNextAvailableId()
        {
            return ++lastUsedId;
        }

        public enum Occupation
        {
            Nieprzydzielony = 5,
            Dyrektor = 0,
            Kierownik = 1,
            Menedżer = 2,
            Specjalista = 3,
            Pracownik = 4,
        }
    }
}
