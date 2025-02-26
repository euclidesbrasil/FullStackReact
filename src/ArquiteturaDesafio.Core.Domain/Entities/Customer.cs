using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.Common;

namespace ArquiteturaDesafio.Core.Domain.Entities
{
    public class Customer: BaseEntity
    {
        public Customer()
        {
            Identification = new InfoContact();
        }

        public Customer(string name, InfoContact infoContact)
        {
            Name = name;
            Identification = infoContact;
        }

        public string Name { get; set; }
       
        public InfoContact Identification { get; set; }

        public void Update(string name, InfoContact infoContact )
        {
            Name = name;
            Identification = infoContact;
        }

        public void UpdateContact(string name, InfoContact infoContact)
        {
            Identification = infoContact;
        }
    }
}
