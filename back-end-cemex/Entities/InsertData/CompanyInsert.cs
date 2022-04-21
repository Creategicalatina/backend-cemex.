using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace back_end_cemex.Entities.InsertData
{
    public class CompanyInsert : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(c => c.NitCompany).IsUnique(true);
            builder.HasData(
                new Company
                {
                 IdCompany = 1,
                 NameCompany = "Sevi Transporte",
                 NitCompany = "123121-212"
                },
                new Company
                {
                    IdCompany = 2,
                    NameCompany = "Entregas SAS",
                    NitCompany = "34341-982"
                },
                new Company
                {
                    IdCompany = 3,
                    NameCompany = "Carga Segura",
                    NitCompany = "431231-12"
                }
            );
        }
    }
}
