using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Authors;

public class Author : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; private set; }

    public DateTime BirthDate { get; set; }

    public string? ShortBio { get; set; }

private Author(){}


internal Author(Guid id, [NotNull] string name, DateTime birthDate, [CanBeNull] string? shortBio = null) : base(id)
{
    
    
    SetName(name);
    BirthDate = birthDate;
    ShortBio = shortBio;
}

    private void SetName([NotNull] string name) 
        => Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 64);

    internal Author ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }
}



