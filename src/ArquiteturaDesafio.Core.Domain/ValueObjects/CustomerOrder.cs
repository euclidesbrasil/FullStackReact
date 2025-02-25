using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class CustomerOrder : ValueObject
    {
        public CustomerOrder(Guid id, string name, string email, string phone)
        {
            Id = id.ToString();
            Name = name;
            Email = email;
            Phone = phone;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
            yield return Email;
            yield return Phone;
        }
    }


}
