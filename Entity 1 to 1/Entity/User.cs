using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
/*
 int : int

bit : bool

char : string

date : DateTime

datetime : DateTime

datetime2 : DateTime

decimal : decimal

float : double

money : decimal

nchar : string

ntext : string

numeric : decimal

nvarchar : string

real : float

smallint : short

text : string

tinyint : byte

varchar : string
     
*/
namespace Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        //public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public User User { get; set; }
    }
    public class Man
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public int ManId { get; set; }
        public string Name { get; set; }
        public Man Man { get; set; }
    }

}
