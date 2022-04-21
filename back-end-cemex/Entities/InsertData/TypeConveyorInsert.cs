using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_end_cemex.Entities.InsertData
{
    public class TypeConveyorInsert : IEntityTypeConfiguration<TypeConveyor>
    {
        public void Configure(EntityTypeBuilder<TypeConveyor> builder)
        {
            builder.HasData(
                new TypeConveyor
                {  
                    IdTypeConveyor = 1,
                    NameTypeConveyor = "AdminLogis",
                    DescriptionTypeConveyor = "Son empresas de transporte de carga que cuentan con una flota que supera los 5 camiones."

                },
                new TypeConveyor
                {
                    IdTypeConveyor = 2,
                    NameTypeConveyor = "ManTruck",
                    DescriptionTypeConveyor = "Son microempresas de transporte que tienen entre 1 a 5 camiones en su flota."

                },
                 new TypeConveyor
                 {
                     IdTypeConveyor = 3,
                     NameTypeConveyor = "Driver",
                     DescriptionTypeConveyor = "Encargado de conducir el camión y su responsabilidad es cumplir con el itinerario de viajes asignados."

                 }
            );
        }
    }
}
