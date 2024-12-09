using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Published { get; set; }
    public int PublishedYear { get; set; }
    public string PublisherName { get; set; }
    public string ISBN { get; set; }
}
