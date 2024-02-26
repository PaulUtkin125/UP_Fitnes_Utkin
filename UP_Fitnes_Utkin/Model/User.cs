using System.ComponentModel.DataAnnotations.Schema;


namespace UP_Fitnes_Utkin.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public Rol Role { get; set; } = null!;
        public int RoleId {  get; set; }
        public double Money { get; set; }
    }
}
