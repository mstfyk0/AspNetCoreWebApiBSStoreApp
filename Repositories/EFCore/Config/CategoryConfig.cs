﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId); //PK;    
            builder.Property(c => c.CategoryName).IsRequired(); //Boş geçilemez;


            builder
               .HasData(
                   new Category { CategoryId = 1, CategoryName = "Book" },
                   new Category { CategoryId = 2, CategoryName = "Computer science" },
                   new Category { CategoryId = 3, CategoryName = "Data science" }
                   );
        }
    }
}
