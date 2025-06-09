using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public abstract class Person
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public string Nationality { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public bool isAlive { get; set; } = true;
        public bool isMale { get; set; } = true;

        public int Reputation { get; set; } = 100;
        public int Motivation { get; set; } = 100;
        public int Happiness { get; set; } = 100;

        public double Money { get; set; } = 100;
        public double WeeklySalary { get; set; } = 0;
        public double ExpectedSalary { get; set; } = 0;
        public int[] ContractExpiration { get; set; } = [0,0];

        public double MarketValue { get; set; } = 0;

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }


        [NotMapped]
        public string FullName => Nickname.Length > 0 ? $"{FirstName} \"{Nickname}\" {LastName}" : $"{FirstName} {LastName}";

    }
}
