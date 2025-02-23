using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class InfoContact : ValueObject
    {
        public InfoContact() { }
        public InfoContact(string email, string phone)
        {
            Email = email;
            Phone = phone;
        }

        public string Email { get; private set; }
        public string Phone { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return Phone;
        }
    }
}
