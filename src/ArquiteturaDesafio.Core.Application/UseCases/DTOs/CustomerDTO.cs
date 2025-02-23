using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InfoContactDTO Identification { get; set; }

        public DateTime DateCreated { get; internal set; }
        public DateTime? DateUpdated { get; internal set; }
        public void setIdContext(Guid id)
        {
            Id = id;
        }
    }
}
